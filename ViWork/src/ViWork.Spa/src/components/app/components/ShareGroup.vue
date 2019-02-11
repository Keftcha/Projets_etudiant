<template>
    <div>
        <el-row>
            <el-col :span="10">
                <h1>Voici la page du groupe : <strong>{{data.groupName}}</strong></h1>
            </el-col>
            <el-col :span="6">
                <el-button type="danger" @click="deleteGroup()">Supprimer groupe</el-button>
                <el-button type="success" @click="dialogVisible = true">Créer un schéma</el-button>
            </el-col>
            
            

            <el-dialog title="Créer un schéma" :visible.sync="dialogVisible" width="30%">
                <el-form model="form" label-width="120px" size="medium">
                        <el-form-item label="Nom du schéma">
                            <el-input v-model="name" placeholder="Nom du schéma"></el-input>
                        </el-form-item>
                    </el-form>
                <span slot="footer" class="dialog-footer">
                    <el-button @click="dialogVisible = false">Annuler</el-button>
                    <el-button type="primary" @click="createSchema()">Créer</el-button>
                </span>
            </el-dialog>
        </el-row>
    </div>
</template>

<script>
import {getGroupData, deleteGroupAsync} from '../../../api/groupApi'
import {createSchemaAsync} from "../../../api/schemaApi"
import { state } from "../../../state"
export default {
    data(){
        return{
            dialogVisible: false,
            data: null,
            name: null,
            model: {
                SchemaName: null,
                GroupId: null
            },
        }
    },

    async mounted() {
        await this.refreshData()
    },

    methods: {
         async refreshData() {
            try {
                state.isLoading = true;
                console.log( " " + this.$route.params.id)
                this.data = await getGroupData(this.$route.params.id);
            }
            catch (e) {
                console.error(e);
                state.exceptions.push(e);
            }
            finally {
                state.isLoading = false;
            }
        },

        async deleteGroup() {
            this.groupId = this.$route.params.id;
            console.log(this.$route.params.id)
            await deleteGroupAsync(this.groupId);
            window.location = "/app/viwork/schemalist";
        },
        
        async createSchema() {
            this.groupId = this.$route.params.id;
            this.model.SchemaName = this.name;
            this.model.GroupId = parseInt(this.groupId, 10);
            console.log(this.model);
            await createSchemaAsync(this.model);
            window.location = "/app/viwork/schemalist";
        }
    }
}
</script>
