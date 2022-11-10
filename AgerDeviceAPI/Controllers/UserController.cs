using AgerDevice.Core.Models;
using AgerDevice.Core.Query;
using AgerDevice.Core.ViewModels;
using AgerDevice.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgerDeviceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UserManager _userManager;

        public UsersController(ILogger<UsersController> logger, UserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        #region NEEDED_FOR_DEMONSTRATION

/// <summary>
        /// Endpoint to get a user ID from a passed in serial number
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{deviceId}")]
        public async Task<ActionResult<UserViewModel>> IncomingUser(string deviceId)
        {
            PagedResult<User> results = await _userManager.QueryAsync(new UserQuery() { SerialNumber = deviceId });

            if(results.FilteredCount > 0)
            {
                User returningUser = results.First();
                returningUser.LastConnected = DateTime.Now;
                returningUser.Modified = DateTime.Now;

                await _userManager.UpdateAsync(returningUser);
                return new UserViewModel() { 
                    Id = returningUser.Id, 
                    UserName = String.IsNullOrEmpty(returningUser.UserName) ? returningUser.Id.ToString() : returningUser.UserName,
                    DeviceId = returningUser.SerialNumber
                };
            }

            Guid newUserId = Guid.NewGuid();
            User newUser = new User() {
                Id = newUserId,
                Modified = DateTime.Now,
                IsDeleted = false,
                SerialNumber = deviceId,
                LastConnected = DateTime.Now,
                UserName = newUserId.ToString()
            };

            await _userManager.CreateAsync(newUser);

            return new UserViewModel() { 
                Id = newUser.Id, 
                UserName = newUser.UserName,
                DeviceId = newUser.SerialNumber
            };
        }
        
        #endregion NEEDED_FOR_DEMONSTRATION
    }
}
