using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AgerDevice.Core.Query;

namespace AgerDevice.Core.Repositories
{
    public interface IAsyncQueryableRepository<TModel, TQuery> where TQuery : BaseQuery
    {
        Task<PagedResult<TModel>> QueryAsync(TQuery query = null);
    }
}