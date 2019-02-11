import "mocha"
import {assert} from "chai"

import {Cable} from "../../js/cable"
import {Computer} from "../../js/computer"
import {Hub} from "../../js/hub"

// Test the cross of cable

// Crossed cable between two computer
describe("Create crossed cable and send paquet between two computer", function() {
    let c1 = new Computer()
    let c2 = new Computer()
    let cable = new Cable(true, c1.network_cards[0].port, c2.network_cards[0].port)

    c1.send_thing("thing", c2.network_cards[0].mac_addr, 0)

    it("The content of the last recv on c2 shoult \"thing\"", function() {
        assert.strictEqual(c2.last_recv, "thing")
    })
})

// Uncrossed cable between two computer
describe("Create uncrossed cable and send paquet between two computer", function() {
    let c1 = new Computer() 
    let c2 = new Computer() 
    let cable = new Cable(false, c1.network_cards[0].port, c2.network_cards[0].port)

    c2.send_thing("thing", c1.network_cards[0].mac_addr, 0)

    it("The content of the last recv on c1 should null", function() {
        assert.isNull(c1.last_recv)
    })
})


// Uncrossed cable between computer and hub, hub and another computer
describe("Create uncrossed cable between hub and computers", function() {
    let cmp1 = new Computer()
    let cmp2 = new Computer()
    let hub = new Hub()
    let cab1 = new Cable(false, cmp1.network_cards[0].port, hub.ports[0])
    let cab2 = new Cable(false, hub.ports[1], cmp2.network_cards[0].port)

    cmp1.send_thing("thing", cmp2.network_cards[0].mac_addr, 0)

    it("The content of the last recv on c2 should \"thing\"", function() {
        assert.strictEqual(cmp2.last_recv, "thing")
    })
})

// Crossed cable between cmp1 and hub, uncrossed cable between hub ans cmp2
describe("Create crossed cable between one computer and the hub, and uncrossed cable between the hub and the other computer", function() {
    let cmp1 = new Computer()
    let cmp2 = new Computer()
    let hub = new Hub()
    let cab1 = new Cable(true, cmp1.network_cards[0].port, hub.ports[0])
    let cab2 = new Cable(false, hub.ports[1], cmp2.network_cards[0].port)

    cmp1.send_thing("thing", cmp2.network_cards[0].mac_addr, 0)

    it("The content of the last recv in c2 should not fun", function() {
        assert.isNull(cmp2.last_recv)
    })
})

// Uncrossed cable between cmp1 and hub, crossed cable between hub ans cmp2
describe("Create uncrossed cable between one computer and the hub, and crossed cable between the hub and the other computer", function() {
    let cmp1 = new Computer()
    let cmp2 = new Computer()
    let hub = new Hub()
    let cab1 = new Cable(false, cmp1.network_cards[0].port, hub.ports[0])
    let cab2 = new Cable(true, hub.ports[1], cmp2.network_cards[0].port)

    cmp1.send_thing("thing", cmp2.network_cards[0].mac_addr, 0)

    it("The content of the last recv in c2 should not fun", function() {
        assert.isNull(cmp2.last_recv)
    })
})

// Crossed cable between cmp1 and hub, crossed cable between hub ans cmp2
describe("Create crossed cable between computers and the hub", function() {
    let cmp1 = new Computer()
    let cmp2 = new Computer()
    let hub = new Hub()
    let cab1 = new Cable(true, cmp1.network_cards[0].port, hub.ports[0])
    let cab2 = new Cable(true, hub.ports[1], cmp2.network_cards[0].port)

    cmp1.send_thing("thing", cmp2.network_cards[0].mac_addr, 0)

    it("The content of the last recv in c2 should not fun", function() {
        assert.isNull(cmp2.last_recv)
    })
})

// Send message in cable which is pluget in only one comlputer
describe("Send message in cable which is branched in only one computer", function() {
    let cmp = new Computer()
    let cable = new Cable(true, cmp.network_cards[0].port, null)

    let fn = cmp.send_thing("thing", cmp.network_cards[0].mac_addr + 1, 0)
})