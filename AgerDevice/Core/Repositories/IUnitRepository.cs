using AgerDevice.Core.Models;
using AgerDevice.Core.Query;

namespace AgerDevice.Core.Repositories
{
    public interface IUnitRepository : IAsyncCrudRepository<Unit>, IAsyncQueryableRepository<Unit, UnitQuery> 
    { 
    }
}