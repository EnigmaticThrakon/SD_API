using AgerDevice.Core.Models;
using AgerDevice.Core.Query;

namespace AgerDevice.Core.Repositories
{
    public interface ILinkRepository : IAsyncCrudRepository<Link>, IAsyncQueryableRepository<Link, LinkQuery> 
    { 
    }
}