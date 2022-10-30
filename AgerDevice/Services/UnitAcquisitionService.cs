using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Managers;

namespace AgerDevice.Services
{
    public class UnitAcquisitionService
    {
        private System.Timers.Timer _acquisitionTimer;
        private UnitManager _unitManager;
        private Random _rnd;

        public UnitAcquisitionService(UnitManager unitManager) 
        {
            _unitManager = unitManager;
            _rnd = new Random();
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

        private async Task SendData()
        {
            await _unitManager.NewData(_rnd.Next(400));
        }
    }
}
