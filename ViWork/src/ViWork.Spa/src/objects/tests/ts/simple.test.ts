import "mocha"
import {assert} from "chai"

describe("Simple tests", function() {
    it("True must be True", function() {
        assert.strictEqual(true, true)
    })

    // Here i know !
    // all "it" functions are excuted after all expressions and statements
    // here there all tests pass
    let a = 3
    it("3 but 4", function() {
        assert.strictEqual(a, 4)
    })
    a += 1
    it("a real 4", function() {
        assert.strictEqual(a, 4)
    })
})

// if a value is modified in precedent it(), he stay modified in the next it()
// he don't reexecute "initial" statements before each it()
describe("Simple tests 2", function() {
    let foo = 3

    it("increment foo", function() {
        foo += 1
        assert.strictEqual(foo, 4)
    })
    it("what is the value of foo ? (4)", function() {
        assert.strictEqual(foo, 4)
    })
})