using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.WebApp.Hubs
{
   public class ForumHub : Hub
    {
        public async Task JoinRoom( string roomName )
        {
            await Groups.AddToGroupAsync( Context.ConnectionId, roomName );
        }

        public async Task SendBlocked(string roomName,int idProject)
        {
            await Clients.Group( roomName ).SendAsync( "AddBlocked",idProject );
        }

        public async Task RefreshClassment(string roomName)
        {
            await Clients.Group( roomName ).SendAsync( "RefreshClassment" );
        }
    }
}
