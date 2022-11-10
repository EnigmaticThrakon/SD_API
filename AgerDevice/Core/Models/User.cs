using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgerDevice.Core.Models
{
    public class User : IBase
    {
        public Guid Id { get; set; }
        public DateTime? Modified { get; set; }
        public string? SerialNumber { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? LastConnected { get; set; }
        public string? UserName { get; set; }

        public User() { }
    }
}
