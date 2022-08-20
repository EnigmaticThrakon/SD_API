using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Core.Models;

namespace AgerDevice.Core.ViewModels
{
    public class ConnectedDeviceViewModel
    {
        public Guid? Id { get; set; }
        public string? SerialNumber { get; set; }
        public string? PublicIP { get; set; }
        public bool? IsConnected { get; set; }
        public Guid? PairedId { get; set; }
        public string? Name { get; set; }

        public ConnectedDeviceViewModel() { }

        static ConnectedDeviceViewModel FromUnitModel(Unit model)
        {
            return new ConnectedDeviceViewModel()
            {
                Id = model.Id,
                SerialNumber = model.SerialNumber,
                PublicIP = model.PublicIP,
                IsConnected = model.IsConnected,
                PairedId = model.PairedId,
                Name = model.Name
            };
        }
    }
}
