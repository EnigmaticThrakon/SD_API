using System;

namespace AgerDevice.Core.Query
{
    public class RunQuery : Query<RunQuery.SortColumns>
    {
        public Guid? Id { get; set; }
        public Guid? AssociatedUnit { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Decimal? Duration { get; set; }
        public long? NumEntries { get; set; }

        public RunQuery()
        {
            Id = null;
            Modified = null;
            StartTime = null;
            EndTime = null;
            Duration = null;
            NumEntries = null;
            AssociatedUnit = null;
        }

        public enum SortColumns
        {
            Id,
            Modified
        }
    }
}