import {EthernetFrame} from "./ethernet_frame"
import {Port} from "./port"

export interface IPortContainer {
    on_receive(tup: [EthernetFrame, EthernetFrame], port?: Port)
}