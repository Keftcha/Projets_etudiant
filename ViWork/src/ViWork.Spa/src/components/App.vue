<template>
    <div id="app">
        <el-container>
            <el-header style="padding: 0; margin-bottom: -3px">
                <el-row  id="header">
                    <el-menu mode="horizontal" background-color="#545c64" text-color="#fff" active-text-color="orange" :router="true">
                        <el-col :sm="3" :md="3" :lg="2" :xl="2">
                            <el-menu-item index="/" class="brand"><img src="../assets/favicon.png" height="30" width="30"><span> ViWork</span></el-menu-item>
                        </el-col>
                        <el-col :sm="2" :md="2" :lg="2" :xl="1">
                                <el-menu-item index="/home">Home</el-menu-item>
                            </el-col>
                            <el-col :sm="2" :md="2" :lg="2" :xl="1">
                            <el-menu-item index="/test">Test</el-menu-item>
                            </el-col>
                            <el-col :sm="2" :md="2" :lg="2" :xl="1">
                            <el-menu-item index="/about">About</el-menu-item>
                            </el-col>
                            <el-col :sm="11" :md="12" :lg="13" :xl="17" v-if="!auth.isConnected"><el-menu-item index="56" disabled></el-menu-item></el-col>
                            <el-col :sm="10" :md="10" :lg="11" :xl="16" v-if="auth.isConnected"><el-menu-item index="57" disabled></el-menu-item></el-col>
                            <el-col :sm="2" :md="2" :lg="2" :xl="1"  v-if="auth.isConnected">
                                <el-submenu index="1" >
                                    <template slot="title"><i class="el-icon-setting"></i></template>
                                    <el-menu-item index="/logout">Déconnexion</el-menu-item>
                                </el-submenu>
                            </el-col>
                            <el-col :sm="3" :md="2" :lg="3" :xl="2"  v-if="auth.isConnected">
                                <router-link to="/app/viwork/schemalist" >
                                    <el-button type="warning" style="margin-top: 10px" index="/app/viwork/schemalist"><i class="el-icon-menu"></i> Lancer</el-button>
                                </router-link>
                            </el-col>

                            <el-col :sm="4" :md="3" :lg="3" :xl="2"  v-if="!auth.isConnected">
                                <el-submenu index="2">
                                    <template slot="title">Connexion</template>
                                    <el-menu-item index="2-1" @click="login('GitHub')"><el-button type="success">via GitHub</el-button></el-menu-item>
                                    <el-menu-item index="2-2" @click="login('Base')"><el-button type="primary">via ViWork</el-button></el-menu-item>
                                </el-submenu>
                            </el-col>
                        </el-menu>
                </el-row>

                <div class="progress" v-if="isLoading">
                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 100%"></div>
                </div>
            </el-header>
            
            <el-main style="padding: 0">
                <main role="main">
                    <router-view class="child"></router-view>
                </main>
            </el-main>
        </el-container>
    </div>
</template>
<script>
import AuthService from '../services/AuthService'
import '../directives/requiredProviders'
import { state } from "../state"
export default {
    data() {
        return {
            
            state,
            activeIndex: this.$route.path,
            dialogFormVisible: false,
            endpoint: null
        }
    },
    mounted() {
        AuthService.registerAuthenticatedCallback(() => this.onAuthenticated());
    },
    beforeDestroy() {
        AuthService.removeAuthenticatedCallback(() => this.onAuthenticated());
    },
    methods: {
      login(provider) {
            AuthService.login(provider);
        },
        onAuthenticated() {
            window.location = "/";
        }
    },
    computed: {
        auth: () => AuthService,
        
        isLoading() {
            return this.state.isLoading;
        }
    }
}
</script>

<style lang="scss" scoped>
.progress {
  margin-top: -20px;
  padding: 0px;
  height: 10px;
}
  .el-row {
    margin-bottom: 20px;
    &:last-child {
      margin-bottom: 0;
    }
  }
  .el-col {
    border-radius: 4px;
    text-align: center;
  }
  .bg-purple-dark {
    background: #99a9bf;
  }
  .bg-purple {
    background: #d3dce6;
  }
  .bg-purple-light {
    background: #e5e9f2;
  }
  .grid-content {
    border-radius: 4px;
    min-height: 36px;
  }
  .row-bg {
    padding: 10px 0;
    background-color: #f9fafc;
  }
.navbarcolor{
    background-color: #545c64;
}
</style>

<style lang="scss">
@import "../styles/global.scss";
</style>