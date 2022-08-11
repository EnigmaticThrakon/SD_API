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
    public class UnitController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UnitManager _unitManager;
        private readonly UserManager _userManager;
        private readonly LinkManager _linkManager;

        public UnitController(ILogger<UsersController> logger, UnitManager unitManager, UserManager userManager, LinkManager linkManager)
        {
            _logger = logger;
            _unitManager = unitManager;
            _userManager = userManager;
            _linkManager = linkManager;
        }

        /// <summary>
        /// Endpoint to automatically add units to user based on public IP address
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("NewUnit/Auto")]
        public async Task<ActionResult<string[]>> AutoAddUnit(GroupViewModel model)
        {
            if(model.UserId == null)
                return BadRequest("User ID Cannot Be Null");

            try
            {
            PagedResult<User> userResults = await _userManager.QueryAsync(new UserQuery() { IsDeleted = false, Id = model.UserId });

            User currentUser;
            if(userResults.FilteredCount > 0)
            {
                currentUser = userResults[0];
            }
            else
            {
                return BadRequest("No User With That ID Found");
            }

            if(currentUser.PublicIP == null)
            {
                return BadRequest("Public IP is Null");
            }

            PagedResult<Unit> unitResults = await _unitManager.QueryAsync(new UnitQuery() { IsDeleted = false, PublicIP = currentUser.PublicIP });
            PagedResult<Link> linkResults = await _linkManager.QueryAsync();

            if(unitResults.FilteredCount > 0)
            {
                List<Guid> takenUnitGuids = new List<Guid>();

                if(linkResults.FilteredCount > 0)
                {
                    takenUnitGuids = linkResults.Select(t => t.Id).ToList();
                }

                List<Guid> availableGuids = new List<Guid>();
                unitResults.Select(t => t.Id).ToList().ForEach(t => {
                    if(!takenUnitGuids.Contains(t))
                        availableGuids.Add(t);
                });

                return availableGuids.Select(t => t.ToString()).ToArray();
            }
            else
            {
                return new string[0];
            }
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Endpoint to get a user ID from a passed in device ID
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UnitData")]
        public async Task<ActionResult<string>> IncomingUnit(ConnectedDeviceViewModel model)
        {
            PagedResult<Unit> results = await _unitManager.QueryAsync(new UnitQuery() { SerialNumber = model.SerialNumber });

            if(results.FilteredCount > 0)
            {
                Unit connectingUnit = results[0];
                return connectingUnit.Id.ToString();
            }

            Unit newUnit = new Unit() {
                Id = Guid.NewGuid(),
                Modified = DateTime.Now,
                IsDeleted = false,
                SerialNumber = model.SerialNumber
            };

            await _unitManager.CreateAsync(newUnit);

            return newUnit.Id.ToString();
        }
    }
}
