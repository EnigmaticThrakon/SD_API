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

        /// <summary>
        /// Endpoint to get all members apart of group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("group/members/${groupId}")]
        public async Task<ActionResult<string[]>> GetGroupMembers(Guid groupId)
        {
            try
            {
                PagedResult<User> result = await _userManager.QueryAsync(new UserQuery() { IsDeleted = false, GroupId = groupId });

                if(result.FilteredCount > 0) 
                {
                    return Ok(result.Select(t => String.IsNullOrEmpty(t.UserName) ? t.Id.ToString() : t.UserName).ToArray());
                }

                return new string[0];
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Endpoint to alter users group id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("group")]
        public async Task<ActionResult<string>> UpdateUserGroup(GroupViewModel model)
        {
            if (model.UserId != null && model.GroupId != null)
            {
                try
                {
                    PagedResult<User> result = await _userManager.QueryAsync(new UserQuery() { IsDeleted = false, Id = model.UserId });

                    if (result.FilteredCount > 0)
                    {
                        User currentUser = result[0];
                        currentUser.GroupId = model.GroupId.Value;
                        currentUser.Modified = DateTime.Now;

                        await _userManager.UpdateAsync(currentUser);
                        return "success";
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

            }

            return BadRequest("Invalid User or Group ID");
        }
    }
}
