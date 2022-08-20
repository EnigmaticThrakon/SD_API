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
        private UserSettingsManager _userSettingsManager;

        public SettingsController(ILogger<SettingsController> logger, UserManager userManager, UserSettingsManager userSettingsManager)
        {
            _logger = logger;
            _userManager = userManager;
            _userSettingsManager = userSettingsManager;
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
                PagedResult<UserSettings> result = await _userSettingsManager.QueryAsync(new UserSettingsQuery() { Id = model.Id });

                if(result.FilteredCount > 0)
                {
                    UserSettings settings = result[0];

                    settings.Modified = DateTime.Now;
                    settings.GroupId = model.GroupId.HasValue ? model.GroupId.Value : settings.GroupId;
                    settings.GroupsEnabled = model.GroupsEnabled.HasValue ? model.GroupsEnabled.Value : settings.GroupsEnabled;
                    settings.UserName = model.UserName == null ? String.Empty : model.UserName;

                    await _userSettingsManager.UpdateAsync(settings);

                    return Ok();
                }

                UserSettings newSettings = new UserSettings() {
                    Id = model.Id.HasValue ? model.Id.Value : Guid.Empty,
                    GroupId = model.GroupId.HasValue ? model.GroupId.Value : Guid.NewGuid(),
                    GroupsEnabled = model.GroupsEnabled.HasValue ? model.GroupsEnabled.Value : false,
                    Modified = DateTime.Now,
                    UserName = model.UserName == null ? String.Empty : model.UserName
                };

                await _userSettingsManager.CreateAsync(newSettings);

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
                PagedResult<UserSettings> result = await _userSettingsManager.QueryAsync(new UserSettingsQuery() { Id = id });

                if(result.FilteredCount > 0)
                {
                    return UserSettingsViewModel.FromModel(result[0]);
                }

                UserSettings newUserSettings = new UserSettings() 
                {
                    Id = id,
                    Modified = DateTime.Now,
                    GroupId = Guid.NewGuid(),
                    GroupsEnabled = false,
                    UserName = String.Empty
                };

                await _userSettingsManager.CreateAsync(newUserSettings);

                return UserSettingsViewModel.FromModel(newUserSettings);
            }
            catch(Exception ex)
            {
                _logger.LogError(exception: ex, message: null);
                return new UserSettingsViewModel();
            }
        }
    }
}
