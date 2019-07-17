<template>
<div>
    <div style="width: 100%; margin-left: -10%;">
        <font-awesome-icon icon="filter" size="lg" style="position: absolute; top: 1%; left: 49%;" /><b style="position: absolute; top: 1%; left: 51%;">Trier :</b>
        <div class="selects">
            <span class="spans" @click="toggleSelect('School')">Choisir une école</span>
            <ul class="listFilter" id="schoolChoices" v-show="showSchool">
                <li v-for="item in schoolOptions" v-bind:key="item.schoolId">
                    <label><input type="checkbox" @click="selectSchool(item)" checked />{{item.name}}</label>
                </li>
            </ul>
            <span class="spans" @click="toggleSelect('Type')">Choisir un type de projet</span>
            <ul class="listFilter" id="typeChoices" v-show="showType">
                <li>
                    <label><input type="checkbox" @click="selectType('I')" name="pi" checked />Projets informatiques</label>
                </li>
                <li>
                    <label><input type="checkbox" @click="selectType('H')" name="pfh" checked />Projets de Formation Humaine</label>                    
                </li>
            </ul>
            <span class="spans" @click="toggleSelect('Semesters')">Choisir un semestre</span>
            <ul class="listFilter" id="semesterChoices" v-show="showSemesters">
                <li v-for="item in semesters" v-bind:key="item">
                    <label><input type="checkbox" @click="selectSemester(item)" checked />{{item}}</label>
                </li>
            </ul>
        <datalist id="languages" >
            <option v-for="(o, idx) in projectListToDisplay" :key="idx">{{o.groupName}}  ({{formatDateMonth(o.begDate)}}) </option>
        </datalist>
        <span>Chercher un projet:</span> <input type="text" style="width: 20%; border: solid black;" list="languages" v-model="groupName" @change="getProject(groupName)">
        </div>
    </div>
    <div v-if="CheckedAuthorize('Administration')">
        <el-button
            v-loading="loading"
            element-loading-text="Generation..."
            element-loading-spinner="el-icon-loading"
            element-loading-background="white"

            type="primary"
            @click="GetAllProjectSheet(schoolChoice,typeChoice,semesterChoice)"
        >
            Télécharger
        </el-button>
    </div>
    <br><br>
    <div class="sk-cube-grid" v-if="isLoading">
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
     <div class="masonry-layout">
        <div class="masonry-layout-panel masonry-layout-flip--md masonry-layout-flip" v-for="(o, index) in projectListToDisplay.length" :key="o">
            <div class="masonry-layout-panel__content masonry-layout-flip__content">
                <div class="masonry-layout-flip__panel masonry-layout-flip__panel--front">
                    <h2>{{projectListToDisplay[index].groupName}}</h2>
                    {{projectListToDisplay[index].semester}} {{projectListToDisplay[index].sector}}
                    <br>
                    {{formatDate(projectListToDisplay[index].begDate)}} / {{formatDate(projectListToDisplay[index].endDate)}}
                    <img :src="projectListToDisplay[index].logo" class="image">
                </div>
                <br>
                <div class="masonry-layout-flip__panel masonry-layout-flip__panel--back">
                    <br>
                    <u><b style="cursor:pointer;" @click="redirect(projectListToDisplay[index].projectStudentId)">{{projectListToDisplay[index].slogan}}</b></u>
                    <br><br>
                    {{projectListToDisplay[index].pitch}}
                    <br><br>
                    <el-button class="test" icon="el-icon-star-off" circle  @click="FavOrUnfav(projectListToDisplay[index], index)" v-bind:style="{background:isStarColored(projectListToDisplay[index].isFav)}"></el-button>
                </div>
            </div>
        </div>
    </div>
    </div>
</template>

<script lang="ts">
import Vue from "vue"
import { Component } from "vue-property-decorator"
import { GetAllProject } from "../api/projectApi"
import {Project} from "../modules/classes/Project"
import {verifyProjectFav, favProject } from "../api/submitProjectApi"
import {ProjectSheet, ProjectPiSheet, ProjectPfhSheet} from "../modules/classes/ProjectSheet"
import {GetAllSheet} from "../api/projectApi"
import pdfMake from "pdfmake/build/pdfmake"
import pdfFonts from "pdfmake/build/vfs_fonts"
pdfMake.vfs = pdfFonts.pdfMake.vfs
import { saveAs } from "file-saver"
import {make_archive} from "../modules/functions/make_archive"
import {School} from "../modules/classes/School"
import {getSchools} from "../api/schoolApi"
import JSZip from "jszip"

@Component
export default class ProjectList extends Vue {
    private projectList: Project[] = []
    private projects : ProjectSheet[] = []
    private projectListToDisplay: Project[] = []
    private border !: string
    private isFav!: boolean    
    private schoolOptions: School[] = []
    private starColor!: string
    private semesters!: string[]
    private semestersPfh!: string[]
    private schoolChoice: School[] = []
    private typeChoice: string[] = []
    private semesterChoice: string[] = []
    private zip : JSZip = new JSZip()
    private loading: boolean = false
    private isLoading: boolean = false
    private showSemesters: boolean = false
    private showType: boolean = false
    private showSchool: boolean = false
    private groupName!: string
    
    async mounted() {
        this.isLoading = true
        this.projectList  = await GetAllProject()
        for (const project of this.projectList) {
            this.projectListToDisplay.push(project)
        }
        this.isLoading = false
        this.schoolOptions = await getSchools()
        this.schoolOptions.splice(0, 1)
        this.semesters = ["Semestre 1", "Semestre 2", "Semestre 3", "Semestre 4", "Semestre 5"]
        this.schoolChoice = this.schoolOptions.slice(0)
        this.semesterChoice = this.semesters.slice(0)
        this.typeChoice = ["I", "H"]
    }
    getLeader(specificProject: Project) {
        for (let i = 0; i < specificProject.userId.length; i += 1) {
            if (specificProject.userId[i] == specificProject.leaderId) {
                return specificProject.firstName[i] + " " + specificProject.lastName[i]
            }
        }
    }
    formatDate(date: Date) {
        const newDate = date.toString()
        return newDate.substr(0, 10)
    }
    formatDateMonth(date: Date){
        const newDate = date.toString()
        return newDate.substr(0, 7)
    }
    getType(specificProject: Project) {
        if ( specificProject.type == "I" ) return this.border = "red 1px solid"
        if ( specificProject.type == "H" ) return this.border = "blue 1px solid"
        return this.border = "white 1px solid"
    }
    async FavOrUnfav(specificProject: Project, index: number) {
        try {
            const div  = document.getElementsByClassName("test")[index]
            this.isFav = await verifyProjectFav(specificProject.projectStudentId)
            await favProject(specificProject.projectStudentId)
            this.isFav = !this.isFav
            if (this.isFav) {
                div.setAttribute("style", "background: #F5CC27 !important;")
            } else {
                div.setAttribute("style", "background: #fffcfc !important;")
            }
        } catch (e) {
            console.error(e)
        }
    }
    isStarColored(i: number) {
        if (i != 0) {
            return this.starColor = "#F5CC27 !important"
        }
        return this.starColor = "#000000 !important;"
    }

    async CreatePdfAndSetUpToZip(project :Array<ProjectSheet> | Array<ProjectPiSheet> | Array<ProjectPfhSheet>,index : number) {
        const sheet =  pdfMake.createPdf(project[index].generate_sheet())
        sheet.getBlob(async (blob :Blob) => {
            if(project[index].type =="I")this.zip.file("fiches/ProjetInformatique/"+project[index].name + ".pdf",blob)
            else this.zip.file("fiches/ProjetFormationHumaine/"+project[index].name + ".pdf",blob)

            if(project.length -1 != index){
                await this.CreatePdfAndSetUpToZip(project,++index)
            }else{
                this.zip.generateAsync({type:"blob"})
                .then(function(content) {
                    saveAs(content, "fiches.zip")
                })
            }
        })
    }

    CheckedAuthorize(needToBe: string){
        return this.$store.state.currentUserType.find(x => x == needToBe) != null ? true : false
    }

    redirect(idProject: number) {
        this.$router.replace("/Project/" + idProject)
    }

    async GetAllProjectSheet(school: School[], type: string[], semester: string[]) {
        this.loading = true
        let semesterToSend: number = 0
        let schoolToSend = this.schoolOptions.find(x => x.name == school[0].name)
        if (schoolToSend == undefined) schoolToSend = new School(0,"Unknown")
        if(this.semesterChoice.length == 1) {
            let sem = semester[0].split(" ")
            semesterToSend = parseInt(sem[1])                            
        }
        else if(this.semesterChoice.length > 1) {
            semesterToSend = 0
        }

        // get projects form the back
        let projects: Array<ProjectSheet> | Array<ProjectPiSheet> | Array<ProjectPfhSheet> = await GetAllSheet(4, "I", 0)

        let index = 0
        this.zip = new JSZip()
        await this.CreatePdfAndSetUpToZip(projects,index)

        this.loading = false
    }
    selectSchool(item: School) {
        let idx = this.schoolChoice.indexOf(item)

        if(idx == -1) this.schoolChoice.push(item)
        else this.schoolChoice.splice(idx, 1)

        this.sortProjects()
    }
    selectSemester(item: string) {
        let idx = this.semesterChoice.indexOf(item)

        if(idx == -1) this.semesterChoice.push(item)
        else this.semesterChoice.splice(idx, 1)
        
        this.sortProjects()
    }
    selectType(item: string) {
        let idx = this.typeChoice.indexOf(item)

        if(idx == -1) this.typeChoice.push(item)
        else this.typeChoice.splice(idx, 1)
        
        this.sortProjects()
    }
    sortProjects() {
        this.projectListToDisplay = []
        let result = this.projectList.filter(project => 
            {
                for(const school of this.schoolChoice) {
                    if(project.schoolId == school.schoolId) {
                        for(const type of this.typeChoice) {
                            if(project.type == type) {
                                for(const semester of this.semesterChoice) {
                                    let sem = semester.split(" ")
                                    if(project.semester == "S0" + sem[1]) {
                                        return true
                                    }
                                }
                            }
                        }
                    }
                }
                return false
            })
        this.projectListToDisplay = result
    }
    getProject(groupName: string){
        console.log(groupName)
            for(const project of this.projectListToDisplay) {
                console.log(project.groupName.startsWith(groupName))
                if(groupName.startsWith(project.groupName)) {
                    console.log("ok")
                    this.projectListToDisplay = []
                    this.projectListToDisplay.push(project)
            }
        }
    }
    toggleSelect(obj: string) {
        switch(obj) {
            case "School":
                this.showSchool = !this.showSchool
                this.showType = false
                this.showSemesters = false
                break
            case "Type":
                this.showType = !this.showType
                this.showSchool = false
                this.showSemesters = false
                break
            case "Semesters":
                this.showSemesters = !this.showSemesters
                this.showType = false
                this.showSchool = false
                break
            default:
                this.showSemesters = false
                this.showType = false
                this.showSchool = false
        }
    }
}
</script>

<style <style lang="scss" scoped>

.image{
      width: 250px; 
      height: 250px;
}
.projectList{
    position: relative;
}
.masonry-layout {
    column-count: 3;
    column-gap: 0;
}
.masonry-layout-panel {
    break-inside: avoid;
    padding: 5px;
}
.masonry-layout-panel__content {
    padding: 10px;
    border-radius: 0px;
    border-style: solid;
    overflow: hidden;
    border-color: black;
    width: 70%;
}
.masonry-layout-flip {
    perspective: 1000;
}
.masonry-layout-flip:hover .masonry-layout-flip__content {
  transform: rotateY(180deg);
}
.masonry-layout-flip__panel--front {
  transform: rotateY(0deg);
  z-index: 2;
}
.masonry-layout-flip__panel--back {
  transform: rotateY(180deg);
  background: linear-gradient(rgb(13, 102, 219), rgb(68, 218, 229))
}
.masonry-layout-flip__panel {
  backface-visibility: hidden;
  border-radius: 0px;
  height: 100%;
  left: 0;
  overflow: auto;
  position: absolute;
  top: 0;
  width: 100%;
}
.masonry-layout-flip__content {
  overflow: visible;
  position: relative;
  transform-style: preserve-3d;
  transition: 0.25s;
  height: 400px; 
}

.sk-cube-grid {
  width: 40px;
  height: 40px;
  margin: 100px auto;
}

.sk-cube-grid .sk-cube {
  width: 33%;
  height: 33%;
  background-color:#0099ff;
  float: left;
  -webkit-animation: sk-cubeGridScaleDelay 1.3s infinite ease-in-out;
          animation: sk-cubeGridScaleDelay 1.3s infinite ease-in-out; 
}
.sk-cube-grid .sk-cube1 {
  -webkit-animation-delay: 0.2s;
          animation-delay: 0.2s; }
.sk-cube-grid .sk-cube2 {
  -webkit-animation-delay: 0.3s;
          animation-delay: 0.3s; }
.sk-cube-grid .sk-cube3 {
  -webkit-animation-delay: 0.4s;
          animation-delay: 0.4s; }
.sk-cube-grid .sk-cube4 {
  -webkit-animation-delay: 0.1s;
          animation-delay: 0.1s; }
.sk-cube-grid .sk-cube5 {
  -webkit-animation-delay: 0.2s;
          animation-delay: 0.2s; }
.sk-cube-grid .sk-cube6 {
  -webkit-animation-delay: 0.3s;
          animation-delay: 0.3s; }
.sk-cube-grid .sk-cube7 {
  -webkit-animation-delay: 0s;
          animation-delay: 0s; }
.sk-cube-grid .sk-cube8 {
  -webkit-animation-delay: 0.1s;
          animation-delay: 0.1s; }
.sk-cube-grid .sk-cube9 {
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
  } }
.test{
    margin-top: 3px !important;
    display: inline-block
}

.selects {
    display: inline-block;
    margin-top: 3%;
    width: 100vw;
}
.spans {
    border: 3px solid;
    padding: 15px;
    cursor: pointer;
    margin-right: 2%;
}
.selects .listFilter {
    list-style: none;
    position:absolute;
    z-index: 1;
    background-color: white;
    padding: 5px 40px 5px 40px;
    border: 3px solid;
}
#schoolChoices {
   position: absolute;
   left: 27.4%;
   top: 9%;
}
#typeChoices {
   position: absolute;
   left: 35%;
   top: 9%;
}
#semesterChoices {
    position: absolute;
    left: 51.75%;
    top: 9%;
}
label {
    cursor: pointer;
}
</style>