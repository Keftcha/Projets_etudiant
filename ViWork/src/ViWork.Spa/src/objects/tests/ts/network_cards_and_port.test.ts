import "mocha"
import {assert} from "chai"

import {NetworkCard} from "../../js/network_card"
import {Computer} from "../../js/computer"
import {Cable} from "../../js/cable"
import {Hub} from "../../js/Hub"

describe("Create network card", function() {
    it("Create network card should create new port which reference the network card", function() {
        let card = new NetworkCard()
        assert.isNotNull(card.port)
        assert.strictEqual(card.port.contenair, card)
    })
})

// Config the network card with paquet filter parametre
describe("Config the network card", function() {
    let hub = new Hub()

    let cmp1 = new Computer()
    cmp1.network_cards[0].paquet_filter = false
    let cmp2 = new Computer()
    let cmp3 = new Computer()

    let cab1 = new Cable(false, cmp1.network_cards[0].port, hub.ports[0])
    let cab2 = new Cable(false, cmp2.network_cards[0].port, hub.ports[1])
    let cab3 = new Cable(false, cmp3.network_cards[0].port, hub.ports[2])

    it("cmp3 send message to cmp2 but due to cmp1's network_card config cmp1 and cmp2 got the message", function() {
        cmp3.send_thing("thing", cmp2.network_cards[0].mac_addr, 0)
        
        assert.strictEqual(cmp2.last_recv, "thing")
        assert.strictEqual(cmp1.last_recv, "thing")
    })
    it("cmp3 send message to cmp1 but due to cmp2's network_card config only cmp1 got the message", function() {
        cmp3.send_thing("poulet", cmp1.network_cards[0].mac_addr, 0)

        assert.strictEqual(cmp2.last_recv, "thing")
        assert.strictEqual(cmp1.last_recv, "poulet")
    })
    it("the sender don't get his paquet back because the hub don't resent the paquet to his sender", function() {
        cmp1.last_recv = null
        cmp1.send_thing("Flon", cmp2.network_cards[0].mac_addr, 0)
        cmp1.send_thing("lettre", cmp1.network_cards[0].mac_addr, 0)

        assert.isNull(cmp1.last_recv)
        assert.isNull(cmp1.last_recv)
    })
})