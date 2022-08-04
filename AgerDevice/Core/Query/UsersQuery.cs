using System;

namespace AgerDevice.Core.Query
{
    public class UsersQuery
    {
        public Guid? UserId { get; set; }
        public string? Username { get; set; }
        public bool? IsDeleted { get; set; }

        public UsersQuery() {}
    }
}