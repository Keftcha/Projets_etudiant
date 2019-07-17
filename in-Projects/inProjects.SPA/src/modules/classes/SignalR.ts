import * as SignalR from "@aspnet/signalr"
import store from "../../store"
import { getIdSchoolOfUser } from '@/api/schoolApi';
class SignalRGestion {
    public connection!: SignalR.HubConnection
    public idZone: number = 0

    public async connect() {
        this.idZone = await getIdSchoolOfUser()
        this.connection = await new SignalR.HubConnectionBuilder()
            .withUrl(process.env.VUE_APP_BACKEND + "/staffMemberHub").build()
        await this.connection.start()
        await this.connection.invoke("JoinRoom", this.idZone)
        store.state.connectionStaffMember = this.connection
    }
}

export { SignalRGestion }