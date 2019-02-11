import Vue from "vue";
import App from "./components/App.vue";
import AuthService from "./services/AuthService.js";
import router from "./main.router.js"
import store from "./store";
import "./main.vendors.js";
import "./main.auth.js";
import "./plugins/element.js";
import './plugins/element.js';
import ElementUI from "element-ui"

Vue.use(ElementUI);
Vue.config.productionTip = false;

const main = async () => {
  await AuthService.init();
  new Vue({
    router,
    store,
    render: (h) => h(App),
  }).$mount("#app");
};

main();