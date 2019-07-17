<template>
<div>
    <el-submenu index="32">
        <template slot="title">
            <font-awesome-icon icon="user-tie" size="lg"/>
                <span v-if="isCollapse == false"> Listes</span>
        </template>
            <el-menu-item-group>
                <el-menu-item index="11-1" @click="redirect('/student')">Liste des étudiants</el-menu-item>
            </el-menu-item-group>
            <el-menu-item-group>
                <el-menu-item index="11-2" @click="redirect('/staffMember')">Liste des professeurs</el-menu-item>
            </el-menu-item-group>

    </el-submenu>
    <el-submenu index = "33">
                            <el-menu-item index="33-1" @click="redirect(`/submit_project`)">
             <font-awesome-icon icon="plus-square" size="lg" />
            <span> Deposer un Projet</span>
        </el-menu-item>
    </el-submenu>
</div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator"

@Component
export default class TeacherPanel extends Vue {
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
