using System;

namespace AgerDevice.Core.Query
{
    public class RunDataQuery : Query<RunDataQuery.SortColumns>
    {
        public Guid AssociatedRun { get; set; }
        public DateTime? Timestamp { get; set; }
        public Decimal? Temperature { get; set; }
        public Decimal? Weight { get; set; }
        public Decimal? AirFlow { get; set; }
        public Decimal? Humidity { get; set; }
        public bool? DoorClosed { get; set; }

        public RunDataQuery()
        {
            Timestamp = null;
            Temperature = null;
            Weight = null;
            AirFlow = null;
            Humidity = null;
            DoorClosed = null;
        }

        public enum SortColumns
        {
            Id,
            Modified
        }
    }
}