using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgerDevice.Core.Query
{
    public class UserSettingsQuery : Query<UserSettingsQuery.SortColumns>
    {
        public Guid? Id { get; set; }
        public DateTime? Modified { get; set; }
        public Guid? GroupId { get; set; }
        public string? UserName { get; set; }
        public bool? GroupsEnabled { get; set; }

        public UserSettingsQuery() 
        { 
            Id = null;
            Modified = null;
            GroupId = null;
            UserName = null;
            GroupsEnabled = null;
        }

        public enum SortColumns
        {
            Id,
            Modified
        }
    }
}
