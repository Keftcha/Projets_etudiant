import {getAsync, postAsync} from "../helpers/apiHelper"
import {Project} from "@/modules/classes/Project"
import {ProjectSheet, ProjectPiSheet, ProjectPfhSheet} from "../modules/classes/ProjectSheet"
import {TypeTimedUser} from "@/modules/classes/TimedUserEnum"
const endpoint = process.env.VUE_APP_BACKEND + "/api/Project"

export async function GetAllProject(): Promise<Project[]> {
    const resp = await getAsync(`${endpoint}/getAllProjects`)
    return resp.data
}

export async function GetAllSheet(school: number, type: string, semester: number): Promise<ProjectSheet[] | ProjectPiSheet[] | ProjectPfhSheet[]> {
    const rep = await getAsync(`${endpoint}/getAllSheet?schoolId=${school}&projectType=${type}&semester=${semester}`)

    let projects
    if (type === "I") {
        projects = new Array<ProjectPiSheet>()

        for (const project of rep.data) {
            projects.push(
                new ProjectPiSheet(
                    project.place,
                    project.name,
                    project.semester,
                    project.sector,
                    project.logo,
                    project.slogan,
                    project.pitch,
                    [
                        project.team.item1,
                        project.team.item2
                    ],
                    project.technos
                )
            )
        }
    } else if (type === "H") {
        projects = new Array<ProjectPfhSheet>()

        for (const project of rep.data) {
            projects.push(
                new ProjectPfhSheet(
                    project.name,
                    project.semester,
                    project.sector,
                    project.logo,
                    project.slogan,
                    project.pitch,
                    [
                        project.team.item1,
                        project.team.item2
                    ],
                    project.background
                )
            )
        }
    } else {
        projects = new Array<ProjectSheet>()

        for (const project of rep.data) {
            if(project.type =="I"){
                projects.push(
                    new ProjectPiSheet(
                        project.place,
                        project.name,
                        project.semester,
                        project.sector,
                        project.logo,
                        project.slogan,
                        project.pitch,
                        [
                            project.team.item1,
                            project.team.item2
                        ],
                        project.technos
                    )
                )
            }else{
                projects.push(
                    new ProjectPfhSheet(
                        project.name,
                        project.semester,
                        project.sector,
                        project.logo,
                        project.slogan,
                        project.pitch,
                        [
                            project.team.item1,
                            project.team.item2
                        ],
                        project.background
                    )
                )
            }
           
        }
    }

    return projects
}

export async function GetAllTypeProjectsOfSchool(idSchool: number, type:string): Promise<Project[]> {
    const resp = await getAsync(`${endpoint}/getAllTypeProjectsOfASchool?idSchool=${idSchool}&&type=${type}`)
    return resp.data
}

export async function GetSelectorGrade(): Promise<number[]> {
    const resp = await getAsync(`${endpoint}/getSelectorGrade`)
    return resp.data
}

export async function GetEvaluateProject(idSchool: number): Promise<Project[]> {
    const resp = await getAsync(`${endpoint}/getProjectEval?idSchool=${idSchool}`)
    return resp.data
}

export async function noteProject(idProject: number,idJury : number, newGrade: number,idSchool: number, timedUser: TypeTimedUser): Promise<any> {
    const resp = await postAsync(`${endpoint}/noteProject?`,{ProjectId: idProject,JuryId: idJury,Grade: newGrade,SchoolId: idSchool,User:timedUser})
    return resp.data
}

export async function noteProjectUser(idProject: number, newGrade: number,idSchool: number, timedUser: TypeTimedUser): Promise<any> {
    const resp = await postAsync(`${endpoint}/noteProject?`,{ProjectId: idProject,Grade: newGrade,SchoolId: idSchool,User:timedUser})
    return resp.data
}

//Case Staff Member
export async function changeNoteProject(idProject: number, newGrade: number,juryId: number, timedUser: TypeTimedUser): Promise<any> {
    const resp = await postAsync(`${endpoint}/noteProject?`,{ProjectId: idProject,Grade: newGrade,JuryId: juryId,User:timedUser})
    return resp.data
}

//Case Staff Member
export async function blockedNoteProject(idProject: number, jurysId: number[], IndividualGrade:[string,number]): Promise<any> {
    const resp = await postAsync(`${endpoint}/blockedProject`,{ProjectId: idProject,JurysId: jurysId,IndividualGrade:IndividualGrade})
    return resp.data
}