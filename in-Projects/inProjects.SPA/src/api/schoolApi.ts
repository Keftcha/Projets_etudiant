import { postAsync, getAsync } from "../helpers/apiHelper"
import axios from "axios"
import { School } from "@/modules/classes/School";

const endpoint = process.env.VUE_APP_BACKEND + "/api/School"

export async function getSchools() : Promise<School[]>  {
    const resp = await getAsync(endpoint)
    return resp.data
}

export async function getIdSchoolOfUser() : Promise<number>  {
    const resp = await getAsync(`${endpoint}/getIdSchoolOfUser`)
    return resp.data
}