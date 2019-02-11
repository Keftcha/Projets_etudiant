export class ipv4 {
    address: number
    mask: number
    network: number
    wildcard: number
    broadcast: number

    constructor(str: string=null) {
        [this.address, this.mask] = ipv4.parse(str)
        this.network = ipv4.network_addresse(this)
        this.wildcard = ipv4.wildcard_addresse(this)
        this.broadcast = ipv4.broadcast_addresse(this)
    }

    static parse(str: string) {
        if (str != null) {
            // assure we only have 1 mask
            if (str.split("/").length > 2) {
                throw new Error("Invalid ip address.")
            }
            
            let [ip, mask] = str.split("/")

            // get the ip
            // assure we only have 4 elements in the addresse
            let ip_addr = ip.split(".")
            if (ip_addr.length != 4) {
                throw new Error("Invalid ip address.")
            }

            // assure we have 4 numbers between 0 and 255
            let addr = []
            for (let nb of ip_addr) {
                try {
                    let nombre = Number(nb)
                    if (nombre < 0 || nombre > 255) {
                        throw new Error()
                        // nothing in the error because we are in try catch
                    }
                    addr.push(nombre)
                } catch {
                    throw new Error("Invalid ip address.")
                }
            }

            let address = (addr[0]<<24) | (addr[1]<<16) | (addr[2]<<8) | (addr[3])

            // get the mask
            let masque
            if (mask != undefined && mask != "") {
                // assure the mask is valid and a number
                try{
                    let nombre = Number(mask)
                    if (nombre < 0 || nombre > 32) {
                        throw new Error()
                    }
                    let bin_mask = "1".repeat(nombre) + "0".repeat(32 - nombre)    // string of the binary mask
                    masque = parseInt(bin_mask, 2)
                } catch {
                    throw new Error("Invalid mask")
                }
            } else {
                masque = null
            }

            return [address, masque]

        } else {
            return [null, null]
        }
    }
    
    static network_addresse(ipa: ipv4) {
        return ipa == null ? null : ipa.address & ipa.mask
    }

    static wildcard_addresse(ipa: ipv4) {
        return ipa != null ? ~ipa.mask : null
    }

    static broadcast_addresse(ipa: ipv4) {
        return ipa != null ? ipa.address | ipa.wildcard : null
    }

    toString() {
        let str = "There is no ip addresse"
        if (this.address != null) {
            str = ipv4.toAddress(this.address)
        }
        if (this.mask != null) {
            str += "/" + String(this.mask.toString(2).split("1").length - 1)
        }
        return str
    }

    static toString(nombre: number) {
        if (nombre == null) {
            throw new Error("The ip given is null")
        }
        // Convert number to ip addresse but this isn't a realy ip because we don't have mask
        return ipv4.toAddress(nombre)
    }

    private static toAddress(nombre: number) {
        return(
            String((nombre >> 24) & 255) + "." +
            String((nombre >> 16) & 255) + "." +
            String((nombre >> 8) & 255) + "." +
            String(nombre & 255)
        )
    }

    static compare(ip1: ipv4, ip2: ipv4) {
        return ip1.network === ip2.network && ip1.address === ip2.address
    }

    static on_same_network(ip1: ipv4, ip2: ipv4) {
        return ip1.network === ip2.network && ip1.mask === ip2.mask
    }
}