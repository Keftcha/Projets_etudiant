<template>
    <div class="create">
        <details>
            <summary style="padding-top: 1%; text-align:left;">Créer une nouvelle période</summary> 
            <br>
            <div v-if="isInPeriod == false">
                <Error  :error="error"/>
                <b>Dates pour la nouvelle période :</b> 
                <br><br>           
                    <el-date-picker
                    v-model="date"
                    type="daterange"
                    align="right"
                    start-placeholder="Date de début"
                    end-placeholder="Date de fin">
                    </el-date-picker>
                    <br><br>
                    <input class="el-input-groupe" placeholder="Ajouter un groupe" v-model="groupName">
                    &nbsp;
                    <button class="button" @click="Add()">Ajouter</button>
                    <br><br>
                    <label>Choissisez les groupes de la nouvelle période :</label>                    
                    <div class="square-container">
                        <div class="card-container">Groupes à ajouter: 
                            <el-card class="square">
                                <el-table :data="listGroup" class="list-groups-period">
                                    <el-table-column label="Nom" prop="name" ></el-table-column>
                                    <el-table-column label="Supprimer" width="90">
                                        <template slot-scope="scope">
                                            <img src="../assets/delete.png" style="width:32px; height:32px;" @click="Delete(scope.$index)">
                                        </template>
                                    </el-table-column>
                                </el-table>
                            </el-card>
                        </div>
                        <div class="card-container">Groupes supprimés: 
                            <el-card class="square">
                            <el-table :data="listRemove" class="list-groups-period">
                                    <el-table-column label="Name" prop="name" ></el-table-column>
                                    <el-table-column label="Ajouter" wi dth="90">
                                        <template slot-scope="scope">
                                            <img src="../assets/add.png" @click="AddToGroup(scope.$index)">
                                        </template>
                                    </el-table-column>
                                </el-table>
                            </el-card>
                        </div>
                    </div>
                    <br>
                    <button class="button" @click="CreatePeriod()">Créer Période</button>
                </div>
                <div v-else>
                <label>Une période en cours, vous ne pouvez pas créer de périodes !</label>
                <br>
                <img src="../assets/notpass.gif">
            </div>
        </details>
        <details>
        <summary style="text-align:left;">Afficher la liste des périodes</summary>
            <ListPeriod></ListPeriod>
        </details>
    </div>

</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator"
import { DatePickerType } from "element-ui/types/date-picker"
import { PeriodCreate } from "@/modules/classes/Periode/PeriodCreate"
import { GroupPeriod } from "@/modules/classes/Periode/GroupPeriod"
import { createPeriodAsync, verifyActualPeriod} from "../api/periodApi"
import { getTemplateGroupsAsync } from "../api/groupApi"
import Error from "./Erreur.vue"
import { getIdSchoolOfUser } from '../api/schoolApi';
import ListPeriod from "./ListPeriod.vue";
@Component({
    components: {
        Error,
        ListPeriod
    }
})

export default class CreatePeriod extends Vue {
    public error: string[] = []
    private date: Date[] = []
    private listGroup: GroupPeriod[] = []
    private groupName: string = ""
    private listRemove: GroupPeriod[] = []
    private isInPeriod: boolean = false
    private idZone!: number

    async created() {
        this.idZone = await getIdSchoolOfUser()
        this.listGroup = await getTemplateGroupsAsync(this.idZone)
        this.isInPeriod = await verifyActualPeriod()
    }

    async CreatePeriod() {
        const date = new Date()

        this.error = []
        if (this.date.length != 0) {
            const pc: PeriodCreate = new PeriodCreate()
            pc.begDate = this.date[0]
            pc.endDate = this.date[1]
            pc.Kind = "S"
            pc.Groups = this.listGroup
            pc.idZone = this.idZone
            try {
                const createdPeriod = await createPeriodAsync(pc)
                this.$message({
                    message: "La période a été créée avec succès.",
                    type: "success"
                })
                 this.$router.replace("/")
            } catch (e) {
                console.log(e.response)
                this.error.push(e.response.data)
            }
        } else {
            this.error.push("Les dates ne sont pas choisies")
        }
    }

    AddToGroup(idx: number) {
         this.listGroup.push(this.listRemove[idx])
         this.listRemove.splice(idx, 1)
    }

    getState(idx: number) {
        return this.listGroup[idx].state
    }

    setState(idx: number) {
        this.listGroup[idx].state = !this.listGroup[idx].state
    }

    Add() {
        if (this.groupName.trim()) {
            const addToList: GroupPeriod = new GroupPeriod()
            addToList.name = this.groupName
            addToList.state = false
            addToList.isAlreadyPermanent = false
            this.listGroup.push(addToList)
        }
    }

    Delete(idx: number) {
        this.listRemove.push(this.listGroup[idx])
        this.listGroup.splice(idx, 1)
    }

    Test() {
        this.error = []
        console.log(this.listGroup)
    }
}
</script>


<style>
.create{
    height: 100vh;
    font-size: 20px;
}
   .list-groups-period {
        margin-left: 1%;
      
    }
   .el-input-groupe{
       width: 10%;
       font-size: 0.9rem;
        height: 2.5rem;
   }
   .square-container{
       display: flex;
       justify-content: space-evenly;
   }
   .card-container{
       display: flex;
       flex-direction: column;
       
       width: 30vw;
   }
   .square {
    -webkit-box-pack: center;
    -ms-flex-pack: center;
    justify-content: space-evenly;
    

    overflow: hidden;
    overflow-y: auto;
    max-height: 50vh;
    height: 100%;
    width: 100%;
    }
    .el-icon-remove:hover{
        opacity: 0.6;
        transform: scale(1.4);
        cursor: pointer;
        transition: all 0.5s;
    }
    .el-icon-circle-plus:hover{
        opacity: 0.6;
        transform: scale(1.4);
        cursor: pointer;
        transition: all 0.5s;
    }
    .switch-container{
        width: 11vw;
        
    }
    .delete-container{
      
    }

    #keep{
    border: 4px solid rgb(0,255,0);
    }
    
    #remove{
    border: 4px solid rgb(220,20,60);
    }
    .createPeriod{
        margin-top: 1%
    }
    
    .card {
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
  transition: 0.3s;
  width: 40%;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
}

.container {
  padding: 2px 16px;
  height: 50vh;
  width: 10hv;
  overflow: scroll

}
</style>
