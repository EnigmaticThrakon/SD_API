using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgerDevice.Core.Models
{
    public class Link : IBase
    {
        public Guid Id { get; set; }        // Represents Unit Id
        public DateTime? Modified { get; set; }
        public Guid GroupId { get; set; }

        public Link() { }
    }
}
