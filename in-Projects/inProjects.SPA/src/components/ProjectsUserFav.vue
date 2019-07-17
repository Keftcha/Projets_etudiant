<template>
    <div>
        <div v-if="usersFav.length == 0">
            Vous n'aimez aucun projet 
            <br/>
            <img class="image-grumpy" src="../assets/grumpyCat.jpg">
        </div>
        <div v-else>
<!--               <el-carousel  class="carousel-fav" trigger="click" :interval="4000" type="card">
                <el-carousel-item class="carousel-item-fav" v-for="(o, idx) in usersFav" :key="idx">
                    <span class="text-fav">{{o.groupName}}</span>
                    <img @click="redirect(o.projectId)" class="image-fav" :src="o.logo"/>
                </el-carousel-item>
            </el-carousel>  -->
            <div>
                <datalist id="languages" >
                    <option v-for="(o, idx) in usersFav" :key="idx">{{o.groupName}}</option>
                </datalist>
                <label>Chercher un projet</label>
                <input class="input-fav" type="text" list="languages" v-model="groupName">
                &nbsp
                <button class="button" @click="goToProject()" type="button">Se rendre sur la page du projet</button> 
            </div>
            <div class="masonry-layout-fav">
                <div class="masonry-layout-panel-fav" v-for="(o, idx) in usersFav" :key="idx">
                    <div class="masonry-layout-panel__content-fav">
                        <h5 class="mansonry-groupName">{{o.groupName}}</h5>
                        <img :src="o.logo" style="height:100px; width:100px; cursor:pointer;"  @click="redirect(o.projectId)">
                    </div>
                </div>
            </div>
        </div>
  </div>
</template>


<script lang="ts">
import { Component, Vue } from "vue-property-decorator"
import { ProjectFav } from "@/modules/classes/ProjectFav"
import { getProjectsFav} from "../api/accountApi"

@Component
export default class ProjectUserFav extends Vue {
    usersFav: ProjectFav[] = []
    private groupName!: string

    async created() {
        this.usersFav = await getProjectsFav()
    }

    redirect(idProject: string) {
        this.$router.push("/Project/" + idProject)
    }

    goToProject(){
        this.usersFav.forEach(element => {
            if(element.groupName == this.groupName) this.redirect(element.projectId.toString())
        });
    }
}
</script>


 <style>
.image-fav{
    height: 200px;
    width: 350px;
    padding-top:10%;
    margin-left :10%;
}
.image-grumpy{
    height: 20vh;
    width: 30vw;

}
.carousel-fav{
      height:100%; 
      width:100%;
}
  .carousel-item-fav {
    background-color: transparent;
    width:20vw;
    margin-left: 10%
  }
 
.text-fav{
    background :#cccccc;
    color: black;
    position: absolute;
    text-align: start;
    width: 100%;
} 

.masonry-layout-fav {
    column-count: 8;
    column-gap: 0;
    padding: 0;
    height: 321pt;
    overflow: auto;
}
.masonry-layout-panel-fav {
    break-inside: avoid;
    padding: 5px;
}
.masonry-layout-panel__content-fav {
    padding: 10px;
    border-radius: 10px;

}

.mansonry-groupName{
    padding: 10px

}

.input-fav{
    width: auto !important;
    border: 1px solid !important;
    padding: 8px !important
    
}
</style>
