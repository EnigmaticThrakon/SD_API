using AgerDevice.Core.Models;
using AgerDevice.Core.Query;

namespace AgerDevice.Core.Repositories
{
    public interface IRunDataRepository : IAsyncQueryableRepository<RunData, RunDataQuery> 
    { 
        Task CreateAsync(RunData record);
        Task Delete(Guid id);
    }
}