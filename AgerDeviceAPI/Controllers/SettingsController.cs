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
        // [HttpPut]
        // [Route("${id}/${userName}")]
        // public async Task<ActionResult<string>> UpdateUserName(Guid id, string userName)
        // {
        //     try
        //     {
        //         PagedResult<User> result = await _userManager.QueryAsync(new UserQuery() { IsDeleted = false, Id = id });

        //         if(result.FilteredCount > 0)
        //         {
        //             User currentUser = result[0];
        //             currentUser.UserName = userName.Length > 100 ? userName.Substring(0, 100) : userName;
        //             currentUser.Modified = DateTime.Now;

        //             await _userManager.UpdateAsync(currentUser);
        //             return Ok("success");
        //         }

        //         return BadRequest("No User Found With That Id");
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(ex);
        //     }
        // }

        [HttpPut]
        [Route("Save")]
        public async Task<ActionResult> SaveSettings(UserSettingsViewModel model)
        {
            try
            {
                PagedResult<User> result = await _userManager.QueryAsync(new UserQuery() { Id = model.Id });

                if(result.FilteredCount > 0)
                {
                    result[0].Modified = DateTime.Now;
                    result[0].GroupId = model.GroupId.HasValue ? model.GroupId.Value : result[0].GroupId;
                    result[0].GroupsEnabled = model.GroupsEnabled.HasValue ? model.GroupsEnabled.Value : result[0].GroupsEnabled;
                    result[0].UserName = model.UserName == null ? String.Empty : model.UserName;

                    await _userManager.UpdateAsync(result[0]);

                    return Ok();
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<UserSettingsViewModel> GetCurrentSettings(Guid id)
        {
            try
            {
                PagedResult<User> result = await _userManager.QueryAsync(new UserQuery() { Id = id });

                if(result.FilteredCount > 0)
                {
                    return UserSettingsViewModel.FromModel(result[0]);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(exception: ex, message: null);
            }

            return new UserSettingsViewModel();
        }
    }
}
