using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Core.Models;
using AgerDevice.Core.Query;
using AgerDevice.Managers;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;

namespace AgerDevice.Hubs
{
    public class MonitorHub : Hub
    {
        private readonly UserManager _userManager;

        public MonitorHub(UserManager userManager)
        {
            _userManager = userManager;
        }

        public async override Task OnConnectedAsync()
        {
            User currentUser;
            try 
            {
                Guid userId = Guid.Parse(Context.GetHttpContext().Request.Query["user"]);
                PagedResult<User> result = await _userManager.QueryAsync(new UserQuery() { Id = userId });

                if(result.FilteredCount > 0) 
                {
                    currentUser = result[0];
                }
                else
                {
                    throw new Exception("No Units Found with ID: " + userId);
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception();
            }

            IHttpConnectionFeature feature = Context.Features.Get<IHttpConnectionFeature>();
            currentUser.PublicIP = feature.RemoteIpAddress.ToString().Trim();
            currentUser.LastConnected = DateTime.Now;
            currentUser.Modified = DateTime.Now;

            await _userManager.UpdateAsync(currentUser);

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
