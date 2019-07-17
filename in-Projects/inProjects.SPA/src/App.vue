<template>
    <div id="app">
      <SideBar></SideBar>
      <div class="content">
        <main class="vue">
            <router-view></router-view>
        </main>
      </div>

    </div>
</template>

<script>
import { Component, Vue } from "vue-property-decorator"
import SideBar from "@/components/SideBar.vue"

@Component({
  components: {
    SideBar
  },
})
export default class App extends Vue {}
</script>

<style lang="scss">
@import "./styles/global.scss";
#app {
  font-family: 'Questrial', sans-serif;
  font-size: 17px;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  background: linear-gradient(135deg, rgba(251,251,254,1) 0%, rgba(251,251,251,1) 43%, rgba(251,251,255,1) 72%, rgba(246,246,249,1) 85%, rgba(0,51,215,1) 100%) no-repeat center center;
  -webkit-background-size: cover;
  -moz-background-size: cover;
  -o-background-size: cover;
  background-size: cover;
}


.content{
  margin-left: 17%;
}
@media screen and (max-width: 900px) {
  .content{
    margin-left: 4%;
    margin-top: 2%;
    margin-right: 2%;
  }
}
</style>