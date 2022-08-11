using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Services;
using AgerDevice.Managers;
using Microsoft.AspNetCore.Http.Features;
using AgerDevice.Core.Query;
using AgerDevice.Core.Models;
using System.Collections.Concurrent;

namespace AgerDevice.Hubs
{
    public class DeviceHub : Hub
    {
        private ConcurrentDictionary<Guid, string> _deviceConnectionHandler;
        private readonly AcquisitionService _acquisitionService;
        // private readonly UserManager _userManager;
        private readonly UnitManager _unitManager;

        public DeviceHub(AcquisitionService acquisitionService, UnitManager unitManager)//, UserManager userManager, UnitManager unitManager)
        {
            _acquisitionService = acquisitionService;
            // _userManager = userManager;
            _unitManager = unitManager;
            _deviceConnectionHandler = new ConcurrentDictionary<Guid, string>();
        }

        public async override Task OnConnectedAsync()
        {
            Unit currentUnit;
            try 
            {
                Guid unitId = Guid.Parse(Context.GetHttpContext().Request.Query["unit"]);
                PagedResult<Unit> result = await _unitManager.QueryAsync(new Core.Query.UnitQuery() { Id = unitId });

                if(result.FilteredCount > 0) 
                {
                    currentUnit = result[0];
                }
                else
                {
                    throw new Exception("No Units Found with ID: " + unitId);
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception();
            }

            IHttpConnectionFeature feature = Context.Features.Get<IHttpConnectionFeature>();
            currentUnit.PublicIP = feature.RemoteIpAddress.ToString().Trim();
            currentUnit.Modified = DateTime.Now;

            await _unitManager.UpdateAsync(currentUnit);
            _deviceConnectionHandler.AddOrUpdate(currentUnit.Id, Context.ConnectionId, (key, oldValue) => Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception ex)
        {
            Guid unitId = _deviceConnectionHandler.Keys.Where(t => {
                string? connectionId;
                _deviceConnectionHandler.TryGetValue(t, out connectionId);

                return connectionId == Context.ConnectionId;
            }).FirstOrDefault();

            if(unitId != null) 
            {
                string temp;
                _deviceConnectionHandler.Remove(unitId, out temp);
            }
            
            await base.OnDisconnectedAsync(ex);
        }

        public async Task SendMessage(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("SendSignal", Context.ConnectionId, message);
        }

        public async Task test(string message) {
            return;
        }
    }
}
