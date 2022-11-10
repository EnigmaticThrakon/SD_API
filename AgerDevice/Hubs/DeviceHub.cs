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

                if (result.FilteredCount > 0)
                {
                    currentUnit = result[0];
                }
                else
                {
                    throw new Exception("No Units Found with ID: " + unitId);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception();
            }

            currentUnit.Modified = DateTime.Now;
            currentUnit.ConnectionId = Context.ConnectionId;
            currentUnit.IsConnected = true;

            await _unitManager.UpdateAsync(currentUnit);
            await _unitManager.NotifyStatusChange(currentUnit);
            await _acquisitionService.CreateService(currentUnit.Id);

            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception ex)
        {
            try
            {
                PagedResult<Unit> result = await _unitManager.QueryAsync(new Core.Query.UnitQuery() { ConnectionId = Context.ConnectionId });

                if (result.FilteredCount > 0)
                {
                    Unit currentUnit = result[0];
                    currentUnit.Modified = DateTime.Now;
                    currentUnit.IsConnected = false;
                    currentUnit.ConnectionId = String.Empty;

                    await _unitManager.UpdateAsync(currentUnit);
                    await _unitManager.NotifyStatusChange(currentUnit);
                }
                else
                {
                    throw new Exception("No Units Found with Connection ID: " + Context.ConnectionId);
                }
            }
            catch (Exception innerEx)
            {
                Console.Write(innerEx.Message);
                throw new Exception();
            }

            await base.OnDisconnectedAsync(ex);
        }

        public async Task LiveData(Guid unitId, string data)
        {
            await _acquisitionService.LiveData(unitId, data);
        }
    }
}
