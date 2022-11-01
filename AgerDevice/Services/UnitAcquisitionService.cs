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

        public UnitAcquisitionService(UnitManager unitManager) 
        {
            _unitManager = unitManager;
            _rnd = new Random();
            _dataQueue = new ConcurrentQueue<IncomingData>();
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
            if(_dataQueue.Count > 0) {
                await _unitManager.NewData(_dataQueue.ToArray());
            }
        }
    }
}
