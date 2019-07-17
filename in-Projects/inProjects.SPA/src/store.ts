import Vue from "vue"
import Vuex from "vuex"
import * as SignalR from "@aspnet/signalr"
Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    connectionStaffMember: SignalR.HubConnection,
    currentUserType: Array<string>()
  },
  mutations: {

  },
  actions: {

  },
})
