using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgerDevice.Core.Models
{
    public class IncomingData
    {
        public DateTime? Timestamp { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? AirFlow { get; set; }
        public double? Weight { get; set; }
        public bool? Door { get; set; }

        public IncomingData() { }
    }
}
