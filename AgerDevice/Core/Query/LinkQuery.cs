using System;

namespace AgerDevice.Core.Query
{
    public class LinkQuery : Query<LinkQuery.SortColumns>
    {
        public Guid? Id { get; set; }
        public DateTime? Modified { get; set; }
        public Guid? GroupId { get; set; }

        public LinkQuery()
        {
            Id = null;
            Modified = null;
            GroupId = null;
        }

        public enum SortColumns
        {
            Id,
            Modified
        }
    }
}