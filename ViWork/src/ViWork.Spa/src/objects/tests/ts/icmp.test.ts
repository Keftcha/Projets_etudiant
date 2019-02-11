import "mocha"
import {assert} from "chai"

import {ipv4} from "../../js/ipv4"
import {new_network} from "./new_network.function_helper.test"
import {Routeur} from "../../js/routeur";
import {Computer} from "../../js/computer";
import {Cable} from "../../js/cable";

describe("ping - pong", function() {
    let network = new_network()

    it("cmp00 ping cmp11", function() {
        network["cmp00"].ping(0, new ipv4("192.168.1.2/24"))

        assert.strictEqual(network["cmp11"].last_recv, "Echo request - ping")
        assert.strictEqual(network["cmp00"].last_recv, "Echo reply - pong")
    })
    
    it("cmp10 ping cmp12", function() {
        network["cmp10"].ping(0, network["cmp12"].network_cards[0].ip_addr)

        assert.strictEqual(network["cmp12"].last_recv, "Echo request - ping")
        assert.strictEqual(network["cmp10"].last_recv, "Echo reply - pong")
    })

    it("cmp21 ping cmp02", function() {
        network["cmp21"].ping(0, new ipv4("192.168.0.3/24"))

        assert.strictEqual(network["cmp02"].last_recv, "Echo request - ping")
        assert.strictEqual(network["cmp21"].last_recv, "Echo reply - pong")
    })

    // TODO
    // unretchable machine
})

describe("TTL", function() {
    // create some routeurs
    let rou0 = new Routeur(2)
    rou0.network_cards[0].ip_addr = new ipv4("192.168.0.2/24")
    rou0.network_cards[1].ip_addr = new ipv4("192.168.1.1/24")
    rou0.route_table.set(new ipv4("192.168.0.0/24"), new ipv4("192.168.0.2/24"))
    rou0.route_table.set(new ipv4("192.168.1.0/24"), new ipv4("192.168.1.1/24"))
    rou0.route_table.set(new ipv4("192.168.2.0/24"), new ipv4("192.168.1.2/24"))
    rou0.route_table.set(new ipv4("192.168.3.0/24"), new ipv4("192.168.1.2/24"))
    rou0.route_table.set(new ipv4("192.168.4.0/24"), new ipv4("192.168.1.2/24"))
    rou0.route_table.set(new ipv4("192.168.5.0/24"), new ipv4("192.168.1.2/24"))

    let rou1 = new Routeur(2)
    rou1.network_cards[0].ip_addr = new ipv4("192.168.1.2/24")
    rou1.network_cards[1].ip_addr = new ipv4("192.168.2.1/24")
    rou1.route_table.set(new ipv4("192.168.0.0/24"), new ipv4("192.168.1.1/24"))
    rou1.route_table.set(new ipv4("192.168.1.0/24"), new ipv4("192.168.1.2/24"))
    rou1.route_table.set(new ipv4("192.168.2.0/24"), new ipv4("192.168.2.1/24"))
    rou1.route_table.set(new ipv4("192.168.3.0/24"), new ipv4("192.168.2.2/24"))
    rou1.route_table.set(new ipv4("192.168.4.0/24"), new ipv4("192.168.2.2/24"))
    rou1.route_table.set(new ipv4("192.168.5.0/24"), new ipv4("192.168.2.2/24"))

    let rou2 = new Routeur(2)
    rou2.network_cards[0].ip_addr = new ipv4("192.168.2.2/24")
    rou2.network_cards[1].ip_addr = new ipv4("192.168.3.1/24")
    rou2.route_table.set(new ipv4("192.168.0.0/24"), new ipv4("192.168.2.1/24"))
    rou2.route_table.set(new ipv4("192.168.1.0/24"), new ipv4("192.168.2.1/24"))
    rou2.route_table.set(new ipv4("192.168.2.0/24"), new ipv4("192.168.2.2/24"))
    rou2.route_table.set(new ipv4("192.168.3.0/24"), new ipv4("192.168.3.1/24"))
    rou2.route_table.set(new ipv4("192.168.4.0/24"), new ipv4("192.168.3.2/24"))
    rou2.route_table.set(new ipv4("192.168.5.0/24"), new ipv4("192.168.3.2/24"))

    let rou3 = new Routeur(2)
    rou3.network_cards[0].ip_addr = new ipv4("192.168.3.2/24")
    rou3.network_cards[1].ip_addr = new ipv4("192.168.4.1/24")
    rou3.route_table.set(new ipv4("192.168.0.0/24"), new ipv4("192.168.3.1/24"))
    rou3.route_table.set(new ipv4("192.168.1.0/24"), new ipv4("192.168.3.1/24"))
    rou3.route_table.set(new ipv4("192.168.2.0/24"), new ipv4("192.168.3.1/24"))
    rou3.route_table.set(new ipv4("192.168.3.0/24"), new ipv4("192.168.3.2/24"))
    rou3.route_table.set(new ipv4("192.168.4.0/24"), new ipv4("192.168.4.1/24"))
    rou3.route_table.set(new ipv4("192.168.5.0/24"), new ipv4("192.168.4.2/24"))
    
    let rou4 = new Routeur(2)
    rou4.network_cards[0].ip_addr = new ipv4("192.168.4.2/24")
    rou4.network_cards[1].ip_addr = new ipv4("192.168.5.1/24")
    rou4.route_table.set(new ipv4("192.168.0.0/24"), new ipv4("192.168.4.1/24"))
    rou4.route_table.set(new ipv4("192.168.1.0/24"), new ipv4("192.168.4.1/24"))
    rou4.route_table.set(new ipv4("192.168.2.0/24"), new ipv4("192.168.4.1/24"))
    rou4.route_table.set(new ipv4("192.168.3.0/24"), new ipv4("192.168.4.1/24"))
    rou4.route_table.set(new ipv4("192.168.4.0/24"), new ipv4("192.168.4.2/24"))
    rou4.route_table.set(new ipv4("192.168.5.0/24"), new ipv4("192.168.5.1/24"))

    // create two computer
    let cmp0 = new Computer()
    cmp0.network_cards[0].ip_addr = new ipv4("192.168.0.1/24")
    cmp0.route_table.set(new ipv4("192.168.0.0/24"), new ipv4("192.168.0.1/24"))
    cmp0.route_table.set(new ipv4("0.0.0.0/0"), rou0.network_cards[0].ip_addr)
    let cmp1 = new Computer()
    cmp1.network_cards[0].ip_addr = new ipv4("192.168.5.2/24")
    cmp0.route_table.set(new ipv4("192.168.5.0/0"), new ipv4("192.168.5.2/24"))
    cmp0.route_table.set(new ipv4("0.0.0.0/0"), rou4.network_cards[0].ip_addr)

    // create cable
    let cab0 = new Cable(false, cmp0.network_cards[0].port, rou0.network_cards[0].port)
    let cab1 = new Cable(true, rou0.network_cards[1].port, rou1.network_cards[0].port)
    let cab2 = new Cable(true, rou1.network_cards[1].port, rou2.network_cards[0].port)
    let cab3 = new Cable(true, rou2.network_cards[1].port, rou3.network_cards[0].port)
    let cab4 = new Cable(true, rou3.network_cards[1].port, rou4.network_cards[0].port)
    let cab5 = new Cable(false, rou4.network_cards[1].port, cmp1.network_cards[0].port)

    // look the network we got

    //    ______         ____           ____           ____           ____           ____         ______
    //   | cmp0 |      /      \       /      \       /      \       /      \       /      \      | cmp1 |
    //   |______|=====|  rou0  |=====|  rou1  |=====|  rou2  |=====|  rou3  |=====|  rou4  |=====|______|
    //     _||_        \______/       \______/       \______/       \______/       \______/        _||_
    //    |____|                                                                                  |____|
    //

    it("ttl = 4, not enouth", function() {
        cmp0.send_thing("some text", null, 0, cmp1.network_cards[0].ip_addr, 0x0800, 4)
        
        assert.isNull(cmp1.last_recv)
        assert.strictEqual(cmp0.last_recv, "Time exceeded.")
    })
    it("ttl = 6, strictly enouth", function() {
        cmp0.last_recv = null
        cmp1.last_recv = null

        cmp0.send_thing("some text", null, 0, cmp1.network_cards[0].ip_addr, 0x0800, 6)

        assert.strictEqual(cmp1.last_recv, "some text")
        assert.isNull(cmp0.last_recv)
    })
    it("ttl = 64, way to mutch", function() {
        cmp0.last_recv = null
        cmp1.last_recv = null

        cmp0.send_thing("some text", null, 0, cmp1.network_cards[0].ip_addr, 0x0800, 64)

        assert.strictEqual(cmp1.last_recv, "some text")
        assert.isNull(cmp0.last_recv)
    })
})