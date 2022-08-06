using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AgerDevice.Redis;

namespace AgerDeviceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UsersHandler _usersHandler;

        public UsersController(ILogger<UsersController> logger, UsersHandler usersHandler)
        {
            _logger = logger;
            _usersHandler = usersHandler;
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
            return Guid.NewGuid().ToString();
        }
    }
}
