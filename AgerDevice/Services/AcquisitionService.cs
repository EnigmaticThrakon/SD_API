using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AgerDevice.Services;
using AgerDevice.Managers;

namespace AgerDevice.Services
{
    public class AcquisitionService : IHostedService
    {
        private ConcurrentDictionary<Guid, UnitAcquisitionService> _unitServices;
        private UnitManager _unitManager;
        public AcquisitionService(UnitManager unitManager)
        {
            _unitServices = new ConcurrentDictionary<Guid, UnitAcquisitionService>();
            _unitManager = unitManager;
        }

        public async Task StartAsync(CancellationToken cancellationToken) 
        { 
            await Task.Run(() => { Console.WriteLine("Dumb Placeholder"); });
        }

        public async Task StopAsync(CancellationToken cancellationToken) 
        {
            await Task.Run(() => { Console.WriteLine("Dumb Placeholder"); });
        }

        public async Task StartAcquisition(Guid unitId)
        {
            UnitAcquisitionService? service = null;
            service = _unitServices.GetOrAdd(unitId, x => new UnitAcquisitionService(_unitManager));

            if(service != null)
            {
                await service.StartAcquisition();
            }
        }

        public async Task StopAcquisition(Guid unitId)
        {
            UnitAcquisitionService? service = null;
            _unitServices.TryGetValue(unitId, out service);

            if(service != null)
            {
                await service.StopAcquisition();
            }
        }
    }
}
