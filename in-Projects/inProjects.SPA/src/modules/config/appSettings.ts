import merge from "lodash/merge"

const c = require("../../../appSettings.json")
const ce = require(`../../../appSettings.${process.env.NODE_ENV}.json`)

merge(c, ce)
Object.freeze(c)

export const appSettings = c