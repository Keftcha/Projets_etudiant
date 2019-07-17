<template>
    <form ref="user" :label-position="labelPosition" :model="user" label-width="100px">
    <form v-on:submit.prevent.stop="Register">
        <center>
            <b>
                <item label="Nom" style="width:60%;" prop="lastName">
                    <input name="lastName" placeholder="Insérez votre nom" v-model="user.lastName" v-validate="'required|alpha'">
                    <i v-show="errors.has('lastName')" class="fa fa-warning" style="color:orange;"></i>
                    <span v-show="errors.has('lastName')" class="errorStyle">{{ errors.first('lastName') }}</span>
                </item>
                <br>
                <item label="Prénom" style="width:60%" prop="firstName">
                    <br>
                    <input name="firstName" placeholder="Insérez votre prénom" v-model="user.firstName" v-validate="'required|alpha'">                    
                    <i v-show="errors.has('firstName')" class="fa fa-warning" style="color:orange;"></i>
                    <span v-show="errors.has('firstName')" class="errorStyle">{{ errors.first('firstName') }}</span>
                </item>
                <br>
                <item label="Email" style="width:60%" prop="email">
                    <br>
                    <input name="email" placeholder="Insérez votre email" v-model="user.email" v-validate="'required|email'">
                    <i v-show="errors.has('email')" class="fa fa-warning" style="color:orange;"></i>
                    <span v-show="errors.has('email')" class="errorStyle">{{ errors.first('email') }}</span>
                </item>
                <br>
                <item label="Mot de passe" style="width:60%" prop="password">
                    <br>
                    <input name="password" ref="password" v-validate="'required|min:6'" placeholder="Insérez votre mot de passe" v-model="user.password" show-password>
                    <i v-show="errors.has('password')" class="fa fa-warning" style="color:orange;"></i>
                    <span v-show="errors.has('password')" class="errorStyle">{{ errors.first('password') }}</span>
                </item>
                <br>
                <item label="Confirmer le mot de passe" style="width:60%" prop="verifiedPassword">
                    <br>
                    <input name="verifiedPassword" v-validate="'required|min:6|confirmed:password'" placeholder="Insérez votre mot de passe" v-model="verifiedPassword" show-password>
                    <i v-show="errors.has('verifiedPassword')" class="fa fa-warning" style="color:orange;"></i>
                    <span v-show="errors.has('verifiedPassword')" class="errorStyle">{{ errors.first('verifiedPassword') }}</span>
                </item>
                <br>
                <item>
                    <br>
                    <button type="button" class="button" @click="Register()">Valider</button>&nbsp;
                    <button type="button" class="button" @click="resetForm()">Réinitialiser</button>
                </item>
            </b>
        </center>
        <input type="submit" hidden/>
    </form>
    </form>
</template>

<script lang="ts">

import { Component, Vue } from "vue-property-decorator"
import { User } from "../modules/classes/User"
import { register as registerRequest } from "../api/accountApi"
import { sha256 } from "js-sha256"
import { AuthService } from "@signature/webfrontauth"
import { getAuthService } from "../modules/authService"
import { Form as ElForm } from "element-ui"

@Component
export default class Register extends Vue {

    private labelPosition: string = "top"
    private user: User = new User()
    private loginResult!: string
    private authService: AuthService = getAuthService()
    private verifiedPassword: string = ""

    async Register() {
        const userHashed: User = new User()
        userHashed.firstName = this.user.firstName
        userHashed.lastName = this.user.lastName
        userHashed.email = this.user.email

        if (await this.$validator.validateAll()) {
            userHashed.password = sha256(this.user.password)
            this.loginResult = await registerRequest(userHashed)
            this.Login(this.loginResult, userHashed.password)
        }
    }

    resetForm() {
        const ref = this.$refs.user as ElForm
        this.verifiedPassword = ""
        ref.resetFields()
    }

    async Login(userId: string, pw: string) {
        await this.authService.basicLogin(userId, pw)
        this.$router.replace("/")
    }
}

</script>

<style>
input{
      border: none;
    font-size: 16px;
    height: auto;
    margin: 0;
    outline: 0;
    padding: 15px !important;
    width: 100%;
    -webkit-box-shadow: 0 1px 0 rgba(0, 0, 0, 0.03) inset;
    box-shadow: 0 1px 0 rgba(0, 0, 0, 0.03) inset;
    margin-bottom: 30px;

}

item{
    margin-bottom: 10%
}
.errorStyle {
    color: red
}

</style>
