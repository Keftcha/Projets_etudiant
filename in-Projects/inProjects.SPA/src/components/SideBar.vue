<template>
<div class="nav">
  <ul class="sidenav">
    <li>      
        <a class="active" @click="redirect('/')"><img src="../assets/home.png" height="24px" width="24px" style="margin-bottom: 5px;">&nbsp;IN'TECH</a>
        <div class="menu-toggle">
          <font-awesome-icon aria-hidden="true" icon="bars" size="lg" @click="changeCollapse()" style="color: white; cursor: pointer;"/>
        </div>
    </li>
    <div class="collapsedMenu">
      <div v-if="authService.authenticationInfo.level != 0">
          <li class="btn-group">
              <button class="buttons" @click="logout()">
                  <img src="../assets/deco.png" height="30px">
              </button>
              <button class="buttons" @click="redirect('/MyProfil')">
                  <img src="../assets/profile.png" height="30px"/>
              </button>
          </li>
        </div>
        <div v-else>
          <li class="btn-group">
              <button class="buttons" @click="redirect('/connection')" style="width: 100%;">
                  <img src="../assets/co.png">
              </button>
          </li>
        </div>
        <div v-for="(o, idx) in whatTimed" :key="idx">
            <div v-if="o == 'Administration'">
                <AdminPanel></AdminPanel>
            </div>
            <div v-if="o == 'User'">
                <UserPanel></UserPanel>
            </div>
            <div v-if="o == 'Student'">
                <StudentPanel></StudentPanel>
            </div>
              <div v-if="o == 'Jury'">
                <JuryPanel></JuryPanel>
            </div>
            </div>
        </div>
  </ul>
</div>
</template>

<script lang="ts">
import { Component, Vue, Watch} from "vue-property-decorator"
import UserInfoBox from "./UserInfoBox.vue"
import AdminPanel from "./AdminPanel.vue"
import TeacherPanel from "./TeacherPanel.vue"
import JuryPanel from "./JuryPanel.vue"
import UserPanel from "./UserPanel.vue"
import StudentPanel from "./StudentPanel.vue"
import { AuthService } from "@signature/webfrontauth"
import { getGroupUserAccessPanel } from "../api/groupApi"
import { getAuthService } from "../modules/authService"
import * as SignalR from "@aspnet/signalr"
import { SignalRGestion } from "../modules/classes/SignalR"
import {GetAllProject} from '../api/projectApi'

@Component({
  components: {
    UserInfoBox,
    AdminPanel,
    TeacherPanel,
    UserPanel,
    JuryPanel,
    StudentPanel
  },
})

export default class SideBar extends Vue {
    isCollapsed: boolean = false
    whatTimed: string[] = []
    ZoneId: number = 4
    authService: AuthService = getAuthService()
    private co!: SignalR.HubConnection
    private signalr: SignalRGestion = new SignalRGestion()

    @Watch("authService.authenticationInfo.level", { immediate: true, deep: true })
    async onLevelChange() {
        await this.getAuthorizedAccess()

    }

    test(){
      console.log(window.innerWidth)
    }
    async mounted() {
      window.addEventListener("resize", function () {
       if(window.innerWidth <= 900) {
          if(document.getElementsByClassName("collapsedMenu")[0].style.display == "none") {
            document.getElementsByClassName("nav")[0].style.marginBottom="40%"
            document.getElementsByClassName("collapsedMenu")[0].style.display="block"
            document.getElementsByClassName("liste")[0].style.backgroundColor="#2d3e4f"
          }
          else {
            document.getElementsByClassName("nav")[0].style.marginBottom="-47%"
            document.getElementsByClassName("collapsedMenu")[0].style.display="none"
          }
        }
        else {
          document.getElementsByClassName("collapsedMenu")[0].style.display="block"
          document.getElementsByClassName("nav")[0].style.marginBottom="0%"
        }
      });
        this.co = this.$store.state.connectionStaffMember
        this.changeCollapse()
        if ( this.co.state == undefined ) {
            await this.signalr.connect()
        }
    }

    handleOpen(key: number, keyPath: number) {
        console.log(key, keyPath)
    }

    handleClose(key: number, keyPath: number) {
        console.log(key, keyPath)
    }

    changeCollapse() {
        if(window.innerWidth <= 900) {
          this.isCollapsed = !this.isCollapsed
          if(!this.isCollapsed) {
            document.getElementsByClassName("nav")[0].style.marginBottom="40%"
            document.getElementsByClassName("collapsedMenu")[0].style.display="block"
            document.getElementsByClassName("liste")[0].style.backgroundColor="#2d3e4f"
          }
          else {
            document.getElementsByClassName("nav")[0].style.marginBottom="-47%"
            document.getElementsByClassName("collapsedMenu")[0].style.display="none"
          }
        }
        else {
          this.isCollapsed = true
          document.getElementsByClassName("collapsedMenu")[0].style.display="block"
          document.getElementsByClassName("nav")[0].style.marginBottom="0%"
        }
    }

    redirect(destination: string) {
        this.$router.push(destination)
    }

    async logout() {
        await this.authService.logout(true)
        this.$router.push("/")
    }

    async getAuthorizedAccess() {
        this.whatTimed = await getGroupUserAccessPanel(this.ZoneId)
        console.log("ok" + this.whatTimed)
        this.$store.state.currentUserType = this.whatTimed
        console.log("store :")
        console.log(this.$store.state)
    }
}
</script>

<style>
.menu-toggle {
  display: none;
}
ul.sidenav {
background: linear-gradient(180deg, rgba(17,46,88,1) 0%, rgba(198,198,198,1) 100%); position: fixed;
  height: 100vh;
  width: 15%;  
  margin-top: 0%;
  padding: 0;
}
#intech {
  background-color: #4CAF50;
  color: white;
  border: none;
  width: 100%;
}
.in-menu {
    float: none;
    text-align: center;
}
 
ul.sidenav li a.active {
  color: white;
  line-height: 25px;
  font-size: 24px;
  font-weight: bold;
}

ul.sidenav li a {
  display: block;
  color: white;
  padding: 18px 16px;
  text-decoration: none;
  cursor: pointer;
}
.btn-group .buttons {
  border: none;
  padding: 10px 25px;
  font-size: 16px;
  cursor: pointer;
  background-color: inherit
}
.btn-group .buttons:hover {
  background-color: inherit
}
ul.sidenav li a:hover:not(.active) {
  background-color:inherit
}
  .test {
  display: block;
  color: white;
  padding: 18px 16px;
  text-decoration: none;
  cursor: pointer;
  border: none;
  }
@media screen and (max-width: 900px) {
  .nav {
    height: 100vh;        
    margin-bottom: 28%;
  }
  .liste {
    background-color: "#2d3e4f;"
  }
  a.dropbtn {
    color:white;
  }
  ul.sidenav {
    width: 100%;
    height: 15vh;
    position: relative;
  }
  #mySidenav {
    position: absolute;
    top: 50px;
    left: -100%;
    width: 100%;
    height: calc(100vh - 50px);
    background:inherit;
    transition: 0.5s;
  }
  .menu-toggle {
    display: block;
    float: right;
    position: relative;
    margin-right: 2vw;
    margin-top: 5vh;
  }
  ul.sidenav li a {
    list-style: none;
    color:white;
  }
  ul.sidenav li a.active {
    height: 15vh;
    float: left;
    color: white;
  }
  .btn-group .buttons {
    background-color: #2d3e4f;
    display: inline-block;
    border: none;
    cursor: pointer;
    width: 100%;
    padding: 15px;
  }
  div.okok {
  text-align: center;
  background-color: #2d3e4f;
  border: none;
  width: 100%;
  padding: 15px;
  height: 100%;
  color: white;
  cursor: pointer;
  box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
  font-size: 100%;
  list-style: none;
  }
  .test {
  text-align: center;
  background-color: #2d3e4f;
  border: none;
  width: 100%;
  padding: 15px;
  height: 100%;
  color: white;
  cursor: pointer;
  box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
  font-size: 100%;
  }
}
</style>
