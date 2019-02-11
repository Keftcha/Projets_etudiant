import "mocha"
import {assert} from "chai"

import {Switch} from "../../js/commutateur"
import {Computer} from "../../js/computer"
import {Cable} from "../../js/cable"

describe("Create a switch and send some paquets", function() {
    let sw = new Switch()

    let cmp1 = new Computer()
    cmp1.network_cards[0].paquet_filter = false
    let cmp2 = new Computer()
    let cmp3 = new Computer()

    let cab1 = new Cable(false, cmp1.network_cards[0].port, sw.ports[0])
    let cab2 = new Cable(false, cmp2.network_cards[0].port, sw.ports[1])
    let cab3 = new Cable(false, cmp3.network_cards[0].port, sw.ports[2])

    it("cmp3 send message to cmp2 but dut to the empty cam table and the cmp&'s network card config cmp1 and cmp2 get the package", function() {
        cmp3.send_thing("thing", cmp2.network_cards[0].mac_addr, 0)

        assert.strictEqual(cmp2.last_recv, "thing")
        assert.strictEqual(cmp1.last_recv, "thing")
    })
    it("the cmp2 send a message, so he is un the cam table of the switch", function() {
        cmp2.send_thing("pouet", cmp1.network_cards[0].mac_addr, 0)

        assert.deepEqual(cmp2.network_cards[0].mac_addr, sw.cam[1][0])
    })
    it("cmp3 send message to cmp2 and the switch know who is cmp2 so, cmp1 don't receive the paquet", function() {
        cmp3.send_thing("poulet", cmp2.network_cards[0].mac_addr, 0)

        assert.strictEqual(cmp1.last_recv, "pouet")
        assert.strictEqual(cmp2.last_recv, "poulet")
    })
    it("the sender don't get his paquet back because the switch don't resent the paquet to his sender unless he know where to find him", function() {
        cmp1.last_recv = null

        cmp1.send_thing("Flon", cmp2.network_cards[0].mac_addr, 0)
        assert.isNull(cmp1.last_recv)

        cmp1.send_thing("lettre", cmp1.network_cards[0].mac_addr, 0)
        assert.strictEqual(cmp1.last_recv, "lettre")
    })
})

describe("Create some vlan", function() {
    it ("Unauhtorised vlan name", function() {
        let sw = new Switch()
        let fn = function() {sw.vlan_add("vlan0", [1, 2])}
        assert.throws(fn, Error)
    })

    it("Create one vlan", function() {
        let sw = new Switch(10)
        sw.vlan_add("vl1", [1, 2, 3])
        assert.deepEqual(sw.vlan["vl1"], [1, 2, 3])
        assert.deepEqual(sw.vlan["vlan0"], [0, 4, 5, 6, 7, 8, 9])
    })

    it("Create two vlan", function() {
        let sw = new Switch(10)
        sw.vlan_add("vl1", [1, 2, 3])
        sw.vlan_add("vl1", [4])
        sw.vlan_add("vl2", [9, 8, 7])
        assert.deepEqual(sw.vlan, {"vlan0": [0, 5, 6], "vl1": [1, 2, 3, 4], "vl2": [9, 8, 7]})
    })

    it("Create vlan with one port in two vlan", function() {
        let sw = new Switch(10)
        sw.vlan_add("vl1", [1, 2, 3])
        sw.vlan_add("vl2", [5, 6, 1])
        assert.deepEqual(sw.vlan, {"vlan0": [0, 4, 7, 8, 9], "vl1": [1, 2, 3], "vl2": [5, 6, 1]})
    })
    
    it("Out of range port number", function() {
        let sw = new Switch()
        let fn = function() {sw.vlan_add("vl1", [9])}
        assert.throw(fn, Error)
        fn = function() {sw.vlan_add("vl1", [-4])}
        assert.throw(fn, Error)
    })
})

describe("Remove some vlan", function() {
    it("Unauthorized vlan name", function() {
        let sw = new Switch()
        let fn = function() {sw.vlan_remove("vlan0", [1, 2])}
        assert.throws(fn, Error)
    })

    it("Remove a port in vlan but the vlan not exist", function() {
        let sw = new Switch()
        let fn = function() {sw.vlan_remove("vl1", [3, 4])}
        assert.throw(fn, Error)
    })

    it("Remove port which isn't in vlan", function() {
        let sw = new Switch()
        sw.vlan_add("vl1", [1, 2, 3])
        let fn = function() {sw.vlan_remove("vl1", [0])}
        assert.throw(fn, Error)
    })

    it("Remove one port of a vlan", function() {
        let sw = new Switch()
        sw.vlan_add("vl1", [0, 1, 2, 3])
        sw.vlan_remove("vl1", [0])
        assert.deepEqual(sw.vlan, {"vlan0": [4, 0], "vl1": [1, 2, 3]})
    })

    it("Remove one port of only one of him vlan", function() {
        let sw = new Switch()
        sw.vlan_add("vl1", [1, 2, 3])
        sw.vlan_add("vl2", [0, 1, 4])
        sw.vlan_remove("vl2", [1])
        assert.deepEqual(sw.vlan, {"vlan0": [], "vl1": [1, 2, 3], "vl2": [0, 4]})
    })
})