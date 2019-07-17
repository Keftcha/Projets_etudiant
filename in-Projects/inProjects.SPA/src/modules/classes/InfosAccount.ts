import { User } from "./User"

class InfosAccount {
    public userData: User
    public group: string
    public isActual: boolean

    constructor(userData: User = new User(), group: string = "", isActual: boolean = false) {
        this.userData = userData
        this.group = group
        this.isActual = isActual
    }

    public clone() {
        const e: InfosAccount = new InfosAccount(this.userData.clone(), this.group, this.isActual)
        console.log(e)
        return e
    }
}

export {InfosAccount}