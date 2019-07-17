<template>
    <div>
        <button type="button" class="button" icon="el-icon-document-add" @click="dialogVisible = true">Importer un fichier .csv</button>
    
        <el-dialog
            title="Ajouter une liste via un csv"
            :visible.sync="dialogVisible"
            width="30%"
            class="dialog--uploadxcel"
        >
            <el-form enctype="mutltipart/form-data">
                <input type="file" name="file" accept=".csv" ref="file" @change="handleFileChange()"/>
                <el-button type="button" @click="submitFile()">télécharger</el-button>
            </el-form>
        </el-dialog>
    </div>
</template>

<script lang="ts">
import Vue from "vue"
import { Component, Prop, Watch } from "vue-property-decorator"
import { getUserList } from "../api/UserApi"
import axios from "axios"
import { sendCsv } from "../api/csvApi"
import { getIdSchoolOfUser } from '../api/schoolApi';

@Component
export default class CsvImport extends Vue {
    @Prop(String) type!: ""
    private dialogVisible: boolean = false
    private file: any = ""
    private env: any = process.env.VUE_APP_BACKEND
    private formData: FormData = new FormData()
    private idZone: number = 0
    private test!: any

    handleFileChange() {
        const finalFile =  this.$refs.file.files[0]
        this.formData.append("file", finalFile)
        this.formData.append("type", this.type)
        for ( const i of this.formData.entries()) {
            console.log(i)
        }
    }
    async submitFile() {
        try {
            console.log("eee")
            console.log(this.type)
         this.idZone = await getIdSchoolOfUser()
         console.log(this.idZone)
         this.test = await sendCsv(this.formData)
         this.$refs.file.files[0] = ""
        } catch (e) {
            console.log(e)
        } finally {
            console.log(this.test)
            this.dialogVisible = false
            const co = this.$store.state.connectionStaffMember
            await co.invoke("RefreshList", this.idZone)
            
        }
        if(this.type == "jury"){
        this.$notify({
          title: 'envoi réussi',
          message: 'les jurys ont bien été assignés à un projet. rendez-vous sur la page des résultats pour plus d\'informations',
          duration: 0
        });
        }
    }
}
</script>

<style lang="scss">
.dialog--uploadxcel{
    margin-left: 5%
}


.el-table th div {
 white-space:initial;
 display: block;
}
</style>
