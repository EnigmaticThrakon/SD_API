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
    public class UserManager
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateAsync(User user)
        {
            await _userRepository.CreateAsync(user);
        }

        public async Task<PagedResult<User>> QueryAsync(UserQuery query = null)
        {
            return await _userRepository.QueryAsync(query);
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _userRepository.DeleteAsync(userId);
        }

        public async Task MarkDeleted(User user)
        {
            user.IsDeleted = true;
            await _userRepository.UpdateAsync(user);
        }
    }
}
