<template>
    <div class="addJury">
        <div><br>
            <span>Attribuer un n° aux projets</span>
            <CsvImport type="projectNumber"></CsvImport>
            <br>
            <a href="exemple_csv_number_project.csv" download>
                <button type="button" class="button">
                    Télécharger un template
                </button>
            </a>
        </div>
        <br>
        <span v-if="countProjetWNumber() == 0">
            Aucun projet ne s'est vu attribuer un numéro pour cette période.
        </span>
        <span v-else-if="countProjetWNumber() == 1">
            {{countProjetWNumber()}} projet s'est vu attribuer un numéro pour cette période.
        </span>
        <span v-else>
            {{countProjetWNumber()}} projets se sont vus attribuer un numéro pour cette période.
        </span>
        <br>
        <div>  
            <span><h3>Attribuer les projets aux jurys</h3></span>
            <CsvImport type="jury"></CsvImport>
            <br>
            <a href="example_csv_jury.csv" download>
                <button type="button" class="button">
                    Télécharger un template
                </button>
            </a>
        </div>
    </div>
</template>


<script lang="ts">
import { Component, Vue } from "vue-property-decorator"
import CsvImport from "@/components/CsvImport.vue"
import { getProjects } from "../api/forumApi"
import { ForumProject } from '../modules/classes/ForumProject';
import { Project } from '../modules/classes/Project';

@Component({
    components: {
     CsvImport
    }
})
export default class AddJury extends Vue {
    private projects: Project[] = new Array()

    async mounted(){
        this.projects = await getProjects()
        console.log(this.projects)
    }

    countProjetWNumber(){
        if(this.projects.length != 0 ) return this.projects.length 
        return 0
    }
}
</script>
<style>
.addJury {
    height: 100vh;
}
</style>
