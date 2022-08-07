using AgerDevice.Core.Models;
using AgerDevice.Core.Query;
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

        /// <summary>
        /// Endpoint to get a user ID from a passed in device ID
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{deviceId}")]
        public async Task<ActionResult<string>> GetUserId(string deviceId)
        {
            PagedResult<User> results = await _userManager.QueryAsync(new UserQuery() { DeviceId = deviceId });

            if(results.FilteredCount > 0)
            {
                User returningUser = results[0];
                returningUser.LastConnected = DateTime.Now;

                await _userManager.UpdateAsync(returningUser);
                return returningUser.Id.ToString();
            }

            User newUser = new User() {
                Id = Guid.NewGuid(),
                Modified = DateTime.Now,
                IsDeleted = false,
                DeviceId = deviceId,
                LastConnected = DateTime.Now
            };

            await _userManager.CreateAsync(newUser);

            return newUser.Id.ToString();
        }
    }
}
