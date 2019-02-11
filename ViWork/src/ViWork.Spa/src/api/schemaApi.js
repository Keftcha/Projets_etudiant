import {
    getAsync,
    postAsync,
    putAsync,
    deleteAsync
} from "../helpers/apiHelper";

const endpoint = process.env.VUE_APP_BACKEND + "/api/schema";

export async function createSchemaAsync(model) {
    return await postAsync(`${endpoint}/CreateSchema`, model);
}

export async function GetSchemaByIdAsync(userId) {
    return await getAsync(`${endpoint}/GetSchemaById/${userId}`);
}

export async function DeleteSchemaByIdAsync(schemaId) {
    return await deleteAsync(`${endpoint}/DeleteSchemaById/${schemaId}`)
}