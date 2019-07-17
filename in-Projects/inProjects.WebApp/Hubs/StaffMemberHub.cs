using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.WebApp.Hubs
{
    public class StaffMemberHub : Hub
    {
        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync( Context.ConnectionId, roomName );
        }

        public async Task RefreshList(string roomName)
        {
            await Clients.Group( roomName ).SendAsync( "RefreshList" );
        }
    }
}
