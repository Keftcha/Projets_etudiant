import { postAsync, getAsync, gettAsyncConfigResponseType } from "../helpers/apiHelper"
import { Plan } from "../modules/classes/Plan"
import { Project } from "@/modules/classes/Project"
import { Chacheli } from "../modules/classes/Chacheli"
import { ProjectForumResult } from '@/modules/classes/ProjectForumResult';

const endpoint = process.env.VUE_APP_BACKEND + "/api/forum"

export async function getPlan() {
    const response = await getAsync(`${endpoint}/getPlan`)

    const plan = new Plan()
    plan.height = response.data.height
    plan.width = response.data.width
    plan.classRooms = response.data.classRooms

    return plan
}

export async function getProjects(): Promise<Project[]> {
    let projects: Project[] = []
    const response = await getAsync(`${endpoint}/getProjects`)
    projects = response.data

    return projects
}


export async function getAllGradeProjects(): Promise<ProjectForumResult[]> {
    let projects: ProjectForumResult[] = []
    const response = await getAsync(`${endpoint}/getAllGradeProjects`)
    projects = response.data
    console.log("test")

    return projects
}


export async function getAllPublicNote(): Promise<ProjectForumResult[]> {
    let projects: ProjectForumResult[] = []
    const response = await getAsync(`${endpoint}/getAllPublicNote`)
    projects = response.data
    return projects
}

export async function downloadExcel(): Promise<any> {
    const response = await gettAsyncConfigResponseType(`${endpoint}/DownloadExcel`)
    console.log(response)
    return response.data
}

export async function savePlan(plan: Chacheli[]): Promise<any> {
    const response = await postAsync(`${endpoint}/savePlan`, plan)
    return response
}