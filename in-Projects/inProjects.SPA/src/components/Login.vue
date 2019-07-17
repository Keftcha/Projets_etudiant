<template>
    <div>
        <div class="loading" v-show="isLoading">
            <div class="sk-cube-grid2">
                <div class="sk-cube sk-cube1"></div>
                <div class="sk-cube sk-cube2"></div>
                <div class="sk-cube sk-cube3"></div>
                <div class="sk-cube sk-cube4"></div>
                <div class="sk-cube sk-cube5"></div>
                <div class="sk-cube sk-cube6"></div>
                <div class="sk-cube sk-cube7"></div>
                <div class="sk-cube sk-cube8"></div>
                <div class="sk-cube sk-cube9"></div>
            </div>
        </div>
        <br>
        <el-form ref="user" :model="user" :label-position="labelPosition" label-width="100px" >
        <form v-on:submit.prevent.stop="Login" >
            <center>
                <b>
                    <u style="color: red;">{{error}}</u>
                    <el-form-item label="Identifiant" style="width:60%">
                        <el-input name="email" v-validate="'required|email'" placeholder="Insérez votre identifiant" v-model="user.email"></el-input>
                        <i v-show="errors.has('email')" class="fa fa-warning" style="color:orange;"></i>
                        <span v-show="errors.has('email')" class="errorStyle">{{ errors.first('email') }}</span>
                    </el-form-item>
                    <el-form-item label="Mot de passe" style="width:60%" >
                        <el-input name="password" v-validate="'required'" placeholder="Insérez votre mot de passe"
                            v-model="user.password" show-password ></el-input>
                        <i v-show="errors.has('password')" class="fa fa-warning" style="color:orange;"></i>
                        <span v-show="errors.has('password')" class="errorStyle">{{ errors.first('password') }}</span>
                    </el-form-item>
                    <el-form-item>
                        <button class="button" type="button" @click="Login">Valider</button>&nbsp;
                        <button class="button" type="button" @click="resetForm()">Réinitialiser</button>
                    </el-form-item>
                </b>
            </center>
            <input type="submit" hidden/>
        </form>
        </el-form>
        <div>
            <button @click="Outlook()" class="button" type="button">Connexion avec Outlook</button>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from "vue"
import { User } from "../modules/classes/User"
import { Component } from "vue-property-decorator"
import { getUserName } from "../api/accountApi"
import { AuthService } from "@signature/webfrontauth"
import { UserInfo } from "../modules/classes/UserInfo"
import { getAuthService } from "../modules/authService"
import { sha256 } from "js-sha256"

@Component

export default class Login extends Vue {
    private labelPosition: string = "top"
    private user: User = new User()
    private authService: AuthService = getAuthService()
    private error: string = ""
    private isLoading: boolean = false

    async Login() {
        this.isLoading = true

        if (await this.$validator.validateAll()) {
            try {
                const userInfos: UserInfo = await getUserName(this.user)
                const password: string = sha256(this.user.password)
                await this.authService.basicLogin(userInfos.userName, password)
            } catch (e) {
                console.debug(e)
            }
            if (this.authService.authenticationInfo.level == 0) {
                this.error = "La connexion a échouée ! Réessayez !"
            } else {
                this.error = ""
                this.$router.replace("/")
            }
        }
        this.isLoading = false
    }

    async Outlook() {
        await this.authService.startPopupLogin("Oidc")
        this.$router.replace("/")
    }

    resetForm() {
        this.user.email = ""
        this.user.password = ""
    }
}
</script>

<style>
input {
    text-align: center
}
.errorStyle {
    color: red
}


.loading {
    background-color: rgba( 255, 255, 255, 0.7);
    height: 600px;
    width: 481px;
    position: absolute;
    margin-left: 29vw;
    margin-top: 3.5vh;
    z-index: 10;
}
.sk-cube-grid2 {
  width: 40px;
  height: 40px;
  margin: 22.5vh auto;
  background-color: white;
}
.sk-cube-grid2 .sk-cube {
  width: 33%;
  height: 33%;
  background-color:#0099ff;
  float: left;
  -webkit-animation: sk-cubeGridScaleDelay 1.3s infinite ease-in-out;
          animation: sk-cubeGridScaleDelay 1.3s infinite ease-in-out; 
}
.sk-cube-grid2 .sk-cube1 {
  -webkit-animation-delay: 0.2s;
          animation-delay: 0.2s; }
.sk-cube-grid2 .sk-cube2 {
  -webkit-animation-delay: 0.3s;
          animation-delay: 0.3s; }
.sk-cube-grid2 .sk-cube3 {
  -webkit-animation-delay: 0.4s;
          animation-delay: 0.4s; }
.sk-cube-grid2 .sk-cube4 {
  -webkit-animation-delay: 0.1s;
          animation-delay: 0.1s; }
.sk-cube-grid2 .sk-cube5 {
  -webkit-animation-delay: 0.2s;
          animation-delay: 0.2s; }
.sk-cube-grid2 .sk-cube6 {
  -webkit-animation-delay: 0.3s;
          animation-delay: 0.3s; }
.sk-cube-grid2 .sk-cube7 {
  -webkit-animation-delay: 0s;
          animation-delay: 0s; }
.sk-cube-grid2 .sk-cube8 {
  -webkit-animation-delay: 0.1s;
          animation-delay: 0.1s; }
.sk-cube-grid2 .sk-cube9 {
  -webkit-animation-delay: 0.2s;
          animation-delay: 0.2s; }

@-webkit-keyframes sk-cubeGridScaleDelay {
  0%, 70%, 100% {
    -webkit-transform: scale3D(1, 1, 1);
            transform: scale3D(1, 1, 1);
  } 35% {
    -webkit-transform: scale3D(0, 0, 1);
            transform: scale3D(0, 0, 1); 
  }
}

@keyframes sk-cubeGridScaleDelay {
  0%, 70%, 100% {
    -webkit-transform: scale3D(1, 1, 1);
            transform: scale3D(1, 1, 1);
  } 35% {
    -webkit-transform: scale3D(0, 0, 1);
            transform: scale3D(0, 0, 1);
  } 
}
</style>
