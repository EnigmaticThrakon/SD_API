using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;

namespace AgerDevice.Hubs
{
    public class MonitorHub : Hub
    {

        public async override Task OnConnectedAsync()
        {
            var feature = Context.Features.Get<IHttpConnectionFeature>();
            var remoteAddress = feature.RemoteIpAddress;
            await base.OnConnectedAsync();
        }
        
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
