using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgerDevice.Core.Models
{
    public class RunData : IBase
    {
        public Guid AssociatedRun { get; set; }
        public DateTime? Timestamp { get; set; }
        public Decimal? Temperature { get; set; }
        public Decimal? Weight { get; set; }
        public Decimal? AirFlow { get; set; }
        public Decimal? Humidity { get; set; }
        public bool? DoorClosed { get; set; }
        public RunData() { }
    }
}
