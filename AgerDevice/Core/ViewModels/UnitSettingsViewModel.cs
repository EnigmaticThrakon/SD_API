using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Core.Models;

namespace AgerDevice.Core.ViewModels
{
    public class UnitSettingsViewModel
    {
        public Guid? Id { get; set; }
        public String? SerialNumber { get; set; }
        public String? Name { get; set; }
        public bool? IsAcquisitioning { get; set; }
        public UnitParametersViewModel? UnitParameters { get; set; }

        public UnitSettingsViewModel() { }
    }

    public class UnitParametersViewModel
    {
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? AirFlow { get; set; }
        public double? Weight { get; set; }
        public bool? Door { get; set; }
    }
}
