import { postAsync, getAsync } from "../helpers/apiHelper"
import { User } from "../modules/classes/User"
import { UserInfo } from "../modules/classes/UserInfo"
import { PasswordChangeInfo } from "../modules/classes/PasswordChangeInfo"
import { InfosAccount } from "@/modules/classes/InfosAccount"
import { ProjectFav } from "@/modules/classes/ProjectFav"
import { OwnProject } from "@/modules/classes/OwnProject"

const endpoint = process.env.VUE_APP_BACKEND + "/api/account"

export async function register(data: User): Promise<string> {
    const response = await postAsync(`${endpoint}/register`, data)

    return response.data
}

export async function getUserName(data: User): Promise<UserInfo> {
    const response = await postAsync(`${endpoint}/getUserName`, data)
    const result: UserInfo = new UserInfo()
    result.userName = response.data.userName

    return result
}

export async function changePassword(data: PasswordChangeInfo): Promise<any> {
    const response = await postAsync(`${endpoint}/changePassword`, data)
    return response
}


export async function getAccountInfos(): Promise<InfosAccount> {
    const response = await getAsync(`${endpoint}/getInfosAccount`)
    console.log(response)
    return new InfosAccount(
        new User(
            response.data.userData.firstName,
            response.data.userData.lastName,
            response.data.userData.email,
            "",    // the password
            response.data.group,
            response.data.userData.emailSecondary),
        response.data.group,
        response.data.isActual
    )
}

export async function getProjectsFav(): Promise<ProjectFav[]> {
    const response = await getAsync(`${endpoint}/getProjectsFav`)
    return response.data
}

export async function getOwnProjects(): Promise<OwnProject[]> {
    const response = await getAsync(`${endpoint}/getProjects`)
    return response.data
}

export async function changeAccountInfos(data: InfosAccount): Promise<any> {
    const response = await postAsync(`${endpoint}/changeInfosAccount`, data)
    return response
}