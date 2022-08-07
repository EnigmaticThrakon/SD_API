using AgerDevice.Core.Models;
using AgerDevice.Core.Query;

namespace AgerDevice.Core.Repositories
{
    public interface IUserRepository : IAsyncCrudRepository<User>, IAsyncQueryableRepository<User, UserQuery> 
    { 
    }
}