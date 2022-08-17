using System;

namespace AgerDevice.Core.Query
{
    public class UnitQuery : Query<UnitQuery.SortColumns>
    {
        public Guid? Id { get; set; }
        public DateTime? Modified { get; set; }
        public string? SerialNumber { get; set; }
        public bool? IsDeleted { get; set; }
        public string? PublicIP { get; set; }
        public bool? IsConnected { get; set; }

        public UnitQuery()
        {
            Id = null;
            Modified = null;
            SerialNumber = null;
            IsDeleted = null;
            PublicIP = null;
            IsConnected = null;
        }

        public enum SortColumns
        {
            Id,
            Modified
        }
    }
}