<template>
<div style="background-color: white">
    <a href="exemple_csv_student.csv" download>
        <button type="button" class="button">
            Télécharger un exemple
        </button>
    </a>

    <el-table
        :data="userListDisplay.filter(data => !search || data.firstName.toLowerCase().includes(search.toLowerCase()) || data.lastName.toLowerCase().includes(search.toLowerCase()) || data.groupName.toLowerCase().includes(search.toLowerCase()))"
        stripe
        style="width: 100%">
        <el-table-column
            prop="lastName"
            label="Nom"
            width="180"
            sortable>
        </el-table-column>
        <el-table-column
            prop="firstName"
            label="Prénom"
            width="180"
            sortable>
        </el-table-column>
        <el-table-column
            prop="groupName"
            label="Semestre actuel"
            sortable
        >
        </el-table-column>
        <el-table-column>
            <template slot="header" slot-scope="scope">
                <el-input
                    v-model="search"
                    size="mini"
                    placeholder="Taper pour chercher"/>
            </template>
        </el-table-column>
        <el-table-column>
            <template slot="header" slot-scope="scope" v-if="isAdmin">
                <CsvImport type="student"></CsvImport>
            </template>
        </el-table-column>
    </el-table>
</div>
</template>

<script lang ="ts">
import Vue from "vue"
import { User } from "../modules/classes/User"
import { BddInfo } from "../modules/classes/BddInfo"
import { Component, Prop } from "vue-property-decorator"
import { getUserList } from "../api/UserApi"
import CsvImport from "@/components/CsvImport.vue"
import { getGroupUserAccessPanel } from "../api/groupApi"
import * as SignalR from "@aspnet/signalr"
import { SignalRGestion } from "../modules/classes/SignalR"

@Component({
    components: {
     CsvImport
    }
})

export default class StudentList extends Vue {

    private bddInfo: BddInfo = new BddInfo()
    private studentList!: User[]
    private userListDisplay: User[] = []
    private search: string = ""
    private isAdmin: boolean = false
    private userState: string[] = []
    private zoneId: number = 4
    private co!: SignalR.HubConnection
    private signalr: SignalRGestion = new SignalRGestion()

     async mounted() {
        this.bddInfo.tableName = "TimedStudent"
        this.bddInfo.tableId = "StudentId"
        this.studentList = await getUserList(this.bddInfo)
        for (const user of this.studentList) {
            this.userListDisplay.push(user)
        }
        await this.checkAdmin()
        // co.state is undefined when co is not working
        this.co = this.$store.state.connectionStaffMember
        await this.co.on("RefreshList", async () => {
            this.studentList = []
            this.userListDisplay = []
            this.studentList = await getUserList(this.bddInfo)
            for (const user of this.studentList) {
            this.userListDisplay.push(user)
            }
        })
    }

    async checkAdmin() {
        this.userState = await getGroupUserAccessPanel(this.zoneId)
        for (const i of this.userState) {
            if (i == "Administration") {
                this.isAdmin = true
            }
        }
    }
}

</script>

<style>
</style>