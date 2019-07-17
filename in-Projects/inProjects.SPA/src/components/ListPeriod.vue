<template>
    <div>
        <Error :error="error"/>
        <el-table :data="listPeriods">
             <el-table-column label="Nom de la période" prop="groupName" ></el-table-column>
             <el-table-column label="Date de debut">
                  <template slot-scope="scope">
                    <el-date-picker :clearable="false" v-model="listPeriods[scope.$index].begDate"  @change="changeDate(scope.$index)" type="date">    
                    </el-date-picker>
                 </template>
             </el-table-column>
             <el-table-column label="Date de fin">
                  <template slot-scope="scope">
                    <el-date-picker :clearable="false" v-model="listPeriods[scope.$index].endDate"  @change="changeDate(scope.$index)" type="date">    
                    </el-date-picker>
                 </template>
             </el-table-column>
        </el-table>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator"
import { Period } from "@/modules/classes/Periode/Period"
import { getAllPeriod, changeDateOfPeriod} from "../api/periodApi"
import Error from "./Erreur.vue"
import { getIdSchoolOfUser } from '../api/schoolApi';

@Component({
    components: {
        Error
    }
})
export default class ListPeriod extends Vue {
    public error: string[] = []
    private listPeriods: Period[] = []
    private idZone: number = 0


    async created() {
        this.idZone = await getIdSchoolOfUser()
        this.listPeriods = await getAllPeriod(this.idZone)
    }
    async changeDate(idx: number) {
        let begDate!: Date
        let endDate!: Date

        begDate = new Date(this.listPeriods[idx].begDate)
        endDate = new Date(this.listPeriods[idx].endDate)

         // Change hours to compare equals Date, if Date is the same but hours different begDate might be bigger than endDate
        begDate.setHours(0, 0, 0, 0)
        endDate.setHours(0, 0, 0, 0)

        this.error = []

        if (begDate >= endDate) {
            this.error.push("Date de debut superieur ou egale à celle de fin")
        } else {
            try {

             begDate.setDate(begDate.getDate() + 1 )
             endDate.setDate(endDate.getDate() + 1)

             await changeDateOfPeriod(this.listPeriods[idx].childId, begDate, endDate)
             this.$message({
                    message: "La date de la période " + this.listPeriods[idx].groupName + " a bien été changé",
                    type: "success"
                })
            } catch (e) {
                this.error.push(e.message)
            }

            this.listPeriods = await getAllPeriod(this.idZone)
        }
    }
}
</script>

<style>

</style>
