<template>
<div class="forumplan">
    <div id="plan">
        <chacheli-designer 
            @chacheli-closed="closeChacheli"
            @chacheli-moved="checkClassroom"
            v-show="editMode" ref="designer" :layout="layout" :chachelis="chachelis" 
        />
        <div class="saveButton" v-if="hasChanged">
            <el-button id="saveButton" @click="SavePlan" type="success">Sauvegarder</el-button>
        </div>
        <div class="saveButton" v-else>
            <el-button id="saveButton" @click="SavePlan" type="success" disabled>Sauvegarder</el-button>
        </div>
    </div>

       <el-table id="projectTable" :data="projects" :row-class-name="tableRowClassName">
            <el-table-column prop="forumNumber" label="#" sortable width="180"/>
            <el-table-column prop="name" label="Nom du projet" width="180"/>
            <el-table-column prop="semester" label="Semestre" sortable width="180"/>
            <el-table-column prop="classRoom" label="Salle" sortable width="180"/>
         </el-table>
</div>
</template>

<script lang="js">
import Vue from "vue"
import { Component } from "vue-property-decorator"
import { ForumProject } from "../modules/classes/ForumProject"
import { getPlan, getProjects, savePlan as saveForumPlan } from "../api/forumApi"
import { Plan } from "../modules/classes/Plan"
import { Layout } from "../modules/classes/Layout"
import { Chacheli } from "../modules/classes/Chacheli"
import { AuthService } from "@signature/webfrontauth"
import { getAuthService } from "../modules/authService"

import ChacheliDesigner from "@shellybits/v-chacheli/dist/ChacheliDesigner"
import "@shellybits/v-chacheli/dist/ChacheliDesigner.css"

export default {
    name: "app",
    components: { ChacheliDesigner },

    data() {
        return {
            editMode: true,
            chachelis: new Array(),
            plan: new Plan(),
            layout: {
                cols: 0,
                rows: 0
            },
            basicHeight: 3,
            basicWidth: 4,
            projects: new Array(),
            authService: null,
            hasChanged: false
        }
    },

    async mounted() {
        this.authService = getAuthService()
        this.plan = await getPlan()
        this.projects = await getProjects(this.authService.authenticationInfo.user.userId)
        this.layout.cols = this.plan.width
        this.layout.rows = this.plan.height

        for (let i = 0; i < this.projects.length; i += 1) {
            const displayedName = this.projects[i].forumNumber + " - " + this.projects[i].name
            let isAvailable = false

            if (this.projects[i].posX == -1) {
                isAvailable = true
            }

            const c = new Chacheli(this.projects[i].forumNumber, this.projects[i].name, this.projects[i].posX, this.projects[i].posY, this.projects[i].width,
                this.projects[i].height, displayedName, isAvailable, "dummy-green", this.projects[i].projectId, this.projects[i].classRoom)
            this.chachelis.push(c)
        }
    },

    methods: {
        async SavePlan() {
            await saveForumPlan(this.projects)
            this.hasChanged = false
            this.$message({
                message: "Sauvegardé !.",
                type: "success"
            })
        },

        checkClassroom(chacheli) {
            this.chacheliMoved(chacheli)
            const middleX = chacheli.x + (chacheli.w / 2)
            const middleY = chacheli.y + (chacheli.h / 2)
            for (let classroom of this.plan.classRooms) {
                if (middleX >= classroom.originX && middleX <= classroom.endPositionX) {
                    if (middleY >= classroom.originY && middleY <= classroom.endPositionY) {
                        chacheli.classRoom = classroom.name
                        this.projects[chacheli.forumNumber - 1].classRoom = classroom.name
                        return
                    }
                }
            }
            this.projects[chacheli.forumNumber - 1].classRoom = ""
        },

        closeChacheli(chacheli) {
            const project = this.projects[chacheli.forumNumber - 1]
            project.posX = -1
            project.posY = -1
            project.width = this.basicWidth
            project.height = this.basicHeight
            project.classRoom = ""

            chacheli.w = this.basicWidth
            chacheli.h = this.basicHeight
            chacheli.classRoom = ""
            this.hasChanged = true
        },

        chacheliMoved(chacheli) {
            const project = this.projects[chacheli.forumNumber - 1]
            project.posX = chacheli.x
            project.posY = chacheli.y
            project.width = chacheli.w
            project.height = chacheli.h

            this.hasChanged = true
        },
        tableRowClassName({ row }) {
            return row.classRoom == "" ? "empty-row" : row.classRoom + "-row"
        }
    }
}
</script>

<style>
#plan {
	height: 90vh;
	display: flex;
    flex-direction: column;
    background-color: white;
}

#saveButton {
    margin-left: 88%;
    margin-bottom: 1%;
    position: sticky;
}

body {
	margin: 0;
	padding: 0;
}

* {
	box-sizing: border-box;
}

.chacheli-designer-layout {
    background-image: url("/plan_ecole.png");
    background-size: 100% 100%;
}

.chacheli-designer-layout .grid {
    color: transparent;
}

.chacheli-designer-layout .chacheli .content {
    color: #000000;
    font-weight: 700;
    font-size: 12px;
    border: none;
}
#projectTable {
    color: #000000;
    font-weight: 700;
    left: 20%;
}
.el-table .empty-row td:last-child {
    background: white;
}
.el-table .E01-row td:last-child {
    background: #867180;
}
.el-table .E02-row td:last-child {
    background: #fc8d84;
}
.el-table .E03-row td:last-child {
    background: #fbbd84;
}
.el-table .E05-row td:last-child {
    background: #ea2843;
}
.el-table .E06-row td:last-child {
    background: #4c83ac;
}
.el-table .E07-row td:last-child {
    background: #74bcf6;
}
.el-table .E08-row td:last-child {
    background: white;
}
.el-table .E09-row td:last-child {
    background: #a0a0a0;
}
.el-table .I16-row td:last-child {
    background: #3cb0c7;
}
.el-table .E0S-row td:last-child {
    background: #84bd5a;
}


</style>
