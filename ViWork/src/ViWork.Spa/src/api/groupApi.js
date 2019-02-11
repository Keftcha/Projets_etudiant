import {
    getAsync,
    postAsync,
    putAsync,
    deleteAsync
} from "../helpers/apiHelper";

const endpoint = process.env.VUE_APP_BACKEND + "/api/group";

export async function createGroupAsync(model) {
    return await postAsync(`${endpoint}/CreateGroup`, model);
}

export async function getGroupListAsync(userId) {   
    return await getAsync(`${endpoint}/GetGroup/${userId}`);
}

export async function getGroupData(groupId) {
    return await getAsync(`${endpoint}/GetGroupData/${groupId}`);
}

export async function deleteGroupAsync(groupId) {
    return await deleteAsync(`${endpoint}/${groupId}`)
}