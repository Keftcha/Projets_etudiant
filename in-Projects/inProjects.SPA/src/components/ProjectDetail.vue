<template>
    <div>
        <div>
            <span class="text-project-name"> {{projectDetail.project.name}}  </span> 

            <br>

            <span class="text-project-name">
                Semestre : {{projectDetail.project.semester}}
            </span>

            <br>

            <span class="star" @click="FavOrUnfav()">
                <span class="fa fa-star"></span>
            </span>
        </div>

        <div v-if="CheckedAuthorize('Administration')" class="generate_sheet">
            <el-button
                v-loading="loading"
                element-loading-text="Generation..."
                element-loading-spinner="el-icon-loading"
                element-loading-background="white"

                type="primary"
                @click="CreateSheet(idProject)"
            >
                Fiche du projet
            </el-button>
        </div>

        <br>

        <hr>

        <br/>
            <img class="image-detail-project" :src="projectDetail.project.logo"/>
        <br/>
        <br/>
            <span class="text-project-slogan"> "{{projectDetail.project.slogan}}" </span>
        <br/>
        <br/>
           <span class="text-project-pitch"> {{projectDetail.project.pitch}} </span>
        <br/>
        <br/>
            <div class="title">Membres du projets : </div>
            <div v-for="(o, idx) in projectDetail.students" :key="idx">
                {{o.firstName}} {{o.lastName}}
            </div>
        <br/>
            <div class="title"> Technologies Utilisées : </div>
            <div v-for="(o, idx) in projectDetail.project.technologies" :key="idx">
                {{o}}
            </div>
            
        <br/>
        <div v-if="projectDetail.project.url != null" class="title">
        <img class="image-github" src="https://github.githubassets.com/images/modules/logos_page/Octocat.png">
         &nbsp;
        <a :href="projectDetail.project.url" target="_blank">GitHub</a>
        </div>

    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator"
import { ProjectDetails } from "../modules/classes/ProjectDetails"
import { getInfosProject, verifyProjectFav, favProject } from "../api/submitProjectApi"
import { getGroupUserAccessPanel } from "../api/groupApi"
import {GetProject} from "../api/getProject"
import {ProjectSheet, ProjectPiSheet, ProjectPfhSheet} from "../modules/classes/ProjectSheet"
import pdfMake from "pdfmake/build/pdfmake"
import pdfFonts from "pdfmake/build/vfs_fonts"
pdfMake.vfs = pdfFonts.pdfMake.vfs

@Component
export default class ProjectDetail extends Vue {
    private idProject!: number
    private projectDetail: ProjectDetails = new ProjectDetails()
    private isFav!: boolean
    private loading: boolean = false

    async mounted() {
    
        this.idProject = Number(this.$route.params.projectId)
        this.projectDetail = await getInfosProject(this.idProject)
        this.isFav = await verifyProjectFav(this.idProject)

        if (this.isFav) {
            const div = document.getElementsByClassName("fa-star")[0]
            div.setAttribute("style", "color: #F5CC27 !important;")
        }
    }

    async FavOrUnfav() {
        try {
            await favProject(this.idProject)
            this.isFav = !this.isFav
            const div = document.getElementsByClassName("fa-star")[0]
            if (this.isFav) {
                div.setAttribute("style", "color: #F5CC27 !important;")
            } else {
                div.setAttribute("style", "color: #000000 !important;")
            }
        } catch (e) {
            console.error(e)
        }
    }

    async CreateSheet(id: number) {
        this.loading = true

        // fetch to the server all information we need and formated
        let project = await GetProject(id)

        // generate the pdf
        var sheet = project.generate_sheet()
        pdfMake.createPdf(sheet).download()

        this.loading = false
    }

    CheckedAuthorize(needToBe: string){
        return this.$store.state.currentUserType.find(x => x == needToBe) != null ? true : false
    }

    async Back() {
        this.$router.go(-1)
    }
}

</script>

<style>
.image-detail-project{
    height: auto;
    border: 2px solid;
    width: 20vw;
}

.text-project-name{
    text-align: start;
    font-size: 200%;
    font-style: italic;
    margin-right: 80%;

}

.text-project-slogan{
    font-size: 150%;
	font-weight: bold;
    font-style: italic;
}

.text-project-pitch{
    text-align: center;
	width :40%;
	font-size: 150%;
	display: inline-block;
}

.title{
	font-weight: bold;
    font-size: 150%
}

.star{
	margin-right: 80%;
	cursor: pointer;
}
.star:hover {
	opacity: 0.8;
}

.star:active {
	transform: scale(0.93,0.93) translateY(2px)
}

.fa-star{
    font-size: 200%;
}

.image-github{
    height: auto;
    width: 2vw;
}

.generate_sheet{
    float: right;
    margin-top: -3vw;
    margin-right: 2vw;
}
</style>