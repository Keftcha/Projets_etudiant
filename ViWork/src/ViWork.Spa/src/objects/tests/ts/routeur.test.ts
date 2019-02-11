import "mocha"
import {assert} from "chai"

import {Roucom} from "../../js/roucom"
import {ipv4} from "../../js/ipv4"

// get_gateway
// get network idx on network
// get mac by ip

describe("Test the gateway on a roucom", function() {
    let roucom = new Roucom()
    // configure the route table
    roucom.route_table.set(new ipv4("0.0.0.0/0"), new ipv4("192.168.1.24/24"))
    roucom.route_table.set(new ipv4("10.0.4.65/16"), new ipv4("192.168.1.76/24"))
    roucom.route_table.set(new ipv4("77.98.90.3/8"), new ipv4("192.168.1.34/24"))

    it("a ip which is in the route table", function() {
        assert.deepEqual(roucom.get_gateway(new ipv4("10.0.4.65/16")), new ipv4("192.168.1.76/24"))
    })
    it("a ip which isn't int the route table, but we know the network to reach", function() {
        assert.deepEqual(roucom.get_gateway(new ipv4("77.90.54.1/8")), new ipv4("192.168.1.34/24"))
    })
    it("when we don't know we route to the default address", function() {
        assert.deepEqual(roucom.get_gateway(new ipv4("42.42.42.42/3")), new ipv4("192.168.1.24/24"))
    })
    it("we know the ip but the mask isn't the same", function() {
        assert.deepEqual(roucom.get_gateway(new ipv4("10.0.4.65/8")), new ipv4("192.168.1.24/24"))
    })
    it("we knox the mask but the ip isn't the same", function() {
        assert.deepEqual(roucom.get_gateway(new ipv4("9.37.5.1/8")), new ipv4("192.168.1.24/24"))
    })
})

describe("Test the get network card idx on network function on a roucom", function() {
    let roucom = new Roucom(6)

    // configure the network card of the roucom
    roucom.network_cards[0].ip_addr = new ipv4("192.168.1.32/24")
    roucom.network_cards[1].ip_addr = new ipv4("10.8.111.150/20")
    roucom.network_cards[2].ip_addr = new ipv4("192.168.2.167/16")
    roucom.network_cards[3].ip_addr = new ipv4("10.0.5.16/8")
    roucom.network_cards[4].ip_addr = new ipv4("172.168.80.6/24")
    roucom.network_cards[5].ip_addr = new ipv4("172.136.12.0/22")

    it("the identique ip", function() {
        assert.strictEqual(roucom.get_network_card_idx_on_network(new ipv4("192.168.1.32/24")), 0)
    })
    it("not the same ip but on the same network", function() {
        assert.strictEqual(roucom.get_network_card_idx_on_network(new ipv4("10.8.110.150/20")), 1)
        assert.strictEqual(roucom.get_network_card_idx_on_network(new ipv4("172.168.80.56/24")), 4)
        assert.strictEqual(roucom.get_network_card_idx_on_network(new ipv4("172.136.12.255/22")), 5)
    })
    it("the same ip but not the same network, the mask is higher", function() {
        assert.isNull(roucom.get_network_card_idx_on_network(new ipv4("192.168.2.167/18")))
    })
    it("the same ip but not the same network, the mask is lower", function() {
        assert.isNull(roucom.get_network_card_idx_on_network(new ipv4("10.0.5.16/3")))
    })
})

describe("Test the get mac in arp function on roucom", function() {
    let roucom = new Roucom()
    
    // put some things in the arp table of the roucom
    roucom.arp_table.set(new ipv4("192.168.1.32/24"), 1)
    roucom.arp_table.set(new ipv4("10.8.111.150/20"), 3)
    roucom.arp_table.set(new ipv4("192.168.2.167/16"), 5)

    it("the ip isn't in the cam table", function() {
        assert.strictEqual(roucom.get_mac_in_arp(new ipv4("26.18.174.242/24")), 0)
    })
    it("ip which are in cam table", function() {
        assert.strictEqual(roucom.get_mac_in_arp(new ipv4("192.168.1.32/24")), 1)
        assert.strictEqual(roucom.get_mac_in_arp(new ipv4("10.8.111.150/20")), 3)
        assert.strictEqual(roucom.get_mac_in_arp(new ipv4("192.168.2.167/16")), 5)
    })
})