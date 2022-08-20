using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Core.Repositories;
using AgerDevice.Core.Models;
using AgerDevice.Core.Query;

namespace AgerDevice.Managers
{
    public class UserSettingsManager
    {
        private readonly IUserSettingsRepository _userSettingsRepository;

        public UserSettingsManager(IUserSettingsRepository userSettingsRepository)
        {
            _userSettingsRepository = userSettingsRepository;
        }

        public async Task CreateAsync(UserSettings userSettings)
        {
            await _userSettingsRepository.CreateAsync(userSettings);
        }

        public async Task<PagedResult<UserSettings>> QueryAsync(UserSettingsQuery? query = null)
        {
            return await _userSettingsRepository.QueryAsync(query);
        }

        public async Task UpdateAsync(UserSettings userSettings)
        {
            await _userSettingsRepository.UpdateAsync(userSettings);
        }

        // public async Task DeleteAsync(Guid userId)
        // {
        //     await _userSettingsRepository.Delete(userId);
        // }
    }
}
