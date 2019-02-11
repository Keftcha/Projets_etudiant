import {IPortContainer} from "./IPortContainer"
import {Port} from "./port"
import { EthernetFrame } from "./ethernet_frame";


export class Hub implements IPortContainer {
    // if you don't know what is a hub see this: https://en.wikipedia.org/wiki/Ethernet_hub
    // the hub write paquet in the element 0 of a cable and listen in the element 1

    nb_ports: number
    ports: Array<Port>    // this list of port reference cable plugin in

    constructor(nb_ports: number = 5) {
        if (nb_ports < 3) {
            throw new RangeError("A hub must have more than 3 ports. Otherwise it's a little useless...")
        }
        this.nb_ports = nb_ports
        this.ports = []
        for (let idx = 0; idx < this.nb_ports; idx += 1) {this.ports.push(new Port(this))}
    }

    on_receive(tup: [EthernetFrame, EthernetFrame], port: Port) {
        let paquet = tup[1]
        this.send(paquet, port)
    }

    send(paquet: EthernetFrame, current_port: Port){
        if (paquet != null) {
            this.ports.forEach(
                port => {
                    if (port != current_port) {
                        port.send(paquet, 0)    // write the paquet in element 0 and don't change what the cable contain in element 1
                    }
                }
            )   
        }
    }
}