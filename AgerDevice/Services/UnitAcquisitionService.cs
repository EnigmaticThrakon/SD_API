using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Managers;
using AgerDevice.Core.Models;
using System.Collections.Concurrent;

namespace AgerDevice.Services
{
    public class UnitAcquisitionService
    {
        private ConcurrentQueue<IncomingData> _dataQueue;
        private System.Timers.Timer _acquisitionTimer;
        private UnitManager _unitManager;
        private Random _rnd;
        private Guid _unitId;

        public UnitAcquisitionService(UnitManager unitManager, Guid unitId) 
        {
            _unitManager = unitManager;
            _rnd = new Random();
            _dataQueue = new ConcurrentQueue<IncomingData>();
            _unitId = unitId;
        }

        public async Task StartAcquisition()
        {
            _acquisitionTimer = new (interval: 1500);
            _acquisitionTimer.Elapsed += async (sender, e) => await SendData();
            _acquisitionTimer.AutoReset = true;
            _acquisitionTimer.Start();
        }

        public async Task StopAcquisition()
        {
            _acquisitionTimer.Stop();
            _acquisitionTimer.Enabled = false;
            _acquisitionTimer.Dispose();
        }

        public async Task IncomingData(string data)
        {
            List<IncomingData> parsedData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IncomingData>>(data);
            for (int i = 0; i < parsedData.Count(); i++)
            {
                _dataQueue.Enqueue(parsedData[i]);
            }
        }

        private async Task SendData()
        {
            // IncomingData tempData = new IncomingData();
            // tempData.AirFlow = _rnd.Next();
            // tempData.Humidity = _rnd.Next();
            // tempData.Temperature = _rnd.Next();
            // tempData.Timestamp = DateTime.Now;
            // tempData.Door = _rnd.Next(0, 1);

            // await _unitManager.NewData(new Core.Models.IncomingData[] { tempData });
            if(_dataQueue.Count > 0) {
                await _unitManager.NewData(_dataQueue.ToArray(), _unitId);
            }
        }
    }
}
