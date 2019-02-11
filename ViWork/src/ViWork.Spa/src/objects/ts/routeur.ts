import {IPortContainer} from "./IPortContainer"
import {Roucom} from "./roucom"

export class Routeur extends Roucom implements IPortContainer {
    // in case you don't know what a routeur is: http://lmgtfy.com/?s=d&iie=1&q=routeur+wikipedia
    // network_cards of routeur write paquet in the element 0 of a cable and listen in the element 1

    constructor(nb_network_card: number=2, cable: number=0) {
        if (nb_network_card < 2) {
            throw Error("A routeur with less than two interfaces is like a car with out engine,\nit's pretty but useless")
        }
        
        super(nb_network_card, true, cable)
    }
    on_receive(){}
    
}