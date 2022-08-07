using System;

namespace AgerDevice.Core.Models
{
    public interface IBase
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Modified timestamp in UTC.
        /// </summary>
        DateTime? Modified { get; set; }
    }
}