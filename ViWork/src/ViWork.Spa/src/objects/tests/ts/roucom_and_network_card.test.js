import "mocha";
import { assert } from "chai";
import { Roucom } from "../../js/roucom";
// The default constructor
describe("Create roucom", function () {
    it("Create roucom should create new network card which reference the roucom", function () {
        let cmp = new Roucom();
        assert.isNotNull(cmp.network_cards[0]);
        assert.strictEqual(cmp.network_cards[0].roucom, cmp);
    });
});
// Roucom with no net network card
describe("Create roucom no network card", function () {
    let cmp = new Roucom(0);
    it("The roucom haven't any network cards", function () {
        assert.isEmpty(cmp.network_cards);
    });
});
// Roucom constructor with multiple network card
describe("Create roucom with 6 network cards which reference the roucom", function () {
    let cmp = new Roucom(6);
    it("The roucom have 6 network cards", function () {
        assert.strictEqual(cmp.network_cards.length, 6);
    });
    it("The six network cards reference the roucom", function () {
        cmp.network_cards.forEach(card => {
            assert.strictEqual(card.roucom, cmp);
        });
    });
});
// add network card to roucom without one
describe("Create roucom without network card and add a network card", function () {
    let cmp = new Roucom(0);
    cmp.add_network_card();
    it("The roucom reference the network card", function () {
        assert.strictEqual(cmp.network_cards.length, 1);
        assert.typeOf(cmp.network_cards[0], "object");
    });
    it("The network card reference the roucom", function () {
        assert.strictEqual(cmp.network_cards[0].roucom, cmp);
    });
});
// Add multiple network card on a roucom
describe("Create roucom with 6 network cards which reference the roucom", function () {
    let cmp = new Roucom();
    for (let idx = 5; idx > 0; idx -= 1) {
        cmp.add_network_card();
    }
    it("The roucom have 6 network cards", function () {
        assert.strictEqual(cmp.network_cards.length, 6);
    });
    it("The six network cards reference the roucom", function () {
        cmp.network_cards.forEach(card => {
            assert.strictEqual(card.roucom, cmp);
        });
    });
});
// Negative number of network card
describe("Create a roucom with less than 0 network card should throw error", function () {
    let fn = function () { new Roucom(-2); };
    it("Negative throw error sa mère", function () {
        assert.throw(fn, Error);
    });
});
//# sourceMappingURL=roucom_and_network_card.test.js.map