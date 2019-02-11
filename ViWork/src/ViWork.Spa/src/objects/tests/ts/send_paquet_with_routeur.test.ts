//    this is a computer (nothing to do with the tests)
//
//     _______________________________
//    |                               |
//    |    _______________________    |
//    |   | $_                    |   |
//    |   |                       |   |
//    |   |                       |   |
//    |   |                       |   |
//    |   |                       |   |
//    |   |                       |   |
//    |   |_______________________|   |
//    |                        O.     |
//    |_______________________________|
//               /_________\
//         ________________________
//        | + + + + + + + + + + + |
//        | + + + + + + + + + + + |
//        |_______________________|
//
//


import "mocha"
import {assert} from "chai"

import {new_network} from "./new_network.function_helper.test"

describe("Send message on our local network on layer 3 with mac_address", function() {
    let network = new_network()

    network["cmp00"].send_thing("§", network["routeur"].network_cards[0].mac_addr, 0, network["routeur"].network_cards[0].ip_addr, 0x0800)
    network["cmp00"].send_thing("¤", network["cmp01"].network_cards[0].mac_addr, 0, network["cmp01"].network_cards[0].ip_addr, 0x0800)
    network["cmp01"].send_thing("^", network["cmp02"].network_cards[0].mac_addr, 0, network["cmp02"].network_cards[0].ip_addr, 0x0800)
    network["cmp01"].send_thing("£", network["cmp00"].network_cards[0].mac_addr, 0, network["cmp00"].network_cards[0].ip_addr, 0x0800)

    it("the destinataire got the message", function() {
        assert.strictEqual(network["routeur"].last_recv, "§")
        assert.strictEqual(network["cmp01"].last_recv, "¤")
        assert.strictEqual(network["cmp02"].last_recv, "^")
        assert.strictEqual(network["cmp00"].last_recv, "£")
    })
})

describe("Send message on our local network on layer 3 with out mac_address", function() {
    let network = new_network()

    // we don't know the mac address of the destination
    network["cmp00"].send_thing("§", null, 0, network["routeur"].network_cards[0].ip_addr, 0x0800)
    network["cmp00"].send_thing("¤", null, 0, network["cmp01"].network_cards[0].ip_addr, 0x0800)
    network["cmp01"].send_thing("^", null, 0, network["cmp02"].network_cards[0].ip_addr, 0x0800)
    network["cmp01"].send_thing("£", null, 0, network["cmp00"].network_cards[0].ip_addr, 0x0800)

    it("the destinataire got the message", function() {
        assert.strictEqual(network["routeur"].last_recv, "§")
        assert.strictEqual(network["cmp01"].last_recv, "¤")
        assert.strictEqual(network["cmp02"].last_recv, "^")
        assert.strictEqual(network["cmp00"].last_recv, "£")
    })
})

describe("Send message on other network on layer 3", function() {
    let network = new_network()
    
    network["cmp00"].send_thing("¤", null, 0, network["cmp10"].network_cards[0].ip_addr, 0x0800)
    network["cmp02"].send_thing("$", null, 0, network["cmp21"].network_cards[0].ip_addr, 0x0800)

    network["cmp11"].send_thing("*", null, 0, network["cmp22"].network_cards[0].ip_addr, 0x0800)
    network["cmp10"].send_thing("£", null, 0, network["cmp02"].network_cards[0].ip_addr, 0x0800)

    network["cmp21"].send_thing("µ", null, 0, network["cmp12"].network_cards[0].ip_addr, 0x0800)
    network["cmp20"].send_thing("§", null, 0, network["cmp01"].network_cards[0].ip_addr, 0x0800)

    it("the destinataire got the message", function() {
        assert.strictEqual(network["cmp10"].last_recv, "¤")
        assert.strictEqual(network["cmp21"].last_recv, "$")

        assert.strictEqual(network["cmp22"].last_recv, "*")
        assert.strictEqual(network["cmp02"].last_recv, "£")

        assert.strictEqual(network["cmp12"].last_recv, "µ")
        assert.strictEqual(network["cmp01"].last_recv, "§")
    })

})

describe("when send message, the arp table fill automatically", function () {
    let network = new_network()

    network["cmp00"].send_thing("", null, 0, network["cmp10"].network_cards[0].ip_addr, 0x0800)
    network["cmp02"].send_thing("", null, 0, network["cmp00"].network_cards[0].ip_addr, 0x0800)
    network["cmp00"].send_thing("", null, 0, network["cmp21"].network_cards[0].ip_addr, 0x0800)

    network["cmp12"].send_thing("", null, 0, network["cmp20"].network_cards[0].ip_addr, 0x0800)
    network["cmp12"].send_thing("", null, 0, network["cmp11"].network_cards[0].ip_addr, 0x0800)
    network["cmp12"].send_thing("", null, 0, network["cmp02"].network_cards[0].ip_addr, 0x0800)

    network["cmp21"].send_thing("", null, 0, network["cmp10"].network_cards[0].ip_addr, 0x0800)
    network["cmp21"].send_thing("", null, 0, network["cmp20"].network_cards[0].ip_addr, 0x0800)
    network["cmp21"].send_thing("", null, 0, network["cmp00"].network_cards[0].ip_addr, 0x0800)

    it("the arp table of cmp00", function() {
        let table = new Map()
        table.set(network["routeur"].network_cards[0].ip_addr, network["routeur"].network_cards[0].mac_addr)

        assert.deepEqual(table, network["cmp00"].arp_table)
    })
    it("the arp table of cmp01", function() {
        let table = new Map()
        table.set(network["routeur"].network_cards[0].ip_addr, network["routeur"].network_cards[0].mac_addr)
        table.set(network["cmp00"].network_cards[0].ip_addr, network["cmp00"].network_cards[0].mac_addr)

        assert.deepEqual(table, network["cmp01"].arp_table)
    })
    it("the arp table of cmp02", function() {
        let table = new Map()
        table.set(network["routeur"].network_cards[0].ip_addr, network["routeur"].network_cards[0].mac_addr)
        table.set(network["cmp00"].network_cards[0].ip_addr, network["cmp00"].network_cards[0].mac_addr)

        assert.deepEqual(table, network["cmp02"].arp_table)
    })

    it("the arp table of cmp10", function() {
        let table = new Map()
        table.set(network["routeur"].network_cards[1].ip_addr, network["routeur"].network_cards[1].mac_addr)
        table.set(network["cmp12"].network_cards[0].ip_addr, network["cmp12"].network_cards[0].mac_addr)

        assert.deepEqual(table, network["cmp10"].arp_table)
    })
    it("the arp table of cmp11", function() {
        let table = new Map()
        table.set(network["routeur"].network_cards[1].ip_addr, network["routeur"].network_cards[1].mac_addr)
        table.set(network["cmp12"].network_cards[0].ip_addr, network["cmp12"].network_cards[0].mac_addr)

        assert.deepEqual(table, network["cmp11"].arp_table)
    })
    it("the arp table of cmp12", function() {
        let table = new Map()
        table.set(network["routeur"].network_cards[1].ip_addr, network["routeur"].network_cards[1].mac_addr)
        table.set(network["cmp11"].network_cards[0].ip_addr, network["cmp11"].network_cards[0].mac_addr)

        assert.deepEqual(table, network["cmp12"].arp_table)
    })

    it("the arp table of cmp20", function() {
        let table = new Map()
        table.set(network["routeur"].network_cards[2].ip_addr, network["routeur"].network_cards[2].mac_addr)
        table.set(network["cmp21"].network_cards[0].ip_addr, network["cmp21"].network_cards[0].mac_addr)

        assert.deepEqual(table, network["cmp20"].arp_table)
    })
    it("the arp table of cmp21", function() {
        let table = new Map()
        table.set(network["routeur"].network_cards[2].ip_addr, network["routeur"].network_cards[2].mac_addr)
        table.set(network["cmp20"].network_cards[0].ip_addr, network["cmp20"].network_cards[0].mac_addr)

        assert.deepEqual(table, network["cmp21"].arp_table)
    })
    it("the arp table of cmp22", function() {
        let table = new Map()
        table.set(network["routeur"].network_cards[2].ip_addr, network["routeur"].network_cards[2].mac_addr)
        table.set(network["cmp21"].network_cards[0].ip_addr, network["cmp21"].network_cards[0].mac_addr)

        assert.deepEqual(table, network["cmp22"].arp_table)
    })

    it("the arp table of the routeur", function() {
        let table = new Map()
        table.set(network["cmp00"].network_cards[0].ip_addr, network["cmp00"].network_cards[0].mac_addr)
        table.set(network["cmp02"].network_cards[0].ip_addr, network["cmp02"].network_cards[0].mac_addr)
        table.set(network["cmp10"].network_cards[0].ip_addr, network["cmp10"].network_cards[0].mac_addr)
        table.set(network["cmp12"].network_cards[0].ip_addr, network["cmp12"].network_cards[0].mac_addr)
        table.set(network["cmp20"].network_cards[0].ip_addr, network["cmp20"].network_cards[0].mac_addr)
        table.set(network["cmp21"].network_cards[0].ip_addr, network["cmp21"].network_cards[0].mac_addr)

        assert.deepEqual(table, network["routeur"].arp_table)
    })
})