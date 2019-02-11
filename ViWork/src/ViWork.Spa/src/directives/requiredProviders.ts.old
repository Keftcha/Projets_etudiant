import Vue from "vue";
import AuthService from "../services/AuthService";

export default Vue.directive("required-providers", {
    bind(el: any, binding: any) {
         const providers = binding.value;

         if (!providers) { throw new Error("v-required-providers Expected Array value."); }

         el.style.visibility = AuthService.isBoundToProvider(providers) ? "visible" : "hidden";
    },
});


