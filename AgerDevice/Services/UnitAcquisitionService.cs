using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Managers;
using AgerDevice.Core.Models;
using System.Collections.Concurrent;
using AgerDevice.Core.Query;

namespace AgerDevice.Services
{
    public class UnitAcquisitionService
    {
        private ConcurrentQueue<RunData> _dataQueue;
        private System.Timers.Timer? _acquisitionTimer;
        private UnitManager _unitManager;
        private RunDataManager _runDataManager;
        private Random _rnd;
        private Guid _unitId;
        private DateTime? _acquisitionStart;
        private Guid? _currentRunId;

        public UnitAcquisitionService(UnitManager unitManager, RunDataManager runDataManager, Guid unitId) 
        {
            _unitManager = unitManager;
            _rnd = new Random();
            _dataQueue = new ConcurrentQueue<RunData>();
            _unitId = unitId;
            _runDataManager = runDataManager;
        }

        public DateTime GetStartTime()
        {
            return _acquisitionStart.HasValue ? _acquisitionStart.Value : DateTime.Now;
        }

        public async Task<DateTime> StartAcquisition(Guid runId)
        {
            _currentRunId = runId;

            _acquisitionTimer = new (interval: 1500);
            _acquisitionTimer.Elapsed += async (sender, e) => await SendData();
            _acquisitionTimer.AutoReset = true;
            _acquisitionTimer.Start();

            _acquisitionStart = DateTime.Now;
            return _acquisitionStart.Value;
        }

        public async Task StopAcquisition()
        {
            _currentRunId = null;

            if(_acquisitionTimer != null){
                _acquisitionTimer.Stop();
                _acquisitionTimer.Enabled = false;
                _acquisitionTimer.Dispose();
            }
        }

        public async Task IncomingData(string data)
        {
            List<RunData> parsedData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RunData>>(data);
            for (int i = 0; i < parsedData.Count(); i++)
            {
                _dataQueue.Enqueue(parsedData[i]);
            }
        }

        private async Task SendData()
        {
            if(_currentRunId.HasValue) {
                RunData tempData = new RunData() {
                    AirFlow = _rnd.Next(),
                    Humidity = _rnd.Next(),
                    Temperature = _rnd.Next(),
                    Timestamp = DateTime.Now,
                    DoorClosed = false,
                    AssociatedRun = _currentRunId.Value
                };

                await _runDataManager.CreateAsync(tempData);
                await _unitManager.NewData(new RunData[] { tempData }, _unitId);
            }
            // if(_dataQueue.Count > 0) {
            //     await _unitManager.NewData(_dataQueue.ToArray(), _unitId);
            // }
        }
    }
}
