using System;

namespace AgerDevice.Core.Query
{
    public class UserQuery : Query<UserQuery.SortColumns>
    {
        public Guid? Id { get; set; }
        public DateTime? Modified { get; set; }
        public string? DeviceId { get; set; }
        public bool? IsDeleted { get; set; }

        public UserQuery()
        {
            Id = null;
            Modified = null;
            DeviceId = null;
            IsDeleted = null;
        }

        public enum SortColumns
        {
            Id,
            Modified
        }
    }
}