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

        ///

        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<string>> TestUsername(string username)
        {
            return Ok(username);
        }
    }
}
