using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgerDevice.Core.Models
{
    public class Run : IBase
    {
        public Guid Id { get; set; }
        public Guid? AssociatedUnit { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Duration { get; set; }
        public long? NumEntries { get; set; }
        public List<RunData>? Data { get; set; }

        public Run() { }
    }
}
