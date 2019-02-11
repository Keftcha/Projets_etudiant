import {Roucom} from "./roucom"
import {ipv4} from "./ipv4"

export class Computer extends Roucom{
    // network_cards of computer write paquet in the element 1 of a cable and listen in the element 0
    constructor(nb_network_card: number=1, route_eh: boolean=false, write_on_cable: number=1) {
        super(nb_network_card, route_eh, write_on_cable)
    }

    ping(network_card: number=0, ip_addr: ipv4, number: number=1) {
        this.network_cards[network_card].ping(ip_addr)
    }
}