using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AgerDevice.Redis;

namespace AgerDeviceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private ILogger _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<string>> TestUsername(string username)
        {
            return Ok(username);
        }
    }
}
