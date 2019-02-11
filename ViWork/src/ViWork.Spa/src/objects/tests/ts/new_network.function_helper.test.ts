import {Routeur} from "../../js/routeur"
import {Switch} from "../../js/commutateur"
import {Computer} from "../../js/computer"
import {ipv4} from "../../js/ipv4"
import {Cable} from "../../js/cable"

export let new_network = function() {
    let routeur = new Routeur(3)
    // configure network cards of the routeur
    routeur.network_cards[0].ip_addr = new ipv4("192.168.0.254/24")
    routeur.network_cards[1].ip_addr = new ipv4("192.168.1.254/24")
    routeur.network_cards[2].ip_addr = new ipv4("10.4.255.254/16")
    // configure the route table of the routeur
    routeur.route_table.set(new ipv4("192.168.0.0/24"), new ipv4("192.168.0.254/24"))
    routeur.route_table.set(new ipv4("192.168.1.0/24"), new ipv4("192.168.1.254/24"))
    routeur.route_table.set(new ipv4("10.4.0.0/16"), new ipv4("10.4.255.254/16"))

    // the first network 192.168.0.0/24
    let switch0 = new Switch(4)
    // connect the switch to the routeur
    let cab0 = new Cable(true, routeur.network_cards[0].port, switch0.ports[3])

    let cmp00 = new Computer()
    // configure the ip
    cmp00.network_cards[0].ip_addr = new ipv4("192.168.0.1/24")
    // configure the route table
    cmp00.route_table.set(new ipv4("192.168.0.0/24"), new ipv4("192.168.0.1/24"))
    cmp00.route_table.set(new ipv4("0.0.0.0/0"), new ipv4("192.168.0.254/24"))
    // connect the computer to the switch
    let cab00 = new Cable(false, cmp00.network_cards[0].port, switch0.ports[0])

    let cmp01 = new Computer()
    // configure the ip
    cmp01.network_cards[0].ip_addr = new ipv4("192.168.0.2/24")
    // configure the route table
    cmp01.route_table.set(new ipv4("192.168.0.0/24"), new ipv4("192.168.0.2/24"))
    cmp01.route_table.set(new ipv4("0.0.0.0/0"), new ipv4("192.168.0.254/24"))
    // connect the computer to the switch
    let cab01 = new Cable(false, cmp01.network_cards[0].port, switch0.ports[1])

    // no comments for computer configuration because you exactly know what i do otherwire look how i configure the previous computer
    let cmp02 = new Computer()
    cmp02.network_cards[0].ip_addr = new ipv4("192.168.0.3/24")
    cmp02.route_table.set(new ipv4("192.168.0.0/24"), new ipv4("192.168.0.3/24"))
    cmp02.route_table.set(new ipv4("0.0.0.0/0"), new ipv4("192.168.0.254/24"))
    let cab02 = new Cable(false, cmp02.network_cards[0].port, switch0.ports[2])


    // the second network 192.168.1.0/24
    // if you want to know explictly what do (with comments) see the creaton and configuration of the first network
    let switch1 = new Switch(4)
    let cab1 = new Cable(true, routeur.network_cards[1].port, switch1.ports[3])

    let cmp10 = new Computer()
    cmp10.network_cards[0].ip_addr = new ipv4("192.168.1.1/24")
    cmp10.route_table.set(new ipv4("192.168.1.0/24"), new ipv4("192.168.1.1/24"))
    cmp10.route_table.set(new ipv4("0.0.0.0/0"), new ipv4("192.168.1.254/24"))
    let cab10 = new Cable(false, cmp10.network_cards[0].port, switch1.ports[0])

    let cmp11 = new Computer()
    cmp11.network_cards[0].ip_addr = new ipv4("192.168.1.2/24")
    cmp11.route_table.set(new ipv4("192.168.1.0/24"), new ipv4("192.168.1.2/24"))
    cmp11.route_table.set(new ipv4("0.0.0.0/0"), new ipv4("192.168.1.254/24"))
    let cab11 = new Cable(false, cmp11.network_cards[0].port, switch1.ports[1])

    let cmp12 = new Computer()
    cmp12.network_cards[0].ip_addr = new ipv4("192.168.1.3/24")
    cmp12.route_table.set(new ipv4("192.168.1.0/24"), new ipv4("192.168.1.3/24"))
    cmp12.route_table.set(new ipv4("0.0.0.0/0"), new ipv4("192.168.1.254/24"))
    let cab12 = new Cable(false, cmp12.network_cards[0].port, switch1.ports[2])


    // the third network 10.4.0.0/16
    let switch2 = new Switch(4)
    let cab2 = new Cable(true, routeur.network_cards[2].port, switch2.ports[3])

    let cmp20 = new Computer()
    cmp20.network_cards[0].ip_addr = new ipv4("10.4.0.1/16")
    cmp20.route_table.set(new ipv4("10.4.0.0/16"), new ipv4("10.4.0.1/16"))
    cmp20.route_table.set(new ipv4("0.0.0.0/0"), new ipv4("10.4.255.254/16"))
    let cab20 = new Cable(false, cmp20.network_cards[0].port, switch2.ports[0])

    let cmp21 = new Computer()
    cmp21.network_cards[0].ip_addr = new ipv4("10.4.0.2/16")
    cmp21.route_table.set(new ipv4("10.4.0.0/16"), new ipv4("10.4.0.2/16"))
    cmp21.route_table.set(new ipv4("0.0.0.0/0"), new ipv4("10.4.255.254/16"))
    let cab21 = new Cable(false, cmp21.network_cards[0].port, switch2.ports[1])

    let cmp22 = new Computer()
    cmp22.network_cards[0].ip_addr = new ipv4("10.4.0.3/16")
    cmp22.route_table.set(new ipv4("10.4.0.0/16"), new ipv4("10.4.0.3/16"))
    cmp22.route_table.set(new ipv4("0.0.0.0/0"), new ipv4("10.4.255.254/16"))
    let cab22 = new Cable(false, cmp22.network_cards[0].port, switch2.ports[2])

    // return all objects
    // - the routeur
    // - all switchs
    // - all computers
    // - all cables

    return {
        "routeur": routeur,

        "sw0": switch0,
        "sw1": switch1,
        "sw2": switch2,

        "cmp00": cmp00,
        "cmp01": cmp01,
        "cmp02": cmp02,

        "cmp10": cmp10,
        "cmp11": cmp11,
        "cmp12": cmp12,

        "cmp20": cmp20,
        "cmp21": cmp21,
        "cmp22": cmp22,

        "cab0": cab0,
        "cab00": cab00,
        "cab01": cab01,
        "cab02": cab02,

        "cab1": cab1,
        "cab10": cab10,
        "cab11": cab11,
        "cab12": cab12,

        "cab2": cab2,
        "cab20": cab20,
        "cab21": cab21,
        "cab22": cab22
    }
}