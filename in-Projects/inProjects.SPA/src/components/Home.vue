<template>
<div><br>
    <div class="masonry-layout">
        <div class="mansonry-layout-panel masonry-layout-cluster masonry-layou-cluster--horizontal">
              <div class="masonry-layout-cluster__segment masonry-layout-cluster__segment--column">
                <div class="mansonry-layout-panel__content">
                    <img src="../assets/intechLogo.png" width="300em;"  class="masonry-layout-panel__content--img">
                </div>
              </div>
        </div>
        <div class="masonry-layout-panel">
            <div class="masonry-layout-panel__content">
                <Countdown :schoolName="schoolName"></Countdown>
            </div>
        </div>
        <div class="masonry-layout-panel">
            <div class="masonry-layout-panel__content">
                <h3>IN'TECH, c'est quoi ? </h3>
                <p style="text-align: justify;">
                    IN’TECH est une École Supérieure d’Ingénierie Informatique créée en 2002. Association à but non lucratif (loi 1901), IN'TECH est rattachée au Groupe d'enseignement supérieur et de recherche ESIEA.
                    Présente sur six campus : Paris, Agen, Dax, Montauban, Pamiers, Nîmes et Béziers, l’école s’adresse aux passionnés d’informatique qui, grâce à sa pédagogie par projets et son contact permanent avec le monde de l’entreprise, s’installe comme un acteur majeur de l’économie du territoire. Elle délivre en 5 ans une certification reconnue par l’État : un titre d’Expert en Ingénierie du Logiciel et un titre d’Expert en Systèmes et Réseaux.
                    IN’TECH ouvre sa formation à des profils très variés et fonde sa pédagogie innovante sur l’accompagnement de chaque élève, ainsi que leur professionnalisation dès leur rentrée dans l’école.
                    IN’TECH est un établissement d’enseignement supérieur technique privé en informatique.
                </p>
            </div>
        </div>
        <div class="mansonry-layout-panel">
            <div class="mansonry-layout-panel__content">
                <img src="../assets/carte-campus.png"  class="masonry-layout-panel__content--img">
            </div>
        </div>
        <div class="masonry-layout-panel">
            <div class="masonry-layout-panel__content">
                <img src="../assets/people.png">
                <br>
                <h3>Accompagner chaque étudiant</h3>
                Avec un taux d’encadrement de 10 élèves par prof, nous valorisons la richesse de tous les profils que nous accueillons.
            </div>
        </div>  
        <div class="masonry-layout-panel">
            <div class="masonry-layout-panel__content">
                <img src="../assets/code.png">
                <br>
                <h3>Enseigner par la pratique</h3>
                60% du semestre des élèves est consacré à la réalisation de projets en informatique et formation humaine
            </div>
        </div>        
        <div class="masonry-layout-panel">
            <div class="masonry-layout-panel__content">
                <img src="../assets/hands.png">
                <br>
                <h3>Maximiser l'employabilité</h3>
                100% de nos étudiants sont embauchés en CDI à la fin de leur formation, avec un salaire moyen de 37 600€
            </div>
        </div>        
        <div class="masonry-layout-panel">
            <div class="masonry-layout-panel__content">
                <img src="../assets/human.png">
                <br>
                <h3>Développer l'humain</h3>
                80 heures de cours par semestre sont dédiées aux compétences transversales des élèves à travers la formation humaine.
            </div>
        </div>
        <div class="masonry-layout-panel">
            <div class="masonry-layout-panel__content">
                <img src="../assets/contactCard.png" height="75" width="85">
                <h3>Contact</h3>
                Campus IN'TECH Paris
                <br>
                Par mail: contact@intechinfo.fr
                <br>
                <a href="https://www.intechinfo.fr/ecole-informatique/contacter-intech/" >Via le site</a>
            </div>
        </div>        
    </div>
<!--             <div class="masonry-layout-panel masonry-layout-cluster__segment">
                <div class="masonry-layout-panel__content"> -->
                    <h3>Un projet réalisé par: </h3>
                    <br>
                    <img src="../assets/julie.png"  class="masonry-layout-panel__content--img">
                    &nbsp
                    <img src="../assets/Alban.png"  class="masonry-layout-panel__content--img">
                    &nbsp
                    <img src="../assets/melvin.png"  class="masonry-layout-panel__content--img">
                    &nbsp
                    <img src="../assets/arthur.png" class="masonry-layout-panel__content--img">
                    &nbsp
                    <img src="../assets/dan.png"  class="masonry-layout-panel__content--img">
<!--                 </div>
            </div> -->
<br><br></div>
</template>

<script lang="ts">
import { Component, Vue} from "vue-property-decorator"
import Countdown from "./Countdown.vue"

@Component({
    components:{
        Countdown

    }
})
export default class HomePage extends Vue {
    private schoolName = "IN'TECH"
}
</script>

<style>
    .masonry-layout {
        column-count: 3;
        column-gap: 0;
    }
    .masonry-layout-panel {
        break-inside: avoid;
        padding: 5px;
    }
    .masonry-layout-panel__content {
        padding: 10px;
        border-radius: 10px;
        border-style: solid;
        overflow: hidden;
        border-color: #167BEB;
    }
    .masonry-layout-cluster--horizontal {
        flex-direction: column;
    }
    .masonry-layout-cluster__segment--column {
        flex-direction: column;
    }

    .masonry-layout-panel__content--img {
        max-width: 100%;
        padding: 0;
    }

    .masonry-layout-cluster__segment--row {
        flex-direction: row;
    }
    .masonry-layout-cluster__segment {
        display: flex;
        flex: 1 1 auto;
    }
    .masonry-layout-cluster {
        display: flex;
        padding: 0;
    }

    @media (min-width: 768px) {
        .masonry-layout {
        column-count: 2;
        }
    }
    @media (min-width: 1200px) {
        .masonry-layout {
        column-count: 3;
        }
    }


</style>
