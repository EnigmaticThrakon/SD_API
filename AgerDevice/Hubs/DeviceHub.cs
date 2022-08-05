using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Services;
using AgerDevice.Managers;

namespace AgerDevice.Hubs
{
    public class DeviceHub : Hub
    {
        private readonly AcquisitionService _acquisitionService;
        // private readonly UserManager _userManager;
        // private readonly UnitManager _unitManager;

        public DeviceHub(AcquisitionService acquisitionService)//, UserManager userManager, UnitManager unitManager)
        {
            _acquisitionService = acquisitionService;
            // _userManager = userManager;
            // _unitManager = unitManager;
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception ex)
        {
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
