import {Cable} from "./cable"
import {IPortContainer} from "./IPortContainer"
import {EthernetFrame} from "./ethernet_frame"

export class Port {
    contenair: IPortContainer
    cable: Cable

    constructor(contenair: IPortContainer=null) {
        this.contenair = contenair
        this.cable = null
    }

    plug(cable: Cable) {
        if (this.cable != null) {
            throw new Error("There is already a cable in this port ! Don't you see it ?")
        }
        cable.on_plug(this)
        this.cable = cable
    }

    on_plug(cable: Cable) {
        if (this.cable != null) {
            throw new Error("A cable is here why did you try this ? (wtf dude, what are you doing ?).")
        }
        this.cable = cable
    }
    
    unplug() {
        if (this.cable == null) {
            throw new Error("WOAW there is nothing here ! How can you expect remove something to nothing ?")
        }
        this.cable.on_unplug(this)
        this.cable = null
    }

    on_unplug(cable: Cable) {
        if (this.cable != cable) {
            throw new Error("You trying to remove a cable which isn't in this port.")
        }
        this.cable = null
    }

    new_message() {
        this.contenair.on_receive(this.cable.Wire, this)
        this.cable.wire = [null, null]    // remove the paquet in the cable
    }

    send(paquet: EthernetFrame, wire: number) {
        if (this.cable != null) {
            this.cable.send(wire ? [this.cable.wire[0], paquet] : [paquet, this.cable.wire[1]], this)
        }
    }
}