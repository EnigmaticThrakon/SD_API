using System;

namespace AgerDevice.Core.Query
{
    public class UserQuery : Query<UserQuery.SortColumns>
    {
        public Guid? Id { get; set; }
        public DateTime? Modified { get; set; }
        public string? SerialNumber { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? LastConnected { get; set; }
        public string? PublicIP { get; set; }
        public Guid? GroupId { get; set; }
        public string? UserName { get; set; }
        public bool? GroupsEnabled { get; set; }

        public UserQuery()
        {
            Id = null;
            Modified = null;
            SerialNumber = null;
            IsDeleted = null;
            LastConnected = null;
            PublicIP = null; 
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