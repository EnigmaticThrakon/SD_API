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
        private readonly UnitManager _unitManager;

        public MonitorHub(UserManager userManager, UnitManager unitManager)
        {
            _userManager = userManager;
            _unitManager = unitManager;
        }

        public async override Task OnConnectedAsync()
        {
            User currentUser;
            try 
            {
                string deviceId = Context.GetHttpContext().Request.Query["deviceId"];
                PagedResult<User> result = await _userManager.QueryAsync(new UserQuery() { SerialNumber = deviceId});

                if(result.FilteredCount > 0) 
                {
                    currentUser = result.First();
                }
                else
                {
                    throw new Exception("No Units Found with Device ID: " + deviceId);
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception();
            }

            currentUser.LastConnected = DateTime.Now;
            currentUser.Modified = DateTime.Now;
            currentUser.ConnectionId = Context.ConnectionId;

            PagedResult<Unit> pairedUnits = await _unitManager.QueryAsync(new UnitQuery() { PairedId = currentUser.Id });

            if(pairedUnits.FilteredCount > 0) {
                foreach(Unit unit in pairedUnits) {
                    await JoinGroup(unit.Id.ToString());
                }
            }

            await _userManager.UpdateAsync(currentUser);

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception ex)
        {
            PagedResult<User> results = await _userManager.QueryAsync(new UserQuery() { ConnectionId = Context.ConnectionId });

            if(results.FilteredCount > 0) {
                User currentUser = results.First();

                PagedResult<Unit> pairedUnits = await _unitManager.QueryAsync(new UnitQuery() { PairedId = currentUser.Id });

                if(pairedUnits.FilteredCount > 0) {

                    foreach(Unit unit in pairedUnits) {
                        await LeaveGroup(unit.Id.ToString());
                    }
                }
            }
            await base.OnDisconnectedAsync(ex);
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
