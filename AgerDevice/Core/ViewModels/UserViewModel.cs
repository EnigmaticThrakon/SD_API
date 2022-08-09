using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgerDevice.Core.ViewModels
{
    public class ConnectedDeviceViewModel
    {
        public Guid? Id { get; set; }
        public string? SerialNumber { get; set; }
        public string? PublicIP { get; set; }

        public ConnectedDeviceViewModel() { }
    }
}
