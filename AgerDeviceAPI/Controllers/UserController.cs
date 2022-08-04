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
        /// Endpoint to query for a username, returning true if the username is available or false if it's taken
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{username}")]
        public async Task<ActionResult<Guid>> ApplyUsername(string username)
        {
            Guid userId = await _usersHandler.ApplyUsername(username);
            if(userId == Guid.Empty)
            {
                return BadRequest();
            }

            return Ok(userId);
        }
    }
}
