import { postAsync } from "../helpers/apiHelper"
import { User } from "../modules/classes/User"
import { BddInfo } from "../modules/classes/BddInfo"

const endpoint = process.env.VUE_APP_BACKEND + "/api/User"

export async function getUserList(data: BddInfo): Promise<User[]> {
    const response = await postAsync(`${endpoint}/getUserList`, data)

    return response.data
}
