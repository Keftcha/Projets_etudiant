<template>
    <div>
        <div class="okok">
            <li class="dropdown">
                <a class="dropbtn" @click="redirect('/projectList')">Liste des projets</a>
                <a class="dropbtn" @click="redirect(`/ProjectUserVote`)">Voter pour les projets</a>  
                <a class="dropbtn" @click="redirect(`/PublicResult`)">État des votes</a>  
            </li>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator"
import { FontAwesomeIcon, FontAwesomeLayers, FontAwesomeLayersText } from "@fortawesome/vue-fontawesome"

@Component
export default class UserPanel extends Vue {
@Prop()
isCollapse!: boolean

 redirect(destination: string) {
        this.$router.push(destination)
    }
}
</script>

<style>
.el-menu-vertical:not(.el-menu--collapse) {
    width: 200px;
    min-height: 400px;
}
.el-menu-vertical{
    width: 80px;
    min-height: 400px;
    z-index: 10
}
</style>
