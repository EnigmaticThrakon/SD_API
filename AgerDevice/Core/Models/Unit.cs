using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgerDevice.Core.Models
{
    public class Unit : IBase
    {
        public Guid Id { get; set; }
        public DateTime? Modified { get; set; }
        public string SerialNumber { get; set; }
        public bool IsDeleted { get; set; }
        public string PublicIP { get; set; }
        public bool IsConnected { get; set; }
        public string ConnectionId { get; set; }

        public Unit() { }
    }
}
