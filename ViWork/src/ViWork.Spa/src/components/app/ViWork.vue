<template>
    <div>
        <el-container>
            <el-aside>
                <el-menu :default-active="activeIndex" :default-openeds="['1']" :router="true" style="padding-top: 10px">
                    <h5 style="margin-left: 25px">Mes Documents</h5>
                    <el-menu-item index="schemalist">
                        <i class="el-icon-document"></i>
                        <span>Tableau de bord</span>
                    </el-menu-item>
                    <el-submenu index="1">
                        <template slot="title">
                        <i class="el-icon-share"></i>
                        <span>Partagés avec moi</span>
                        </template>
                        <!--el-menu-item-group title="Schémas de :">
                            <el-menu-item index="/app/viwork/shareuser">Baptiste</el-menu-item>
                            <el-menu-item index="/app/viwork/shareuser">Alban</el-menu-item>
                            <el-menu-item index="/app/viwork/shareuser">Mam's</el-menu-item>
                        </el-menu-item-group-->
                        <el-menu-item-group title="Mes groupes :" >
                            <div v-for="i of data" :key="i.groupId">
                                <el-menu-item :index="`/app/viwork/sharegroup/${i.groupId}`">{{i.groupName}}</el-menu-item>
                            </div>
                            <el-menu-item><el-button type="primary" @click="dialogVisible = true" style="margin auto;">Nouveau groupe</el-button></el-menu-item>
                        </el-menu-item-group>
                    </el-submenu>
                </el-menu>
            </el-aside>

        <el-dialog title="Créer un groupe" :visible.sync="dialogVisible" width="30%">
                <el-form model="form" label-width="120px" size="medium">
                    <el-form-item label="Nom du groupe">
                        <el-input v-model="name" placeholder="Nom du groupe"></el-input>
                    </el-form-item>
                </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">Annuler</el-button>
                <el-button type="primary" @click="createGroup()">Créer</el-button>
            </span>
        </el-dialog>

        <el-main>
            <main role="main">
                <router-view class="child"></router-view>
            </main>
        </el-main>
        </el-container>
    </div>
  
</template>

<script>
import AuthService from '../../services/AuthService'
import {createGroupAsync, getGroupListAsync} from '../../api/groupApi'
import { state } from "../../state"
import{ findByEmail} from '../../api/userApi'

export default {
    data() {
        return {
            page: this.$route.path,
            activeIndex: null,
            dialogVisible: false,
            model: {
                GroupName: null,
                OwnerID: null
            },
            User:[],
            data: [],
            email: null,
            name: null
         
        }
    },
     async mounted() {
        await this.getUserId();
        await this.refreshData(this.User.userId);
        //console.log(this.User.userId);
    },

  
     computed: {
        auth: () => AuthService,
    },

    async created() {
        var splitvar = this.page.split("/");
        this.activeIndex = splitvar[3];
    },

    methods: {
        async getUserId(){
        try {
        this.User = await findByEmail(this.auth.email);
        }
        catch (e) {
                    console.error(e);
                    state.exceptions.push(e);
            }

    },
        async refreshData() {
            try {
                state.isLoading = true;
                this.data = await getGroupListAsync(this.User.userId);
            }
            catch (e) {
                console.error(e);
                state.exceptions.push(e);
            }
            finally {
                state.isLoading = false;
            }
        },

        async createGroup() {
            this.model.GroupName = this.name;
            this.model.OwnerID = this.User.userId;
            console.log(this.model);
            await createGroupAsync(this.model);
            this.dialogVisible = false;
            window.location = "/app/viwork/schemalist";
        },
    }

   
}
</script>

