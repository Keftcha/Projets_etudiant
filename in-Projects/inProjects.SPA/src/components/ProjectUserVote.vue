<template>
    <div class="userVote">
        <div>
        <el-select @change="change()" v-model="options" placeholder="Select">
            <el-option v-for="item in schoolOptions" :key="item.schoolId" :value="item.name">
            </el-option>
        </el-select>
        <br/>

        <div v-if="forumOff == true && changeSchool==true">
            <h2> Le Forum PI n'est pas ouvert !</h2>
            <countdown :schoolName="schoolName"></countdown>
        </div>

        <div v-if="projectList.length >0" style="background-color: white">
            <el-row>
                <el-col :span="5" v-for="(o, index) in projectList.length" :key="o" :offset="index > 0 ? 1 : 1" >            
                    <el-card v-bind:body-style="{ padding: '3px'}">
                        <img :src="projectList[index].logo"  @click="redirect(projectList[index].projectStudentId)" style="height: 100px; width: 100px">
                        <div class="my-card-row">
                            <span>{{projectList[index].groupName}}</span><br>
                            <div class="block">
                                <el-rate @change="note(projectList[index].grade,projectList[index].projectStudentId)" v-model="projectList[index].grade"></el-rate>
                            </div>
                        </div>
                    </el-card>
                    <br/>
                </el-col>
            </el-row>
        </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator"
import { GetAllProject, GetAllTypeProjectsOfSchool, noteProjectUser } from "../api/projectApi"
import { getSchools } from "../api/schoolApi"
import {Project} from "../modules/classes/Project"
import {School} from "../modules/classes/School"
import { TypeTimedUser } from "@/modules/classes/TimedUserEnum"
import Countdown from "@/components/Countdown.vue"
import { GetEventSchoolByName } from '../api/eventApi';
import { Event } from '../modules/classes/EventSchool';


@Component({
    components: {
        Countdown,
    }
})
export default class ProjectUserVote extends Vue {
    private nextForum: Event = new Event()
    private projectList: Project[] = []
    private projectListToDisplay: Project[] = []
    private schoolOptions: School[] = []
    private options: string = ""
    private type: string = "I"
    private schoolId: number = 0
    private numberMax = 4
    private forumOff = true
    private changeSchool = false
    private schoolName = ""


    async created() {
      this.schoolOptions = await getSchools()
    }

    async change() {
        try {
            this.projectList = []
            this.changeSchool = true;
            this.forumOff = true;
            let school = this.schoolOptions.find(x => x.name == this.options)
            if (school == undefined) {
                school = new School(0, "Unknown")
            }
            this.schoolId = school.schoolId
            this.schoolName = school.name
            this.nextForum = await GetEventSchoolByName("ForumPI",this.schoolName)
            let dateNow = new Date()

           if(dateNow >= this.nextForum.begDate && dateNow <= this.nextForum.endDate){
                this.changeSchool = false;
                this.forumOff = false
                this.projectList  = await GetAllTypeProjectsOfSchool(this.schoolId, this.type)
                for (let index = 0; index < this.projectList.length; index += 1 ) {
                    this.projectList[index].grade = Math.ceil(this.projectList[index].grade / 4)
                }
           }
            
        } catch (e) {
            console.log(e.message)
        }
    }

    getStars(grade: number) {
        const idx = Math.ceil(grade / 4)

        return idx
    }

   async note(newGrade: number, id: number) {
        try {
            const grade = newGrade * this.numberMax
            await noteProjectUser(id, grade, this.schoolId, TypeTimedUser.Anon)
        } catch (e) {
            console.log(e)
        }
    }

     redirect(idProject: string) {
        this.$router.replace("/Project/" + idProject)
    }
}
</script>

<style>
.userVote{
    height: 100vh;    
}

</style>
