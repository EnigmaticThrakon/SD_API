using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgerDevice.Core.ViewModels
{
    public class GroupViewModel
    {
        public Guid? UserId { get; set; }
        public Guid? GroupId { get; set; }

        public GroupViewModel() { }
    }
}
