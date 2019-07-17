 //     ___     __    _______               __
//    |    \  |  |  |   ____|             |  |
//    |  |  \ |  |  |  |__                |  |
//    |  |\  \|  |  |   __|               |__|
//    |  | \  |  |  |  |____               __
//    |__|  \ ___|  |_______| pas toucher |__|
//

<template>
  <div>
    <el-tooltip
      v-for="scheme in schemes"
      :key="scheme.name"
      :content="parameter.tooltipTextGenerator(scheme)"
      placement="right"
    >
      <img
        :alt="scheme.name"
        :src="getSchemeImage(scheme.name)"
        v-on:click="parameter.authService.startPopupLogin(scheme.name)"
        class="button"
      >
    </el-tooltip>
    <el-tooltip v-if="parameterReduced != null" content="Plus...">
      <img alt="Plus d'options de connexion." :src="getSchemeImage('moreOptions')">
    </el-tooltip>
  </div>
</template>

 <script lang="ts">
/*import { Component, Prop, Watch, Vue } from "vue-property-decorator"

import {
  IUserSchemeInfo,
  AuthLevel,
  AuthService
} from "@signature/webfrontauth"

export class SchemeDisplayParameter {
  public authService: AuthService
  public schemesURL: string
  public tooltipTextGenerator: (arg: IUserSchemeInfo) => string
  constructor(
    authService: AuthService,
    schemesURL: string,
    tooltipTextGenerator: (arg: IUserSchemeInfo) => string
  ) {
    if (authService == undefined || schemesURL == undefined) {
      throw new Error("Undefined or null argument")
    }
    this.authService = authService
    this.schemesURL = schemesURL
    this.tooltipTextGenerator = tooltipTextGenerator
  }
}

export class SchemeDisplayReducedModeParameter extends SchemeDisplayParameter {
  public numberOfElement: number
  public menuCallback: () => void
  constructor(
    authService: AuthService,
    schemesURL: string,
    numberOfElement: number,
    tooltipTextGenerator: (arg: IUserSchemeInfo) => string,
    menuCallback: () => void
  ) {
    super(authService, schemesURL, tooltipTextGenerator)
    if (numberOfElement == undefined || numberOfElement < 1) {
      throw new Error(
        "Invalid number of element, there should be at least one element to display"
      )
    }
    if (menuCallback == undefined) {
      throw new Error("Invalid menuCallback, should not be undefined or null")
    }
    this.numberOfElement = numberOfElement
    this.menuCallback = menuCallback
  }
}

@Component
export default class SchemeDisplay extends Vue {
  private static loadSchemes(obj: SchemeDisplay) {
    obj.schemes = SchemeDisplay.schemesSorted(obj)
    if (obj.parameterReduced != undefined) {
      obj.schemes = obj.schemes.slice(
        0,
        obj.parameterReduced.numberOfElement - 1
      )
    }
  }
  private static availableSchemeCasted(obj: SchemeDisplay): IUserSchemeInfo[] {
    // Casting available Scheme to add a date and sort it with the user schemes
    return obj.parameter.authService.availableSchemes.map(scheme => {
      return {
        name: scheme,
        lastUsed: new Date(0) // Using Date(0) for an unexisting date.
      }
    })
  }
  private static schemesSorted(obj: SchemeDisplay) {
    // Sort schemes by most recently used
    const authInfo = obj.parameter.authService.authenticationInfo
    if (authInfo.level > 0) {
      const userScheme = authInfo.unsafeUser.schemes
      const availableSchemes = SchemeDisplay.availableSchemeCasted(obj)
      for (const iterator of availableSchemes) {
        if (!obj.schemeInfoArrayContain(iterator, userScheme)) {
          // Add schemes which was not present in the userScheme
          userScheme.push(iterator)
        }
      }
      return userScheme
    }
    return SchemeDisplay.availableSchemeCasted(obj)
  }

  @Prop({ required: true })
  private parameter!: SchemeDisplayParameter
  private parameterReduced: SchemeDisplayReducedModeParameter | null = null
  private reducedMode: boolean = false
  private schemes: IUserSchemeInfo[] = []
  constructor() {
    super()
    if (
      this.parameter == undefined ||
      this.parameter.authService == undefined
    ) {
      throw new Error("Invalid paramater.")
    }
    if (this.parameter instanceof SchemeDisplayReducedModeParameter) {
      const castedParam: SchemeDisplayReducedModeParameter = this
        .parameter as SchemeDisplayReducedModeParameter
      if (castedParam.numberOfElement < 1) {
        throw new Error("No number given in parameter")
      }
      console.log("ah")
      this.parameterReduced = castedParam
    }
    SchemeDisplay.loadSchemes(this)
  }

  @Watch("parameter.authService.availableSchemes", { deep: true })
  private reloadSchemes() {
    SchemeDisplay.loadSchemes(this)
  }
  private schemeInfoArrayContain(
    scheme: IUserSchemeInfo,
    schemeArray: IUserSchemeInfo[]
  ): boolean {
    // return true if an array of scheme contain a given scheme
    for (const iterator of schemeArray) {
      if (iterator.name === scheme.name) return true
    }
    return false
  }
  private getSchemeImage(schemeName: string) {
    try {
      return require("@/assets/" + schemeName + ".png")
    } catch (e) {
      return require("@/assets/chapin.png")
    }
  }
}
</script>
<style scoped>
.button {
  cursor: pointer;
}
</style> */ */