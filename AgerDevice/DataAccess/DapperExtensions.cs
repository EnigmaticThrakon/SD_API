using System;
using System.Collections.Generic;
using System.Linq;
using AgerDevice.Core.Query;

namespace AgerDevice.DataAccess
{
    public static class DapperExtensions
    {
        public static string? GetOrderByString<TSort>(this Query<TSort> query, Dictionary<TSort, string>? columns = null) where TSort : notnull
        {
            if (query.Sort == null)
            {
                return null;
            }

            return String.Join(", ", query.Sort.Select(t => String.Format("{0} {1}", columns != null && columns.ContainsKey(t.Value.Item1) ? columns[t.Value.Item1] : (t.Value.Item1 != null ? t.Value.Item1.ToString() : null), t.Value.Item2 == SortDirection.Ascending ? "ASC" : "DESC")));
        }
    }
}