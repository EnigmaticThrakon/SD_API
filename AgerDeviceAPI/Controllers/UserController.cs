using AgerDevice.Core.Models;
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
            User results = _userManager.
            User tempUser = new User() {
                Id = Guid.NewGuid(),
                Modified = DateTime.Now,
                IsDeleted = false,
                DeviceId = deviceId
            };

            await _userManager.CreateAsync(tempUser);

            return tempUser.Id.ToString();
        }
    }
}
