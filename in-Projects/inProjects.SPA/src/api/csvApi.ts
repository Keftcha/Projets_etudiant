import { postAsync } from "../helpers/apiHelper"
import axios from "axios"

const endpoint = process.env.VUE_APP_BACKEND + "/api/User"

export async function sendCsv(file: any) {
    axios.defaults.headers.post["Content-Type"] = "multipart/form-data"
    return await postAsync(endpoint, file)
}