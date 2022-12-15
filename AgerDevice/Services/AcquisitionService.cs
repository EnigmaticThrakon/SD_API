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
        private RunDataManager _runDataManager;
        public AcquisitionService(UnitManager unitManager, RunDataManager runDataManager)
        {
            _unitServices = new ConcurrentDictionary<Guid, UnitAcquisitionService>();
            _unitManager = unitManager;
            _runDataManager = runDataManager;
        }

        public async Task StartAsync(CancellationToken cancellationToken) 
        { 
            await Task.Run(() => { Console.WriteLine("Dumb Placeholder"); });
        }

        public async Task StopAsync(CancellationToken cancellationToken) 
        {
            await Task.Run(() => { Console.WriteLine("Dumb Placeholder"); });
        }

        public async Task<DateTime?> StartAcquisition(Guid unitId, Guid runId)
        {
            UnitAcquisitionService? service = null;
            service = _unitServices.GetOrAdd(unitId, x => new UnitAcquisitionService(_unitManager, _runDataManager, unitId));

            if(service != null)
            {
                return await service.StartAcquisition(runId);
            }

            return null;
        }

        public async Task<DateTime?> GetStartTime(Guid unitId)
        {
            UnitAcquisitionService? service = null;
            _unitServices.TryGetValue(unitId, out service);

            if(service != null){
                return service.GetStartTime();
            }

            return null;
        }

        public async Task CreateService(Guid unitId)
        {
            UnitAcquisitionService? service = null;
            _unitServices.TryGetValue(unitId, out service);

            if(service == null) {
                service = new UnitAcquisitionService(_unitManager, _runDataManager, unitId);
                _unitServices.AddOrUpdate(unitId, service, (key, oldValue) => service);
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

        public async Task LiveData(Guid unitId, string data)
        {
            UnitAcquisitionService? service = null;
            _unitServices.TryGetValue(unitId, out service);

            if(service != null)
            {
                await service.IncomingData(data);
            }
        }
    }
}
