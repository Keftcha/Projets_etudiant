import {IEthernetPayload} from "./IEthernetPayload"
import {IIpPayload} from "./IIpPayload"
import {ipv4} from "./ipv4"

export class ip_paquet implements IEthernetPayload {
    ip_src: ipv4
    ip_dest: ipv4
    content: IIpPayload
    ttl: number

    constructor(
        ip_src: ipv4,
        ip_dest: ipv4,
        content: IIpPayload,
        ttl: number=64
    ) {
        this.ip_src = ip_src
        this.ip_dest = ip_dest
        this.content = content
        this.ttl = ttl
    }
}