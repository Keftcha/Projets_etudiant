import "mocha"
import {assert} from "chai"

import {Computer} from "../../js/computer"
import {Hub} from "../../js/hub"
import {Cable} from "../../js/cable"
import {Switch} from "../../js/commutateur"

// One computer send message to a second but the third don't receive it
describe("Send message to one computer but not the others", function() {
    let cmp1 = new Computer()
    let cmp2 = new Computer()
    let cmp3 = new Computer()
    let hub = new Hub()
    let cab1 = new Cable(false, cmp1.network_cards[0].port, hub.ports[0])
    let cab2 = new Cable(false, cmp2.network_cards[0].port, hub.ports[1])
    let cab3 = new Cable(false, cmp3.network_cards[0].port, hub.ports[2])

    cmp1.send_thing("fun", cmp2.network_cards[0].mac_addr, 0)

    it("The content of the last recv in cmp3 shouldn't fun, because null is the opposite of fun", function() {
        assert.isNull(cmp3.last_recv)
    })
    it("The content of the last recv in cmp2 should be fun", function() {
        assert.strictEqual(cmp2.last_recv, "fun")
    })
})

// One computer send message to another but they aren't in the same lane, so he don't receive the paquet
describe("Send message to computher which isn't in the same vlan of us", function() {
    let cmp1 = new Computer()
    let cmp2 = new Computer()
    let sw = new Switch()
    let cab1 = new Cable(false, cmp1.network_cards[0].port, sw.ports[0])
    let cab2 = new Cable(false, cmp2.network_cards[0].port, sw.ports[1])

    sw.vlan_add("vl1", [0])
    sw.vlan_add("vl2", [1])

    cmp1.send_thing("thing", cmp2.network_cards[0].mac_addr, 0)

    it("cmp2 don't get thing and the switch don't know cmp2", function() {
        assert.isNull(cmp2.last_recv)
    })
    it("cmp1 don't get thing and the switch know cmp1", function() {
        assert.isNull(cmp1.last_recv)
    })
})

describe("Send message to computer which is in the same vlan of us", function() {
    let cmp1 = new Computer()
    let cmp2 = new Computer()
    let cmp3 = new Computer()
    let cmp4 = new Computer()
    let sw = new Switch(7)
    let cab1 = new Cable(false, cmp1.network_cards[0].port, sw.ports[0])
    let cab2 = new Cable(false, cmp2.network_cards[0].port, sw.ports[1])
    let cab3 = new Cable(false, cmp3.network_cards[0].port, sw.ports[4])
    let cab4 = new Cable(false, cmp4.network_cards[0].port, sw.ports[5])

    sw.vlan_add("vl1", [0, 1, 2])

    cmp1.send_thing("thing", cmp2.network_cards[0].mac_addr, 0)
    cmp1.send_thing("thing", cmp3.network_cards[0].mac_addr, 0)

    it("cmp2 get the thing of cmp1", function() {
        assert.strictEqual(cmp2.last_recv, "thing")
    })
    it("cmp3 don't get the thing of cmp1 but he get the pouet of cmp4", function() {
        assert.isNull(cmp3.last_recv)
        cmp4.send_thing("pouet", cmp3.network_cards[0].mac_addr, 0)
        assert.strictEqual(cmp3.last_recv, "pouet")
    })
})

describe("Send message to broadcast", function() {
    let cmp1 = new Computer()
    let cmp2 = new Computer()
    let cable = new Cable(true, cmp1.network_cards[0].port, cmp2.network_cards[0].port)

    cmp1.send_thing("thing", 0xFFFFFFFFFFFF, 0)

    it("cmp2 get the broadcast message", function() {
        assert.strictEqual("thing", cmp2.last_recv)
    })
})

describe("One computer have his network card in bad configuration", function() {
    let cmp1 = new Computer(1, false, 1)
    let cmp2 = new Computer(1, false, 0)
    let cab1 = new Cable(true)
    let cab2 = new Cable(false)

    it("cmp2 don't have the paquet because his network card configuration is totaly broken", function() {
        // plug the cable in computers
        cab1.plug(cmp1.network_cards[0].port)
        cab1.plug(cmp2.network_cards[0].port)

        cmp1.send_thing("¤", cmp2.network_cards[0].mac_addr, 0)
        assert.isNull(cmp2.last_recv)
    })
    it("A bad network configuration and a straight cable between two computer should transmit the message", function() {
        // unplug to the first cable
        cab1.unplug(cmp1.network_cards[0].port)
        cab1.unplug(cmp2.network_cards[0].port)
        // plug to the second cable
        cab2.plug(cmp1.network_cards[0].port)
        cab2.plug(cmp2.network_cards[0].port)

        cmp2.send_thing("<°)))<", cmp1.network_cards[0].mac_addr, 0)
        assert.strictEqual("<°)))<", cmp1.last_recv)
    })
})