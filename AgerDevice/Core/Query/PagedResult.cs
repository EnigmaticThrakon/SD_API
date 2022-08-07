using System.Collections;
using System.Collections.Generic;

namespace AgerDevice.Core.Query
{
    public class PagedResult<T> : List<T>
    {
        public int TotalCount => _totalCount;
        public int FilteredCount => _filteredCount;
        private readonly int _totalCount;
        private readonly int _filteredCount;

        public PagedResult(IEnumerable<T> records, int totalCount, int filteredCount)
            : base(records)
        {
            _totalCount = totalCount;
            _filteredCount = filteredCount;
        }
    }
}