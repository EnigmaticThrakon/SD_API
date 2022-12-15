using AgerDevice.Core.Models;
using AgerDevice.Core.Query;

namespace AgerDevice.Core.Repositories
{
    public interface IRunsRepository : IAsyncCrudRepository<Run>, IAsyncQueryableRepository<Run, RunQuery> 
    { 
    }
}