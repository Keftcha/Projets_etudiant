import { postAsync, getAsync } from "../helpers/apiHelper"
import { ProjectDetails } from "@/modules/classes/ProjectDetails"
import { Project } from "@/modules/classes/Project"
import { User } from "@/modules/classes/User"

const endpoint = process.env.VUE_APP_BACKEND + "/api/Project"

export async function SubmitProject(link: string, projectType: number, userId: number): Promise<[boolean, string]> {
    const resp = await postAsync(`${endpoint}/submitProject`, {link: link, projectType: projectType, userId: userId})

    return [resp.data.item1, resp.data.item2]
}

export async function getInfosProject(idProject: number): Promise<ProjectDetails> {
    const response = await getAsync(`${endpoint}/getInfosProject?idProject=${idProject}`)
    console.log(response)
    const projectDetails: ProjectDetails = new ProjectDetails()

    const project = new Project(response.data.project.name, response.data.project.logo, response.data.project.slogan,
                                response.data.project.pitch, response.data.project.leaderId, response.data.project.semester
                                , response.data.project.technologies,response.data.project.url)

    projectDetails.project = project

    const students: User[] = []
    for (let index = 0; index < response.data.students.length; index += 1 ) {
        const student = new User(response.data.students[index].firstName, response.data.students[index].lastName)
        students.push(student)
    }

    projectDetails.students = students
    return projectDetails
}

export async function verifyProjectFav(idProject: number): Promise<boolean> {
    const resp = await getAsync(`${endpoint}/verifyProjectFav?idProject=${idProject}`)

    return resp.data
}

export async function favProject(idProject: number): Promise<any> {
    const resp = await getAsync(`${endpoint}/favProject?idProject=${idProject}`)

    return resp.data
}
