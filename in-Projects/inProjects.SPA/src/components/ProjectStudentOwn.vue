<template>
    <div>
        <div v-if="ownProjects.length == 0">
            Vous n'avez aucun projet 
            <br/>
            <img class="image-grumpy" src="../assets/grumpyCat.jpg">
        </div>
        <div v-else>
            <el-carousel class="carousel-proj" trigger="click" :interval="4000" type="card" >
                <el-carousel-item  class="carousel-item-proj" v-for="(o, idx) in ownProjects" :key="idx">
                        <span class="text-project">{{o.name}}</span>
                        <img @click="redirect(o.projectStudentId)" class="image-proj" :src="o.logo"/>
                </el-carousel-item>
            </el-carousel>
        </div>
  </div>
</template>


<script lang="ts">
import { Component, Vue } from "vue-property-decorator"
import { OwnProject } from "@/modules/classes/OwnProject"
import { getOwnProjects} from "../api/accountApi"

@Component
export default class ProjectStudentOwn extends Vue {
      ownProjects: OwnProject[] = []

    async created() {
        this.ownProjects = await getOwnProjects()
    }

    redirect(idProject: string) {
        this.$router.push("/Project/" + idProject)
    }
}
</script>

<style>
.image-proj{
    height: auto;
    width: 10vw;
    padding-top:10%;
    padding-bottom: 100%;
    margin-left :10%;
}

.image-proj-grumpy{
    height: 20vh;
    width: 30vw;

}

.carousel-proj{
      height:100%; 
      width:100%;
}

.carousel-item-proj {
    background-color: transparent;
    width:20vw;
    margin-left: 10%
}
 
.text-project{
    background :#cccccc;
    color: black;
    position: absolute;
    text-align: start;
    width: 100%;
    
}


</style>
