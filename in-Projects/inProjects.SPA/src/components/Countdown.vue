<template>
  <div>
    <div v-if="forumOn == false">
	      <h2>Prochain forum PI dans : <br>{{days}}d {{hours}}h {{minutes}}m {{seconds}}s</h2>
    </div>
    <div v-else>
      <h2>{{message}}</h2>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue,Prop,Watch } from "vue-property-decorator"
import { GetEventSchoolByName } from '../api/eventApi';
import { Event } from '../modules/classes/EventSchool';

@Component
export default class Countdown extends Vue {
    @Prop(String) schoolName!: "IN'TECH"
    nextForum: Event = new Event()
    days: number = 0
    hours: number = 0
    minutes: number = 0
    seconds: number = 0
    vm = this
    private forumOn = false
    private message = ""
    private test!: NodeJS.Timeout 
    private timeToNextForum : number = 0

  @Watch("schoolName", { immediate: true, deep: true })
  async onChange() {
    console.log(this.schoolName)
     await this.setTimer()
  }

    async setTimer(){
        if(this.test != undefined) clearInterval(this.test)

        try{
              this.nextForum = await GetEventSchoolByName("ForumPI",this.schoolName)
            
              const vm = this
              this.forumOn = false
              let dateNow = new Date()

              if(dateNow >= this.nextForum.begDate && dateNow <= this.nextForum.endDate){
                this.forumOn = true;
                this.message = "Le forum est ouvert ! Amusez-vous bien ! ;)"
              }else if(dateNow <= this.nextForum.begDate){
                  vm.test = setInterval(() => {
                      vm.timeToNextForum = vm.nextForum.begDate.getTime() - new Date().getTime()

                      vm.days = Math.floor(vm.timeToNextForum / (1000 * 60 * 60 * 24))
                      vm.hours = Math.floor((vm.timeToNextForum % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60))
                      vm.minutes = Math.floor((vm.timeToNextForum % (1000 * 60 * 60)) / (1000 * 60))
                      vm.seconds = Math.floor((vm.timeToNextForum % (1000 * 60)) / 1000)
                  }, 1000)
                  console.log(vm.test)
              }else{
                this.forumOn = true
                this.message = "Le forum est terminé ! Revenez le semestre prochain pour de nouveaux projets fabuleux !"
              }
          }catch(e){
            this.forumOn = true
            this.message = "Pas de date connue pour le prochain Forum Pi"

          }

    }
}

</script>