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

        public UserQuery()
        {
            Id = null;
            Modified = null;
            SerialNumber = null;
            IsDeleted = null;
            LastConnected = null;
            PublicIP = null;
        }

        public enum SortColumns
        {
            Id,
            Modified
        }
    }
}