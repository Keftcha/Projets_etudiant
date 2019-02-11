<template>
    <div>
            <h1>Voici vos schémas</h1>
            <el-table :data="data" style="width: 100%">
                <el-table-column prop="schemaId" label="ID" width="100"></el-table-column>
                <el-table-column prop="schemaName" label="Nom"></el-table-column>
                <el-table-column  prop="groupName" label="Groupe"></el-table-column>
                <el-table-column prop="schemaId" label="Action">
                    <template slot-scope="scope">
                        <el-button size="mini" @click="deleteSchema(scope.$index)">Edit</el-button>
                        <el-button size="mini" type="danger" @click="deleteSchema(scope.$index)">Delete {{scope.$index}}</el-button>
                    </template>
                </el-table-column>
            </el-table>
        
    </div>
</template>

<script>
import AuthService from '../../../services/AuthService'
import { state } from "../../../state"
import{ findByEmail} from '../../../api/userApi'
import { GetSchemaByIdAsync, DeleteSchemaByIdAsync} from "../../../api/schemaApi"
  export default {
    data() {
      return {
          data: null,
      }
    },

    async mounted() {
        await this.getUserId();
        await this.refreshData(this.User.userId);
        console.log(this.User);
    },

  
    computed: {
        auth: () => AuthService,
    },

    methods: {
        async getUserId(){
        try {
        this.User = await findByEmail(this.auth.email);
        console.log(this.User);
        }
        catch (e) {
                console.error(e);
                state.exceptions.push(e);
            }

        },

        async refreshData(userId){
            state.isLoading = true;
            this.data = await GetSchemaByIdAsync(userId);
            state.isLoading = false;
        },

        async deleteSchema(indexData){
            state.isLoading = true;
            await DeleteSchemaByIdAsync(this.data[indexData].schemaId);
            state.isLoading = false;
            window.location = "/app/viwork/schemalist";
        }
    }

  };
</script>