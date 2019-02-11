import {IIpPayload} from "./IIpPayload";

export class icmp_paquet implements IIpPayload {
    // a ICMP paquet is in a datagram ip
    // French: https://fr.wikipedia.org/wiki/Internet_Control_Message_Protocol
    // English: https://en.wikipedia.org/wiki/Internet_Control_Message_Protocol

    icmp_eh: boolean
    type: Number
    code: Number
    check_sum: string
    content: string

    constructor(type: Number, code: Number, content: string) {
        this.icmp_eh = true
        this.type = type
        this.code = code
        this.check_sum = null
        this.content = content
    }
}