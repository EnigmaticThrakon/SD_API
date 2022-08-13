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

        /// <summary>
        /// Endpoint to get a user ID from a passed in serial number
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{deviceId}")]
        public async Task<ActionResult<string>> IncomingUser(string deviceId)
        {
            PagedResult<User> results = await _userManager.QueryAsync(new UserQuery() { SerialNumber = deviceId });

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
                SerialNumber = deviceId,
                LastConnected = DateTime.Now
            };

            await _userManager.CreateAsync(newUser);

            return newUser.Id.ToString();
        }
    }
}
