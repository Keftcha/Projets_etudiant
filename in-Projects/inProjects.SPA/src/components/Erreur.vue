<template>
     <div class="alert" v-if="error.length > 0">
        <b>Erreurs  : </b>

       <ul>
             <li v-for="e of error">{{e}}</li>
      </ul>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator"

@Component

export default class Erreur extends Vue {
     @Prop()
      error: string[] = []
}
</script>

<style>
.alert {
  padding: 20px;
  background-color:rgba(236, 100, 75, 1); /* Red */
  color: white;
  margin-bottom: 15px;
  height: 30%;
  width: 40%;
  margin-left : 30%;
}

</style>
