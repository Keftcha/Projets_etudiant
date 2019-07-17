<template>
    <div class="forumresult">
<!--         <div v-for="(o, idx) in projects" :key="idx" >             
            <el-card :class="'box-card-'+idx">
                <div slot="header" class="clearfix">
                    <span>{{o.name}}</span>
                    &nbsp;
                    <el-button v-if="!projects[idx].isBlocked" @click="blockedGrade(idx)" type="warning">Bloquer les notes</el-button>
                </div>
                <div v-for="(a,value,idx2) in o.individualGrade" :key="idx2" class="text item">
                        {{value}}
                        <div v-if="projects[idx].individualGrade[value] > 0">
                            <div v-if="!projects[idx].isBlocked">
                                <el-select @change="gradeChange(idx,idx2,value)" style="width: 5%" v-model="projects[idx].individualGrade[value]" placeholder="Select">
                                            <el-option v-for="(item,index) in selector" :key="index" :value="item"></el-option>
                                </el-select><span class="grade-max">/20</span>
                            </div>
                            <div v-else>
                                 <span class="grade-max">{{projects[idx].individualGrade[value]}}/20</span>
                            </div>
                        </div>
                        <div v-else>
                            Ce Jury n'a pas encore voté ce projet
                        </div>
                        <br/>
                </div>
                Moyenne : {{o.average}}
            </el-card>
            <br/>
        </div> -->
        <el-button @click="DownloadExcel()"> Telecharger les résultats </el-button>
        <canvas id="bar-chart"></canvas>
<div id="myModal" class="modal">

  <!-- Modal content -->
  <div class="modal-content">
    <span class="close">&times;</span>
    <button class="button" v-if="!specificProject.isBlocked" @click="blockedGrade(index)" type="warning">Bloquer les notes</button>
    <div v-for="(o,idx) in specificProject.individualGrade" :key="idx">
        <div v-if="o >= 0">
              {{idx}}
            <div v-if="!specificProject.isBlocked">
                <select @change="gradeChange(idx,idx2,value)" style="width: 5%" v-model="projects[index].individualGrade[idx]" placeholder="Select">
                            <option v-for="(item,index2) in selector" :key="index2" :value="item">{{item}}</option>
                </select><span class="grade-max">/20</span>
            </div>
            <div v-else>
              {{o}} / 20
            </div>

        </div>
        <div v-else>
            {{idx}}<br> n'a pas encore voté
        </div>
        <hr>
    </div>
  </div>

</div>
    </div>
</template>

<script lang="ts">
import { Component,Vue } from 'vue-property-decorator';
import { getAllGradeProjects, downloadExcel } from '../api/forumApi';
import { ProjectForumResult } from '@/modules/classes/ProjectForumResult';
import { GetSelectorGrade, changeNoteProject, blockedNoteProject } from '../api/projectApi';
import { TypeTimedUser } from '../modules/classes/TimedUserEnum';
import{saveAs} from "file-saver"
import * as SignalR from "@aspnet/signalr"
import Chart from 'chart.js';
import { constants } from 'crypto';


@Component
export default class ForumResult extends Vue {
    private connection!: SignalR.HubConnection
    private projects: ProjectForumResult[] = []
    private projectsOrigin: ProjectForumResult[] = []
    private selector: number[]= []
    private projectName: string[] = []
    private projectAverage: number[] = []
    private chart!: Chart
    private count: number = 0
    private specificProject: ProjectForumResult = new ProjectForumResult()
    private index: number = 0


    async created(){
       this.projects = await getAllGradeProjects()
    //    this.projects.forEach(element => {
    //        this.projectsOrigin.push(element)
    //    });
    //    console.log(this.projectsOrigin)
        setInterval(this.update, 300000)

       this.selector = await GetSelectorGrade()
       await this.AddEvents()
        this.chart = new Chart(<HTMLCanvasElement>document.getElementById("bar-chart"),{
        type: 'bar',
        data:{
            labels: await this.getprojectName(),
            datasets: [{
                label:"Gagnant",
                backgroundColor:["gold", "silver", "#ad713d"],
                data: await this.getProjectAverage()
                
            }]
        },
        options:{
            responsive: true,
            onClick: this.test,
            scales:{
                yAxes:[{
                    display:true,
                    ticks:{
                        beginAtZero: true,
                        max: 20,
                        stepSize: 1
                    }
                }]
            }
        }
    })

    console.log(this.chart)
    }
    
    test(event, array){
        var modal = document.getElementById("myModal");
        var span = document.getElementsByClassName("close")[0];
        span.onclick = function() {
        modal.style.display = "none";
        console.log(array)
}

// When the user clicks anywhere outside of the modal, close it
        window.onclick = function(event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
        }
        modal.style.display = "block";
                console.log(array)

        this.specificProject = this.projects[array[0]._index]
        this.index = array[0]._index
    }
     async gradeChange(idx: number, idx2: number, key: string) {
        try {
            await changeNoteProject(this.projects[idx].projectId, this.projects[idx].individualGrade[key], this.projects[idx].jurysId[idx2], TypeTimedUser.StaffMember)
        } catch (e) {
            console.log(e)
        }
     }

     async blockedGrade(idx: number){
        try {
            await blockedNoteProject(this.projects[idx].projectId,this.projects[idx].jurysId,this.projects[idx].individualGrade)
            this.projects[idx].isBlocked = true
            await this.connection.invoke('SendBlocked',4,this.projects[idx].projectId)
        } catch (e) {
            console.log(e)
        }
     }

    async DownloadExcel(){
         let excel = await downloadExcel()
         let blob = new Blob([excel], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'})
         saveAs(blob,'ResultatFP.xlsx')
     }

     async AddEvents(){
         this.connection = await new SignalR.HubConnectionBuilder()
            .withUrl(process.env.VUE_APP_BACKEND + "/ForumHub").build()
             await this.connection.on("RefreshClassment", async () => {
                 this.count ++
                 if(this.count >= 5){
                     await this.update()
                 }
            });
            await this.connection.start()
            await this.connection.invoke("JoinRoom", 4)
     }

    async update(){
        this.projects = await getAllGradeProjects()
        await this.updateChart()
        this.count = 0
        var modal = document.getElementById("myModal");
        modal.style.display = "none"


    }
     async getprojectName(){
         this.projectName = []
         this.projects.forEach(el=>{
            this.projectName.push(el.name)
         })
         return this.projectName
     }
    async getProjectAverage(){
         this.projectAverage = []
         this.projects.forEach(el=>{
            this.projectAverage.push(el.average)
         })
         return this.projectAverage
     }

    async updateChart(){
        this.chart.data.labels = await this.getprojectName();
        this.chart.config.data.datasets[0].data = await this.getProjectAverage();
        await this.chart.update()
        console.log(this.chart.config.data.datasets[0].data)
        console.log(this.chart.data.labels)
     }
}
</script>

<style <style lang="scss">

.box-card-0{
    background: gold;
}
.box-card-1{
    background: silver;

}
.box-card-2{
    background: #ad713d;


}

.forumresult{
    height: 100vh;
}

.bar-chart{
    height: 100vh !important;

}
/* The Modal (background) */
.modal {
  display: none; /* Hidden by default */
  position: fixed; /* Stay in place */
  z-index: 1; /* Sit on top */
  left: 0;
  top: 0;
  width: 100%; /* Full width */
  height: 100%; /* Full height */
  overflow: auto; /* Enable scroll if needed */
  background-color: rgb(0,0,0); /* Fallback color */
  background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
}

/* Modal Content/Box */
.modal-content {
  background-color: #fefefe;
  margin: 15% auto; /* 15% from the top and centered */
  padding: 20px;
  border: 1px solid #888;
  width: 80%; /* Could be more or less, depending on screen size */
}

/* The Close Button */
.close {
  color: #aaa;
  float: right;
  font-size: 28px;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}
</style>
