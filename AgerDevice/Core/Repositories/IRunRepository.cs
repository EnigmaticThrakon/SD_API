using AgerDevice.Core.Models;
using AgerDevice.Core.Query;

namespace AgerDevice.Core.Repositories
{
    public interface IRunRepository : IAsyncCrudRepository<Run>, IAsyncQueryableRepository<Run, RunQuery> 
    { 
    }
}