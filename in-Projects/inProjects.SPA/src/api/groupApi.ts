import { getAsync } from "../helpers/apiHelper"
import { GroupPeriod } from "@/modules/classes/Periode/GroupPeriod"

const endpoint = process.env.VUE_APP_BACKEND + "/api/group"

export async function getTemplateGroupsAsync(idZone: number): Promise<GroupPeriod[]> {
    const response = await getAsync(`${endpoint}/getAllGroupTemplate?idZone=${idZone}`)
    return response.data
}

export async function getGroupUserAccessPanel(idZone: number): Promise<string[]> {
    const response = await getAsync(`${endpoint}/getGroupUserAccessPanel?idZone=${idZone}`)
    return response.data
}