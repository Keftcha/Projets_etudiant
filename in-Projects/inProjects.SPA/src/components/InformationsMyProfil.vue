<template>
    <div>        
    <Error :error="error"/>
        <div v-if="mode=='normal'" class="profileInfo">
            <table>
                <fieldset>
                    <tbody>
                        <tr>
                            <td><b>Nom: </b></td>
                            <td>{{this.infosUser.userData.firstName}}</td>
                        <tr>
                            <td><b>Prénom: </b></td>
                            <td>{{this.infosUser.userData.lastName}}</td>
                        <tr>
                            <td><b>Email: </b></td>
                            <td>{{this.infosUser.userData.email}}</td>
                        <tr>
                            <td><b>Email Secondaire: </b></td>
                            <td>{{this.infosUser.userData.emailSecondary}}<span v-if="this.infosUser.userData.emailSecondary == null"> Auncun email secondaire</span></td>
                            <tr>
                            <td><b>Groupe: </b></td>
                            <td>{{this.infosUser.group}} <span v-if="this.infosUser.isActual == false">(Anciennement)</span></td>
                        </tr>
                    </tbody>
                </fieldset>
            </table>
            <button class="button" @click="ChangeMode('edit')">Editer Profil</button>
        </div>
        <!-- Mode edit -->
        <div v-else>
            <form ref="infosUserTemp" label-suffix=" :" size="medium">
                <i v-show="errors.has('Nom')" class="fa fa-warning" style="color:orange;"></i>
                <span v-show="errors.has('Nom')" class="errorStyle">{{ errors.first('Nom') }}</span>
                <item v-if="CheckUserSchemes('Basic')">
                    <label>Nom:</label>
                    <input name="Nom" v-validate="'required|alpha'" v-model="infosUser.userData.firstName">
                </item>
                    <i v-show="errors.has('Prenom')" class="fa fa-warning" style="color:orange;"></i>
                    <span v-show="errors.has('Prenom')" class="errorStyle">{{ errors.first('Prenom') }}</span>
                <item v-if="CheckUserSchemes('Basic')">
                    <label>Prénom:</label>
                    <input name="Prenom" v-validate="'required|alpha'" v-model="infosUser.userData.lastName">
                </item>
                    <i v-show="errors.has('Email')" class="fa fa-warning" style="color:orange;"></i>
                    <span v-show="errors.has('Email')" class="errorStyle">{{ errors.first('Email') }}</span>
                    <item v-if="CheckUserSchemes('Basic')">
                        <label>Email :</label>
                        <input name="Email" v-validate="'required|email'" v-model="infosUser.userData.email">
                    </item>
                        <i v-show="errors.has('Email Secondaire')" class="fa fa-warning" style="color:orange;"></i>
                        <span v-show="errors.has('Email Secondaire')" class="errorStyle">{{ errors.first('Email Secondaire') }}</span>
                    <item>
                            <label>Email Secondaire :</label>
                            <input class="text-input-my-profil" v-validate="'email'" v-model="infosUser.userData.emailSecondary">
                    </item>  
                    <br><br><br>
                <button class="button" @click="ChangeInformation()">Valider</button>
                <button class="button" @click="Cancel('normal')">Annuler</button>
            </form>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator"
import { getAccountInfos, changeAccountInfos} from "../api/accountApi"
import { InfosAccount } from "@/modules/classes/InfosAccount"
import { getAuthService, AuthService } from "../modules/authService"
import Error from "./Erreur.vue"

@Component({
    components: {
        Error
    }
})
export default class InformationsMyProfil extends Vue {
    public  error: string[] = []
    private infosUser: InfosAccount = new InfosAccount()
    private infosUserTemp: InfosAccount = new InfosAccount()
    private mode: string = "normal"
    private authService: AuthService = getAuthService()

    async created() {
       this.infosUser = await getAccountInfos()
    }

    async ChangeInformation() {
         if (await this.$validator.validateAll()) {
             try {
                this.error = []

                await changeAccountInfos(this.infosUser)

                this.$message({
                    message: "Information changer",
                    type: "success"
                })

                this.ChangeMode("normal")
             } catch (e) {
                 this.error.push(e.response.data)
             }
         }
    }

    ChangeMode(mode: string) {
        this.error = []
        this.mode = mode
        console.log(this.infosUser.clone)
        if (this.mode == "edit") {this.infosUserTemp = this.infosUser.clone()}
    }

     Cancel(mode: string) {
        this.infosUser = this.infosUserTemp.clone()
        this.infosUser.userData.firstName = this.infosUserTemp.userData.firstName
        this.infosUser.userData.lastName = this.infosUserTemp.userData.lastName
        this.infosUser.userData.email = this.infosUserTemp.userData.email
        this.infosUser.userData.emailSecondary = this.infosUserTemp.userData.emailSecondary
        this.ChangeMode(mode)
    }

    CheckUserSchemes(schemes: string) {
        var email = this.infosUser.userData.email.substring(this.infosUser.userData.email.lastIndexOf("@") +1);

        if(email == 'intechinfo.fr') return false

        const exist = this.authService.authenticationInfo.user.schemes.find(x => x.name == schemes)
        if ( exist == undefined) return false
        return true
    }
}
</script>

<style>
.text-input-my-profil{
    width: 50%;
    padding: 0%;
}

.profileInfo{
    padding-left: 8%;
    display: inline-flex;
}
td{
    text-align: left
}


</style>