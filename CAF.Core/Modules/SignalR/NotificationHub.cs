using Microsoft.AspNetCore.SignalR;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Core.Modules.SignalR
{
    public class NotificationHub : Hub
    {
        public void SendToAll(string name, string message)
        {
            Clients.All.SendAsync("sendToAll", name, message);
        }
        public override Task OnConnectedAsync()
        {
            
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
