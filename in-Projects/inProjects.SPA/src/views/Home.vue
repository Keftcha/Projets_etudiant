<template>
  <div class="home">
<!--     <img src="../assets/intechLogo.png" width="300em;"><br>
    
    <Countdown></Countdown>
    <b><u>NOUS PENDANT CE PROJET :</u></b><br><br>
    <img src="/nous.gif"> -->
  <HomePage></HomePage>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator"
import Countdown from "@/components/Countdown.vue"
import HomePage from "@/components/Home.vue"

@Component({
    components: {
        Countdown,
        HomePage
    }
})
export default class Home extends Vue {

}
</script>

<style>

</style>
