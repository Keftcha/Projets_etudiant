import { postAsync, getAsync } from "../helpers/apiHelper"
import { PeriodCreate } from "@/modules/classes/Periode/PeriodCreate"
import { Period } from "@/modules/classes/Periode/Period"

const endpoint = process.env.VUE_APP_BACKEND + "/api/period"

export async function createPeriodAsync(data: PeriodCreate): Promise<any> {
    const response = await postAsync(`${endpoint}/createPeriod`, data)
    return response
}

export async function verifyActualPeriod(): Promise<boolean> {
    const response = await getAsync(`${endpoint}/verifyActualPeriod`)
    return response.data
}

export async function getAllPeriod(idZone: number): Promise<Period[]> {
    const response = await getAsync(`${endpoint}/getAllPeriod?idZone=${idZone}`)
    return response.data
}

export async function changeDateOfPeriod(idPeriod: number, begDate: Date, endDate: Date): Promise<any> {
    const response = await postAsync(`${endpoint}/changeDateOfPeriod`, {idPeriod, begDate, endDate})
    return response
}
