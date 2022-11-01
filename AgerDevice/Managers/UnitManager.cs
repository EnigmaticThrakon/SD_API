using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Core.Models;
using AgerDevice.Core.Query;
using AgerDevice.Core.Repositories;
using AgerDevice.Hubs;
using Microsoft.AspNetCore.SignalR;
using AgerDevice.Services;

namespace AgerDevice.Managers
{
    public class UnitManager
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IHubContext<DeviceHub> _deviceHub;
        private readonly IHubContext<MonitorHub> _monitorHub;

        public UnitManager(IUnitRepository unitRepository, IHubContext<MonitorHub> monitorHub, IHubContext<DeviceHub> deviceHub)
        {
            _unitRepository = unitRepository;
            _monitorHub = monitorHub;
            _deviceHub = deviceHub;
        }

        public async Task CreateAsync(Unit unit)
        {
            await _unitRepository.CreateAsync(unit);
        }

        public async Task<PagedResult<Unit>> QueryAsync(UnitQuery? query = null)
        {
            return await _unitRepository.QueryAsync(query);
        }

        public async Task UpdateAsync(Unit unit)
        {
            await _unitRepository.UpdateAsync(unit);
        }

        // public async Task DeleteAsync(Guid userId)
        // {
        //     await _unitRepository.Delete(userId);
        // }

        public async Task MarkDeleted(Unit unit)
        {
            unit.IsDeleted = true;
            await _unitRepository.UpdateAsync(unit);
        }

        public async Task NotifyConnectionChange(Unit unit)
        {
            await _monitorHub.Clients.All.SendAsync("newConnection", unit);
        }

        public async Task NotifyUnitLinked(Unit unit)
        {
            await _monitorHub.Clients.All.SendAsync("unitLinked", unit);
        }

        public async Task NotifyUnitUnlinked(Unit unit)
        {
            await _monitorHub.Clients.All.SendAsync("unitUnlinked", unit);
        }

        public async Task NewData(IncomingData[] data)
        {
            await _monitorHub.Clients.All.SendAsync("newValue", Newtonsoft.Json.JsonConvert.SerializeObject(data));
        }

        public async Task SendCommand(string connectionId, string command)
        {
            await _deviceHub.Clients.Client(connectionId).SendAsync("ExecuteCommand", command);
        }
    }
}
