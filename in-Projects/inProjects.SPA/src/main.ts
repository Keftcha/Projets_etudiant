import Vue from "vue"
import VeeValidate, { Validator } from "vee-validate"
import fr from "vee-validate/dist/locale/fr"
import ElementUI from "element-ui"
import locale from "element-ui/lib/locale/lang/fr"
import "element-ui/lib/theme-chalk/index.css"
import App from "./App.vue"
import router from "./router"
import store from "./store"
import axios from "axios"
import { initializeAuthService } from "./modules/authService"
import { FontAwesomeIcon, FontAwesomeLayers, FontAwesomeLayersText } from "@fortawesome/vue-fontawesome"
import { library } from "@fortawesome/fontawesome-svg-core"
import {
    faHome, faUserGraduate, faUserTie,
    faFileAlt, faClipboard, faBars,
    faSignInAlt, faSignOutAlt, faBell,
    faUser, faCog, faSearch, faPlusSquare,
    faThumbsUp, faFilter
} from "@fortawesome/free-solid-svg-icons"

library.add(
    faHome, faUserGraduate, faUserTie,
    faFileAlt, faClipboard, faBars,
    faSignInAlt, faSignOutAlt, faBell,
    faUser, faCog, faSearch, faPlusSquare,
    faThumbsUp, faFilter
)


Vue.component("font-awesome-icon", FontAwesomeIcon)
Vue.component("font-awesome-layers", FontAwesomeLayers)
Vue.component("font-awesome-layers-text", FontAwesomeLayersText)


Vue.config.productionTip = false
const axiosConst = axios.create()

Vue.use(ElementUI, { locale })
Vue.use(VeeValidate)
Validator.localize("fr", fr)

initializeAuthService(axiosConst).then( () => {
    new Vue({
        router,
        store,
        render: (h) => h(App),
    }).$mount("#app")
})