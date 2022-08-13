using AgerDevice.Core.Models;
using AgerDevice.Core.Query;

namespace AgerDevice.Core.Repositories
{
    public interface IUserSettingsRepository : IAsyncCrudRepository<UserSettings>, IAsyncQueryableRepository<UserSettings, UserSettingsQuery> 
    { 
    }
}