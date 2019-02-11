import Vue from 'vue';
import AuthService from '../services/AuthService';
export default Vue.directive('required-providers', {
    bind(el, binding) {
        let providers = binding.value;
        if (!providers)
            throw new Error("v-required-providers Expected Array value.");
        el.style.visibility = AuthService.isBoundToProvider(providers) ? 'visible' : 'hidden';
    }
});
//# sourceMappingURL=requiredProviders.js.map