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
    public class SettingsController : ControllerBase
    {
        private ILogger _logger;
        private UserManager _userManager;

        public SettingsController(ILogger<SettingsController> logger, UserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        /// <summary>
        /// Endpoint to update users name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("${id}/${userName}")]
        public async Task<ActionResult<string>> UpdateUserName(Guid id, string userName)
        {
            try
            {
                PagedResult<User> result = await _userManager.QueryAsync(new UserQuery() { IsDeleted = false, Id = id });

                if(result.FilteredCount > 0)
                {
                    User currentUser = result[0];
                    currentUser.UserName = userName.Length > 100 ? userName.Substring(0, 100) : userName;
                    currentUser.Modified = DateTime.Now;

                    await _userManager.UpdateAsync(currentUser);
                    return Ok("success");
                }

                return BadRequest("No User Found With That Id");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
