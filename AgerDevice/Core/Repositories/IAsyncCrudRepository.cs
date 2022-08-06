using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgerDevice.Core.Models;

namespace AgerDevice.Core.Repositories
{
    public interface IAsyncCrudRepository<T, TQuery> where T : IBase
    {
        Task CreateAsync(T record);
        Task BulkCreateAsync(IEnumerable<T> records);
        Task<T> ReadAsync(string id);
        Task UpdateAsync(T record);
        Task DeleteAsync(string id);
    }
}