import {AuthService} from "@signature/webfrontauth"
import {appSettings} from "@/modules/config/appSettings"
import { AxiosInstance } from "axios"

let instance: AuthService
let instanceAxios: AxiosInstance

export async function initializeAuthService(axiosFromInitialize: AxiosInstance): Promise<AuthService> {
    instanceAxios = axiosFromInitialize
    instance = await AuthService.createAsync( { identityEndPoint: appSettings.AuthService }, axiosFromInitialize )
    return instance
}
export const getAuthService: () => AuthService = () => instance
export const getAxios: () => AxiosInstance = () => instanceAxios
export * from "@signature/webfrontauth"