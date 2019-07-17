<template>
    <div class="forum-result-public">
        <table class="table-forum-result-public">
        <tr v-for="(item, index) in  projectsPublicResult.slice(0, Math.round(projectsPublicResult.length/2))" :key="index">
                    <td><span style="font-size:30px"> {{item.name}}</span></td>
                    <td><span style="font-weight: bold; font-size:30px" class="percentage-forum-result-public">{{getPercentage(index)}}%</span></td>
                    <td >
                        <div class="w3-light" style="width: 25vw">
                             <div class="w3-blue" :style="'height:30px;width:'+getPercentage(index) +'%'"></div>
                         </div>

                         
                    </td>
                    
                    <div v-if="(Math.round(projectsPublicResult.length/2) + index) < projectsPublicResult.length ">
                        <td><span style="font-size:30px">{{projectsPublicResult[Math.round(projectsPublicResult.length/2) + index].name}}</span></td>
                        <td><span style="font-weight: bold; font-size:30px" class="percentage-forum-result-public">{{getPercentage(Math.round(projectsPublicResult.length/2) + index)}}%</span></td>
                        <td>
                            <div class="w3-light" style="width: 25vw">
                                <div class="w3-blue" :style="'height:30px;width:'+getPercentage(Math.round(projectsPublicResult.length/2) + index) +'%'"></div>
                            </div>
                        </td>
                    </div>
        </tr>

        </table>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { ProjectForumResult } from '../modules/classes/ProjectForumResult';
import { getAllPublicNote } from '../api/forumApi';

@Component
export default class ForumResultPublic extends Vue {
    private projectsPublicResult : ProjectForumResult[] = []
    private diviseur : number = 0

    async created(){
        this.projectsPublicResult = await getAllPublicNote()
        console.log(this.projectsPublicResult)

        const reducer = (accumulator : number, currentValue : ProjectForumResult) => accumulator + currentValue.average;

        this.diviseur = this.projectsPublicResult.reduce(reducer,0);
        if(this.diviseur ==0) this.diviseur = 1
        setInterval(this.update, 18000)
    }

    async update(){
        this.projectsPublicResult = await getAllPublicNote()
        const reducer = (accumulator : number, currentValue : ProjectForumResult) => accumulator + currentValue.average;

        this.diviseur = this.projectsPublicResult.reduce(reducer,0);
    }

    getPercentage(index: number){
       let number = this.projectsPublicResult[index].average/this.diviseur * 100
       let percentage = number.toFixed(2)
       return percentage
    }
}
</script>

<style>
@import '../styles/w3.css';

.span-name-project-result{
font-weight: bold;
text-align: end
}

.forum-result-public{
   overflow-x: hidden;
   
}

.percentage-forum-result-public{
    /* margin-left: 100% */
}

.w3-light{
    /* margin-left: 3%; */

}

.w3-blue{
    border: 1px solid black;
    background-color: #75afff!important
}

.table-forum-result-public{
    /*padding-top: 7%*/
}

.forum-result-public{
    height: 100vh;
}
</style>
