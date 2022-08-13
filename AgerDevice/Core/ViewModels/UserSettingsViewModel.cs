using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgerDevice.Core.ViewModels
{
    public class UserSettingsViewModel
    {
        public Guid? Id { get; set; }
        public Guid? GroupId { get; set; }
        public string? UserName { get; set; }
        public bool? GroupsEnabled { get; set; }

        public UserSettingsViewModel() { }
    }
}
