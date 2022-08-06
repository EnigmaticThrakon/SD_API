using System;

namespace AgerDevice.Core.Models
{
    public interface IBase
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Creation timestamp in UTC.
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// Modified timestamp in UTC.
        /// </summary>
        DateTime? Modified { get; set; }
    }
}