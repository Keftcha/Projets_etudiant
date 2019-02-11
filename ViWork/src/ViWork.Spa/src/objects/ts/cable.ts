import { Port } from './port'
import { EthernetFrame } from './ethernet_frame'

export class Cable {
    // A cable reference two other things
    // a cable can be crossed or not, this represented by a boolean
    // a cable transmit paquets with tuple, one element if for talk and the second for listen

    branched: [Port, Port]    // reference the two machines branched
    cross_eh: boolean    // is the clable crossed or not ?
    wire: [EthernetFrame, EthernetFrame]    // represent the two wire in cable, une for write, the second to listen

    constructor(is_cross: boolean=true, port_a: Port=null, port_b: Port=null) {
        this.cross_eh = is_cross
        this.branched = [null, null]
        port_a != null ? this.plug(port_a): null
        port_b != null ? this.plug(port_b): null
        //this.branched = [port_a, port_b]
        this.wire = [null, null]
    }

    get Wire(): [EthernetFrame, EthernetFrame] {
        // if the cable is crossed we have to reverse the two wire
        // i chose to do this here be we can do this in the setter  
        return this.cross_eh ? [this.wire[1], this.wire[0]] : this.wire
    }

    send(tup: [EthernetFrame, EthernetFrame], port: Port){
        this.wire = tup
        let my_index = this.branched.indexOf(port)
        // the mystic line
        this.branched[my_index ? 0 : 1] != null ? this.branched[Number(!my_index)].new_message() : null
    }

    plug(port: Port) {
        if (this.branched[0] != null && this.branched[1] != null) {    // the cable if branched in two other port
            throw new Error("How could you do this ! The cable is already branched to two ports, are you bling or what !")
        }
        port.on_plug(this)
        this.branched[this.branched.indexOf(null)] = port
        
    }

    on_plug(port: Port) {
        if (this.branched[0] != null && this.branched[1] != null) {    // the cable if brancher in two other port
            throw new Error("How could you do this ! The cable is already branched to two ports, are you bling or what !")
        }
        this.branched[this.branched.indexOf(null)] = port
    }

    unplug(port: Port) {
        if (this.branched.indexOf(port) === -1) {
            throw new Error("Dude, the port is not connecter to this cable, are you high ?")
        }
        port.on_unplug(this)
        this.branched[this.branched.indexOf(port)] = null
    }

    on_unplug(port: Port) {
        if (this.branched.indexOf(port) === -1) {
            throw new Error("Bro, this cable isn't connected to this port...")
        }
        this.branched[this.branched.indexOf(port)] = null
    }

}
