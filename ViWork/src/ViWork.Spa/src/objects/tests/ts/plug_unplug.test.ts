import "mocha"
import {assert} from "chai"

import {Port} from "../../js/port"
import {Cable} from "../../js/cable"

// Plug and unplug test

describe("Create cable and port", function() {
    let port = new Port()
    let cable = new Cable()
    
    // Nothing reference nothing
    it("The port have no reference to a cable.", function() {
        assert.strictEqual(port.cable, null)
    })
    it("The cable reference nothing.", function() {
        assert.deepEqual(cable.branched, [null, null])
    })
})


// plug and unplug work with only one port in cable
describe("Plug a cable in a port.", function() {
    let port = new Port()
    let cable = new Cable()

    // plug the cable in the port
    cable.plug(port)
    it("The cable reference the port", function() {
        assert.strictEqual(cable.branched[0], port)
    })
    it("The port reference the cable", function() {
        assert.strictEqual(port.cable, cable)
    })
})

describe("Unplug the cable in the port", function() {
    let port = new Port()
    let cable = new Cable()
    cable.plug(port)

    // unplug the cable in the port
    cable.unplug(port)
    it("The cable don't reference the port", function() {
        assert.deepEqual(cable.branched, [null, null])
    })
    it("the port don't reference the cable", function() {
        assert.strictEqual(port.cable, null)
    })
})

describe("Plug the port around the cable", function() {
    let port = new Port()
    let cable = new Cable()

    // plug the port around the cable
    port.plug(cable)
    it("The cable reference the port", function() {
        assert.strictEqual(cable.branched[0], port)
    })
    it("The port reference the cable", function() {
        assert.strictEqual(port.cable, cable)
    })
})

// unplug nothing or not pluged in
describe("Unplug the port around the cable", function() {
    let port = new Port()
    let cable = new Cable()
    port.plug(cable)
    
    // unplug the port around the cable
	port.unplug()
    it("The cable don't reference the port", function() {
        assert.deepEqual(cable.branched, [null, null])
    })
    it("the port don't reference the cable", function() {
        assert.strictEqual(port.cable, null)
    })
})

describe("Unplug not pluged in", function() {
    let port = new Port()
    let cable = new Cable()

    let fn = function() {cable.unplug(port)}

    it("Unplug nothing on the port must throw error", function() {
        assert.throw(port.unplug, Error)
    })
    it("Unplug the port which isn't around the cable", function () {
        assert.throw(fn, Error)
    })
})


// plug cable or port in port or cable which are aleready pluged in
describe("Plug cable in port where the port is have already this cable.", function() {
    let port = new Port()
    let cable = new Cable()

    cable.plug(port)
    let fn = function() {cable.plug(port)}

    it("Plug cable which already plugin must throw error", function() {
        assert.throw(fn, Error)
    })
})

describe("Plug port around the cable where the cable have already this cable.", function() {
    let port = new Port()
    let cable = new Cable()

    port.plug(cable)
    let fn = function() {port.plug(cable)}

    it("Plug port which already around the cable muth throw error.", function() {
        assert.throw(fn, Error)
    })
})


// plug two port around one cable and unplug them
describe("Plug two port in cable", function() {
    let p1 = new Port()
    let p2 = new Port()
    let cable = new Cable()

    cable.plug(p1)
    cable.plug(p2)

    it("The cable must reference the two ports", function() {
        assert.deepEqual(cable.branched, [p1, p2] || [p2, p1])
    })
    it("The two cable reference the same cable", function() {
        assert.strictEqual(p1.cable, p2.cable)
    })        
})

describe("Unplug one cable", function() {
    let p1 = new Port()
    let p2 = new Port()
    let cable = new Cable()

    cable.plug(p1)
    cable.plug(p2)

    p1.unplug()
    it("The cable must reference just one ports", function() {
        assert.deepEqual(cable.branched, [null, p2])
    })
    it("The cable reference null", function() {
        assert.strictEqual(p1.cable, null)
    })
    it("The cable reference the cable", function() {
        assert.strictEqual(p2.cable, cable)
    })
})

describe("Unplug the other cable", function() {
    let p1 = new Port()
    let p2 = new Port()
    let cable = new Cable()

    cable.plug(p1)
    cable.plug(p2)

    p2.unplug()
    it("The cable must reference just one ports", function() {
        assert.deepEqual(cable.branched, [p1, null])
    })
    it("The cable reference null", function() {
        assert.strictEqual(p2.cable, null)
    })
    it("The cable reference the cable", function() {
        assert.strictEqual(p1.cable, cable)
    })
})


// Plug cable in port while the cable is branched to two other ports
// Plug port in cable while the port is already pluged in other cable
describe("Plug cable in port while the cable is full", function() {
    let p1 = new Port()
    let p2 = new Port()
    let p3 = new Port()
    let cable = new Cable()

    cable.plug(p1)
    cable.plug(p2)

    let fn = function() {cable.plug(p3)}

    it("Plug the cable in third port must throw error", function() {
        assert.throw(fn, Error)
    })
})

describe("Plug a second cable in a port", function() {
    let port = new Port()
    let c1 = new Cable()
    let c2 = new Cable()

    c1.plug(port)
    
    let fn = function() {c2.plug(port)}

    it("Plug another cable in a port which have already a cable must throw an Error", function() {
        assert.throw(fn, Error)
    })
})

describe("Create cable with port in constructor", function () {
    it("Cable with two port", function() {
        let p1 = new Port()
        let p2 = new Port()
        let cable = new Cable(false, p1, p2)

        assert.deepEqual(cable.branched, [p1, p2])
    })
    it("Cable with the first port", function() {
        let port = new Port()
        let cable = new Cable(false, port)

        assert.deepEqual(cable.branched, [port, null])
    })
    it("Cable with the second port", function() {
        let port = new Port()
        let cable = new Cable(false, null, port)

        assert.deepEqual(cable.branched, [port, null])
    })
})