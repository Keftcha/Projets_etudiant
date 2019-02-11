import {IEthernetPayload} from "./IEthernetPayload"

export class EthernetFrame {
    destination: number
    source: number
    protocol_layer3: number
    content: IEthernetPayload
    crc: string

    constructor(
        destination: number,
        source: number,
        protocol_layer3: number=null,
        content: IEthernetPayload,
        crc: string=null
    ) {
        this.destination = destination
        this.source = source
        this.protocol_layer3 = protocol_layer3
        this.content = content
        this.crc = crc
    }
}