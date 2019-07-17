<template>
    <div>
        <div class="okok">
            <li>
                <a class="dropbtn" @click="redirect('/createPeriod')">Périodes</a>
            </li>
        </div>
        <div class="okok">
            <li class="dropdown">
                <a class="dropbtn">Listes</a>
                <div class="dropdown-content">
                    <button @click="redirect('/student')">Liste des étudiants</button>
                    <button  @click="redirect('/staffMember')">Liste des professeurs</button>
                </div>        
            </li>
        </div>
        <div class="okok">
            <li class="dropdown">
                <a class="dropbtn">Forum</a>
                <div class="dropdown-content">
                    <button @click="redirect('/addJury')">Gestion forum</button>
                    <button @click="redirect('/plan')">Plan</button>
                    <button @click="redirect('/ForumResult')">Résultats Forum PI</button>
                </div>        
            </li>
        </div>
        <div class="okok">
            <li>
                <a class="dropbtn" @click="redirect(`/Events`)">Évènements de l'école !</a>
            </li>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator"
import * as SignalR from "@aspnet/signalr"
import { SignalRGestion } from "../modules/classes/SignalR"

@Component
export default class AdminPanel extends Vue {
@Prop()
isCollapse!: boolean
private connection!: SignalR.HubConnection
private signalr: SignalRGestion = new SignalRGestion()

async created() {
    await this.signalr.connect()

    // this.$store.state.connectionStaffMember = this.connection
}

 redirect(destination: string) {
        this.$router.push(destination)
    }
}
</script>

<style lang="scss">

.dropbtn {
  display: inline-block;
  color: white;
  text-align: center;
  text-decoration: none;
  font-size: 19px;
 }

.dropdown {
  display: inline-block;
  width: 100%;
}

.dropdown-content {
  display: none;
  position: absolute;
  background-color: #2d3e4f;
  box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
  z-index: 1;
  font-size: 19px;
}

.dropdown-content button {
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

.dropdown-content button:hover {
    background-color: #2d4f4f;
}

.dropdown:hover .dropdown-content {
  display: block;
  width: 100%;
  position: relative;
}

.okok {
    display: block  
}
</style>
