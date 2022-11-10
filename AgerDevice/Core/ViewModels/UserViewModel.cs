using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgerDevice.Core.ViewModels
{
    public class UserViewModel
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? DeviceId { get; set; }

        public UserViewModel() { }
    }
}
