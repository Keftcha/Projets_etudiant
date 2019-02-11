import {IPortContainer} from "./IPortContainer"
import {Port} from "./port"
import {EthernetFrame} from "./ethernet_frame"
import {IEthernetPayload} from "./IEthernetPayload"
import {Roucom} from "./roucom"
import {ipv4} from "./ipv4"
import {IIpPayload} from "./IIpPayload"
import {ip_paquet} from "./ip_paquet"
import {icmp_paquet} from "./icmp_paquet"

export class NetworkCard implements IPortContainer {
    static last_avariable_mac_addr: number = 0x0
    ip_addr: ipv4
    readonly mac_addr: number
    port: Port
    roucom: Roucom
    paquet_filter: boolean
    write_on_cable: number
    // network_cards write paquet in the element <write_on_cable> of a cable and listen in the other cable

    constructor(cmp: Roucom=null, paquet_filter: boolean=true, write_on_cable: number=1) {
        // the paquet_filter is for layer two only (on the mac address)
        // default write_on_cable is for computer
        this.mac_addr = NetworkCard.last_avariable_mac_addr + 1
        NetworkCard.last_avariable_mac_addr += 1
        this.port = new Port(this)
        this.ip_addr = new ipv4()
        this.roucom = cmp
        this.paquet_filter = paquet_filter
        this.write_on_cable = write_on_cable
    }

    on_receive(cable_content: [EthernetFrame, EthernetFrame], port: Port) {
        let frame = cable_content[(this.write_on_cable+1)%2]
        if (frame != null && (!this.paquet_filter || (frame.destination === this.mac_addr || frame.destination === 0xFFFFFFFFFFFF))) {

            // check the protocole
            if (frame.protocol_layer3 === null) {    // no protocole, we are un layer 2
                let payload = this.get_ethernet_payload(frame)
                let content = this.get_payload_content(payload) as string
                this.roucom.arrived(content)
            
            } else if (frame.protocol_layer3 === 0x0800) {    // Internet Protocol version 4 (IPv4)
                let payload = this.get_ethernet_payload(frame)
                if (ipv4.compare(payload.ip_dest, this.ip_addr)) {    // the paquet is for our us
                    // may this a ICMP protocole
                    if (this.get_ip_payload_content(payload).icmp_eh) {
                        let icmp = this.get_ip_payload_content(payload)
                        switch (icmp.type) {
                            case 0:
                                // we recv an echo reply
                                if (icmp.code === 0) {    // check the code
                                    this.roucom.arrived(icmp.content)
                                }
                                break
                            case 8:
                                // we recv an echo request
                                if (icmp.code === 0) {    // check the code of the icmp paquet
                                    this.pong(payload.ip_src)
                                    this.roucom.arrived(icmp.content)
                                }
                                break
                            case 11:
                                // we recv a time out exceeded
                                if (icmp.code === 0) {    // ttl of the paquet i send is equal to 0
                                    this.roucom.arrived(icmp.content)
                                }
                                break
                        }
                    }
                    // may this is for layer 4
                    // else if (this.get_ip_payload_content(payload) as IIpPayload) {
                    // }
                    // may it's for layer 3
                    else {
                        let content = this.get_payload_content(payload) as string
                        this.roucom.arrived(content)
                    }
                } else if (this.roucom.routing_eh) {    // the paquet isn't for us but we route paquets
                    this.route(payload)
                }
            
            } else if (frame.protocol_layer3 === 0x0806) {    // Address Resolution Protocol (ARP)

                // add the ip and the mac address of the source in our arp table
                if (frame.content.ip_src != null && frame.source != null) {
                    this.roucom.arp_table.set(frame.content.ip_src, frame.source)
                }

                let payload = this.get_ethernet_payload(frame)
                if (ipv4.compare(payload.ip_dest, this.ip_addr)) {
                    if (new RegExp("^Who is ([0-9]{1,3}\.){3}[0-9]{1,3}\/([0-9]){1,2} \\?$").test(payload.content as string)) {    // regex of an ipv4
                        this.send("Hey, it's me " + String(this.mac_addr), this.roucom.arp_table.get(payload.ip_src), payload.ip_src, 0X0806)
                    }
                }
            }
        }
    }

    route (paquet: IEthernetPayload) {
        // decrement the ttl of the ip_paquet
        paquet.ttl -= 1

        if (paquet.ttl <= 0) {
            // send ICMP Time exceeded error
            let icmp = new icmp_paquet(11, 0, "Time exceeded.")
            this.send(icmp, null, paquet.ip_src, 0X0800)
        } else {
            // serach who is the gateway to join the network of the destinataire of the paquet
            let gateway = this.roucom.get_gateway(paquet.ip_dest)

            // now, we know the gateway but we have to chose on which network to send the paquet

            let network_card_idx = this.roucom.get_network_card_idx_on_network(gateway)

            // we know on which interface to send the paquet if we don't break before

            if (network_card_idx != null) {
                // make check for the ICMP protocole
                if (false) {//(this.know_the_ip_eh(gateway)) {
                    // Type 3 Code 1 -> unreatchable host
                    let icmp = new icmp_paquet(3, 0, "Unreatchable host.")
                    let payload = this.make_ethernet_payload(icmp, paquet.ip_src, this.ip_addr)
                    this.route(payload)
                } else {
                    // we know on which network card we have to send the paquet
                    let content = this.get_payload_content(paquet)
                    // we don't know to who send the frame, this why the mac_dest is null
                    // but don't worry, the network card will do that
                    this.roucom.network_cards[network_card_idx].send(content, null, paquet.ip_dest, 0x0800, paquet.ttl, paquet.ip_src)
                }
            }
        }
    }

    broadcast_arp(ip: ipv4) {
        this.send("Who is " + ip.toString() + " ?", 0xFFFFFFFFFFFF, ip, 0x0806)
    }

    send(content: any, mac_dest: number, ip_dest: ipv4=null, protocole: number=null, ttl: number=64, ip_src: ipv4=this.ip_addr) {
        // if the mac_dest isn't given probably because we send the paquet on the layer 3
        // we need to search the mac address of the routeur we want to join
        // first, we look in the route table to know which routeur we want to send the frame
        // second, we define the mac_dest to the routeur we find

        if (mac_dest === null) {
            let ip_next_routeur = this.roucom.get_gateway(new ipv4("0.0.0.0/0"))
            // here we search in the route_table
            for (let ip of Array.from(this.roucom.route_table.keys())) {
                if (ipv4.on_same_network(ip_dest, ip)) {
                    ip_next_routeur = this.roucom.get_gateway(ip)
                    break
                }
            }
            
            // if the ip is our ip, that mean we are on the same network of the sestinataire
            // so the ip of the next routeur (which isn't a routeur) is the final destinataire
            if (ipv4.compare(this.ip_addr, ip_next_routeur)) {
                ip_next_routeur = ip_dest
            }

            // here we search the mac address of the next routeur
            // may we don't know the mac adress of the next routeur, so we broadcast arp
            let no_break = true
            for (let ip of Array.from(this.roucom.arp_table.keys())) {
                if (ipv4.compare(ip, ip_next_routeur)) {
                    no_break = false
                    break
                }
            } if (no_break) {
                this.broadcast_arp(ip_next_routeur)
            }

            mac_dest = this.roucom.get_mac_in_arp(ip_next_routeur)
        }

        let payload = this.make_ethernet_payload(content, ip_dest, ip_src, ttl)
        let frame = this.make_ethernet_frame(payload, mac_dest, protocole)
        this.port.send(frame, this.write_on_cable)
    }

    ping(ip_reach: ipv4) {
        let icmp = new icmp_paquet(8, 0, "Echo request - ping")
        this.send(icmp, null, ip_reach, 0x0800)
    }

    pong(ip_reach: ipv4) {
        let icmp = new icmp_paquet(0, 0, "Echo reply - pong")
        this.send(icmp, null, ip_reach, 0X0800)
    }

    know_the_ip_eh(ip: ipv4) {
        return this.roucom.get_mac_in_arp(ip) != null
    }

    make_ethernet_frame(content: IEthernetPayload, destinataire: number, protocole_layer3: number){
        return new EthernetFrame(destinataire, this.mac_addr, protocole_layer3, content, null)
    }

    get_ethernet_payload(frame: EthernetFrame) {
        return frame.content
    }

    make_ethernet_payload(content: IIpPayload, ip_dest: ipv4, ip_src: ipv4, ttl: number=64) {
        return new  ip_paquet(ip_src, ip_dest, content, ttl)
    }

    get_payload_content(payload: IEthernetPayload) {
        return payload.content
    }

    make_ip_payload(){}

    get_ip_payload_content(ip_payload: IIpPayload) {
        return ip_payload.content
    }

    remove() {
        if (this.port.cable != null) {
            this.port.unplug()
        }
        delete(this.port)
    }
}