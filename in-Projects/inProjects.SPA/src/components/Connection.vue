<template>
    <div>
        <div v-if="action == true">
            <Login></Login>
            <button type="button" class="button" @click="changeAction()">Créer un compte</button>
        </div>
        <div v-else>
            <Register></Register>
            <button type="button" class="button" @click="changeAction()">Déja un compte ?</button>
        </div>
    </div>
</template>

<script lang="ts">

import { Vue, Component } from "vue-property-decorator"
import Login from "@/components/Login.vue"
import Register from "@/components/Register.vue"
import { AuthService } from "@signature/webfrontauth"

@Component({
    components: {
        Login,
        Register,
    },
})
export default class Connection extends Vue {
    action: boolean = true
    authService!: AuthService

    changeAction() {
        this.action = !this.action
    }
}

</script>

<style>

</style>
