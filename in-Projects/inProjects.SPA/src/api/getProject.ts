import { getAsync } from "../helpers/apiHelper"
import {ProjectSheet, ProjectPiSheet, ProjectPfhSheet} from "../modules/classes/ProjectSheet"

const endpoint = process.env.VUE_APP_BACKEND + "/api/Project"

export async function GetProject(idx: number): Promise<ProjectSheet | ProjectPiSheet | ProjectPfhSheet> {
    const resp = await getAsync(`${endpoint}/getProjectSheet?idx=${idx}`)

    if (resp.data.type === "I") {
        return new ProjectPiSheet(
            resp.data.project.place,
            resp.data.project.name,
            resp.data.project.semester,
            resp.data.project.sector,
            resp.data.project.logo,
            resp.data.project.slogan,
            resp.data.project.pitch,
            [
                resp.data.project.team.item1,
                resp.data.project.team.item2
            ],
            resp.data.project.technos
        )
    } else if (resp.data.type === "H") {
        return new ProjectPfhSheet(
            resp.data.project.name,
            resp.data.project.semester,
            resp.data.project.sector,
            resp.data.project.logo,
            resp.data.project.slogan,
            resp.data.project.pitch,
            [
                resp.data.project.team.item1,
                resp.data.project.team.item2
            ],
            resp.data.project.background
        )
    } else {
        return new ProjectSheet(
            resp.data.project.name,
            resp.data.project.semester,
            resp.data.project.sector,
            resp.data.project.logo,
            resp.data.project.slogan,
            resp.data.project.pitch,
            [
                resp.data.project.team.item1,
                resp.data.project.team.item2
            ],
            resp.data.project.type
        )
    }
}