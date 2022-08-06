using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgerDeviceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
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
