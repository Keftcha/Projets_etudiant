<template>
    <div class="profil">
        <b class="first-element MyProfil">Mon Profil:</b>
        <br><br><br><br>   
            <InformationsMyProfil></InformationsMyProfil>
        <br/>
        <br/>
        <div v-if="CheckUserSchemes('Basic')">
        <br/>
            <PasswordChange></PasswordChange>
        <br><hr>
        </div>
        <div>
             <font size=5><b>Les Projets que vous aimez :</b></font>
             <br>
             <ProjectsUserFav></ProjectsUserFav>
        </div>
        <div v-if="this.whatTimed.find(x => x == 'Student')">
            <hr>
            <font size=5><b>Vos Projets :</b></font> 
            <ProjectStudentOwn></ProjectStudentOwn>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator"
import PasswordChange from "@/components/PasswordChange.vue"
import InformationsMyProfil from "@/components/InformationsMyProfil.vue"
import ProjectsUserFav from "@/components/ProjectsUserFav.vue"
import ProjectStudentOwn from "@/components/ProjectStudentOwn.vue"
import { getGroupUserAccessPanel } from "../api/groupApi"
import { getAuthService, AuthService } from "../modules/authService"
import { InfosAccount } from '../modules/classes/InfosAccount';
import { getAccountInfos } from '../api/accountApi';

@Component({
    components: {
        PasswordChange,
        InformationsMyProfil,
        ProjectsUserFav,
        ProjectStudentOwn
    },
})
export default class MyProfil extends Vue {
    private ZoneId: number = 4
    private whatTimed: string[] = []
    private infosUser: InfosAccount = new InfosAccount()
    private authService: AuthService = getAuthService()

    async created() {
        this.whatTimed = await getGroupUserAccessPanel(this.ZoneId)
        this.infosUser = await getAccountInfos()
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

.MyProfil{
    color: #111; 
    float: left;
    font-size:43px;
}
.profil{
        height: 100vh; 

}
</style>