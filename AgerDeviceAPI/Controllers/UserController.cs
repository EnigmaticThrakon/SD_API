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
        /// Endpoint to get a user ID from a passed in serial number
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{deviceId}")]
        public async Task<ActionResult<UserViewModel>> IncomingUser(string deviceId)
        {
            PagedResult<User> results = await _userManager.QueryAsync(new UserQuery() { SerialNumber = deviceId });

            if(results.FilteredCount > 0)
            {
                User returningUser = results[0];
                returningUser.LastConnected = DateTime.Now;

                await _userManager.UpdateAsync(returningUser);
                return new UserViewModel() { Id = returningUser.Id, PublicIP = returningUser.PublicIP };
            }

            User newUser = new User() {
                Id = Guid.NewGuid(),
                Modified = DateTime.Now,
                IsDeleted = false,
                SerialNumber = deviceId,
                LastConnected = DateTime.Now
            };

            await _userManager.CreateAsync(newUser);

            return new UserViewModel() { Id = newUser.Id, PublicIP = newUser.PublicIP };
        }

        /// <summary>
        /// Endpont for getting the current settings of the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Endpont for saving changed settings for a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
    }
}
