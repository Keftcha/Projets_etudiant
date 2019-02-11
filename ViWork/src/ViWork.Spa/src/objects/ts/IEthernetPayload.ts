import {ipv4} from "./ipv4"
import {IIpPayload} from "./IIpPayload"

export interface IEthernetPayload {
    ip_src: ipv4
    ip_dest: ipv4
    content: IIpPayload | String
    ttl: number
}