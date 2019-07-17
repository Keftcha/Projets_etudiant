<template>
    <div>
        <center>
        <div style="width: 1000px; text-align: justify">

        Les fiches de présentation de vos projets PI et PFH doivent être disponibles en public sur Google Drive.<br>
        Leur niveau d'accessibilité doit être public. Cela signifie que tout utilisateur d'internet peut y avoir accès sans inscription à la plateforme avec le lien que vous nous donnerez.<br> 
        Il en est de même pour toutes les ressources que vous nous joindrez sous forme de liens tels que votre logo et votre image de fond en PFH. Le lien doit mener directement à votre image, non à un site sur lequel votre image est.<br>
        Votre logo doit avoir une taille de 800 px par 800 px.<br>
        <br>
        Ces fiches doivent être nommées ficheprojet.toml et placées à la racine de votre répertoire en ligne.<br>
        Pour plus d'informations sur les fichiers en toml <a target="_blank" href="https://github.com/toml-lang/toml">cliquez ici</a>.<br>
        Les ressources jointes, hors images, doivent être au format PDF.<br>
        <br>
        Les fichiers d'exemples se trouvent dans les répertoires ./projet_PFH et ./projet_PI. Ces derniers doivent contenir toutes les tables et clés présentes dans les templates. Les tables ou clés en plus seront ignorées.<br>
        Il y a une exception, c'est la table othersDocuments qui est facultative. Dans le cas où vous n'avez pas d'autres ressources en plus à joindre, veillez à effacer ces lignes.<br>
        <br>
        Pour les valeurs possibles à chaque clé, référez-vous aux fichiers d'exemples.<br>
        Les fichiers d'exemples se trouvent dans les répertoires ./projet_PFH et ./projet_PI.<br>
        <br>
        <a href="documentations.zip" download>
            <button type="button" class="button">
                <b>Télécharger les exemples</b>
            </button>
        </a>

        </div>
        </center>

        <br>
        <br>
        <br>

        <div v-if="msg">
            <div v-if="isSucces">
                <font color="green">
                    <b>
                        {{msg}}
                    </b>
                </font>
            </div>
            <div v-else>
                <font color="red">
                    <b>
                        {{msg}}
                    </b>
                </font>
            </div>
        </div>
        <div v-else>
            <br>
        </div>

        <br>

        <div v-if="this.loading"
            v-loading="loading"
            element-loading-text="Loading..."
            element-loading-spinner="el-icon-loading"
            element-loading-background="rgba(0, 0, 0, 0.8)"
        >
        </div>
        <div v-else>
            <el-form :inline="true">
                <b>

                    <el-radio-group v-model="projectType">
                        <el-radio :label="0">
                            Projet Informatique
                        </el-radio>
                        <el-radio :label="1">
                            Projet de Formation Humaine
                        </el-radio>
                    </el-radio-group>

                    <br>
                    <br>

                    <el-form-item label="Lien du fichier toml: ">
                        <el-input
                            placeholder="Lien de partage du fichier toml"
                            v-model="projectLink"
                            clearable
                            style="width: 100%"
                        >
                        </el-input>
                    </el-form-item>
                    <el-form-item>
                        <button type="button" class="button" @click="Submit()">Soumettre le projet</button>
                    </el-form-item>
                </b>
            </el-form>
        </div>
    </div>
</template>

<script lang="ts">
import {Component, Vue} from "vue-property-decorator"
import {SubmitProject} from "../api/submitProjectApi"
import {getAuthService} from "../modules/authService"
import {AuthService} from "@signature/webfrontauth"

@Component
export default class Submit extends Vue {
    private projectLink: string = ""
    private msg: string = ""
    private isSucces: boolean = false
    private returnMsg: string = ""
    private loading: boolean = false
    private projectType: number = 0
    private authService: AuthService = getAuthService()

    async Submit() {
        this.loading = true

        // initialisation
        this.isSucces = false, this.msg = ""

        const futur: Promise<[boolean, string]> = SubmitProject(
            this.projectLink,
            this.projectType,
            this.authService.authenticationInfo.user.userId
        )

        futur.catch(() => {
            this.isSucces = false
            this.msg = "Failed to join the server"
        })

        futur.then((result) => {
            this.isSucces = result[0], this.msg = result[1]
        })

        futur.finally(() => this.loading = false)
    }
}
</script>


<style>
input {
    text-align: center;
}
</style>