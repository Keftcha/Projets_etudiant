import "mocha"
import {assert} from "chai"

import {ipv4} from "../../js/ipv4"

describe("Create ip object", function() {
    let addr0 = new ipv4("10.0.1.34/24")
    let addr1 = new ipv4("192.168.15.91/24")
    let addr2 = new ipv4("77.179.54.2/20")
    let addr3 = new ipv4("145.65.95.5/30")
    let addr4 = new ipv4("100.0.52.12/10")
    let addr5 = new ipv4("255.255.255.255/1")
    let addr6 = new ipv4("0.0.0.0/32")
    let addr7 = new ipv4("34.98.0.21/0")
    
    it("the ip.toString", function() {
        assert.strictEqual(addr0.toString(), "10.0.1.34/24")
        assert.strictEqual(addr1.toString(), "192.168.15.91/24")
        assert.strictEqual(addr2.toString(), "77.179.54.2/20")
        assert.strictEqual(addr3.toString(), "145.65.95.5/30")
        assert.strictEqual(addr4.toString(), "100.0.52.12/10")
        assert.strictEqual(addr5.toString(), "255.255.255.255/1")
        assert.strictEqual(addr6.toString(), "0.0.0.0/32")
        assert.strictEqual(addr7.toString(), "34.98.0.21/0")
    })
    it("the network address", function() {
        assert.strictEqual(ipv4.toString(addr0.network), "10.0.1.0")
        assert.strictEqual(ipv4.toString(addr1.network), "192.168.15.0")
        assert.strictEqual(ipv4.toString(addr2.network), "77.179.48.0")
        assert.strictEqual(ipv4.toString(addr3.network), "145.65.95.4")
        assert.strictEqual(ipv4.toString(addr4.network), "100.0.0.0")
        assert.strictEqual(ipv4.toString(addr5.network), "128.0.0.0")
        assert.strictEqual(ipv4.toString(addr6.network), "0.0.0.0")
        assert.strictEqual(ipv4.toString(addr7.network), "0.0.0.0")
    })
    it("the mask", function() {
        assert.strictEqual(ipv4.toString(addr0.mask), "255.255.255.0")
        assert.strictEqual(ipv4.toString(addr1.mask), "255.255.255.0")
        assert.strictEqual(ipv4.toString(addr2.mask), "255.255.240.0")
        assert.strictEqual(ipv4.toString(addr3.mask), "255.255.255.252")
        assert.strictEqual(ipv4.toString(addr4.mask), "255.192.0.0")
        assert.strictEqual(ipv4.toString(addr5.mask), "128.0.0.0")
        assert.strictEqual(ipv4.toString(addr6.mask), "255.255.255.255")
        assert.strictEqual(ipv4.toString(addr7.mask), "0.0.0.0")
    })
    it("the wildcard", function() {
        assert.strictEqual(ipv4.toString(addr7.wildcard), "255.255.255.255")
        assert.strictEqual(ipv4.toString(addr6.wildcard), "0.0.0.0")
        assert.strictEqual(ipv4.toString(addr5.wildcard), "127.255.255.255")
        assert.strictEqual(ipv4.toString(addr4.wildcard), "0.63.255.255")
        assert.strictEqual(ipv4.toString(addr3.wildcard), "0.0.0.3")
        assert.strictEqual(ipv4.toString(addr2.wildcard), "0.0.15.255")
        assert.strictEqual(ipv4.toString(addr1.wildcard), "0.0.0.255")
        assert.strictEqual(ipv4.toString(addr0.wildcard), "0.0.0.255")
    })
    it("the broadcars address", function() {
        assert.strictEqual(ipv4.toString(addr0.broadcast), "10.0.1.255")
        assert.strictEqual(ipv4.toString(addr1.broadcast), "192.168.15.255")
        assert.strictEqual(ipv4.toString(addr2.broadcast), "77.179.63.255")
        assert.strictEqual(ipv4.toString(addr3.broadcast), "145.65.95.7")
        assert.strictEqual(ipv4.toString(addr4.broadcast), "100.63.255.255")
        assert.strictEqual(ipv4.toString(addr5.broadcast), "255.255.255.255")
        assert.strictEqual(ipv4.toString(addr6.broadcast), "0.0.0.0")
        assert.strictEqual(ipv4.toString(addr7.broadcast), "255.255.255.255")
    })
})

describe("Compare two ip", function() {
    let ip1 = new ipv4("10.10.10.5/4")
    let ip2 = new ipv4("10.10.10.5/4")
    let ip3 = new ipv4("192.168.1.70/24")

    it("ip are differents objects", function() {
        assert.notStrictEqual(ip1, ip2)
        assert.notStrictEqual(ip1, ip3)
    })
    it("ip are deeply equal", function() {
        assert.deepEqual(ip1, ip2)
        assert.notDeepEqual(ip1, ip3)
    })
    it("the compare class method work fine", function() {
        assert.isTrue(ipv4.compare(ip1, ip2))
        assert.isFalse(ipv4.compare(ip1, ip3))
    })
})

describe("is ip are on the same network", function() {
    // on the same network
    it("exactly the same ip", function() {
        assert.isTrue(ipv4.on_same_network(new ipv4("192.168.1.32/24"), new ipv4("192.168.1.32/24")))
    })
    it("not the same ip, bu on the same network", function() {
        assert.isTrue(ipv4.on_same_network(new ipv4("10.8.111.150/20"), new ipv4("10.8.110.150/20")))
        assert.isTrue(ipv4.on_same_network(new ipv4("172.168.80.6/24"), new ipv4("172.168.80.56/24")))
        assert.isTrue(ipv4.on_same_network(new ipv4("172.136.12.0/22"), new ipv4("172.136.12.255/22")))
    })

    // not on the same network
    it("not on the same network with a mask higher", function() {
        assert.isFalse(ipv4.on_same_network(new ipv4("192.168.2.167/16"), new ipv4("192.168.2.167/18")))
    })
    it("not on the same network with a mask lower", function() {
        assert.isFalse(ipv4.on_same_network(new ipv4("192.168.2.167/16"), new ipv4("192.168.2.167/18")))
    })
    it("not on the same network with a mask higher", function() {
        assert.isFalse(ipv4.on_same_network(new ipv4("10.0.5.16/8"), new ipv4("10.0.5.16/3")))
    })
})