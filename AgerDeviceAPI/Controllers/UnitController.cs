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

        public UnitController(ILogger<UsersController> logger, UnitManager unitManager, UserManager userManager)
        {
            _logger = logger;
            _unitManager = unitManager;
            _userManager = userManager;
        }

        [HttpPut]
        [Route("Unlink")]
        public async Task<ActionResult<bool>> UnlinkUnit(ConnectedDeviceViewModel model) 
        {
            PagedResult<Unit> units = await _unitManager.QueryAsync(new UnitQuery() { Id = model.Id });

            if(units.FilteredCount > 0)
            {
                units[0].PairedId = Guid.Empty;
                units[0].Modified = DateTime.Now;
                units[0].Name = null;

                await _unitManager.UpdateAsync(units[0]);
                await _unitManager.NotifyUnitUnlinked(units[0]);

                return true;
            }

            return false;
        }


        /// <summary></summary>
        /// Endpoint to link unit to user
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Link")]
        public async Task<ActionResult<bool>> LinkUnit(ConnectedDeviceViewModel model)
        {
            PagedResult<Unit> units = await _unitManager.QueryAsync(new UnitQuery() { Id = model.Id, IsConnected = true });

            if(units.FilteredCount > 0 && model.PairedId.HasValue)
            {
                units[0].PairedId = model.PairedId.Value;
                units[0].Modified = DateTime.Now;

                await _unitManager.UpdateAsync(units[0]);
                await _unitManager.NotifyUnitLinked(units[0]);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Endpoint to get the units that are currently linked with the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<ConnectedDeviceViewModel>>> GetUserUnits(Guid id)
        {
            PagedResult<Unit> units = await _unitManager.QueryAsync(new UnitQuery() { IsDeleted = false });
            List<ConnectedDeviceViewModel> response = new List<ConnectedDeviceViewModel>();

            for(int i = 0; i < units.FilteredCount; i++) {
                if(units[i].PairedId == id) {
                    response.Add(new ConnectedDeviceViewModel() {
                        IsConnected = units[i].IsConnected,
                        PublicIP = units[i].PublicIP,
                        SerialNumber = units[i].SerialNumber,
                        Id = units[i].Id,
                        PairedId = units[i].PairedId,
                        Name = units[i].Name
                    });
                }
            }

            return response;
        }

        /// <summary>
        /// Endpoint to automatically add units to user based on public IP address
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Auto")]
        public async Task<ActionResult<List<ConnectedDeviceViewModel>>> AutoAddUnit(GroupViewModel model)
        {
            // List<ConnectedDeviceViewModel> temp = new ConnectedDeviceViewModel[2]{
            //     new ConnectedDeviceViewModel() {
            //         Id = Guid.NewGuid(),
            //         PublicIP = "192.168.5.106"
            //     },
            //     new ConnectedDeviceViewModel() {
            //         Id = Guid.NewGuid(),
            //         PublicIP = "192.168.6.105"
            //     }
            // }.ToList();

            // return temp;

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

            if(unitResults.FilteredCount > 0)
            {
                List<Unit> availableUnits = unitResults.Where(t => t.PairedId == Guid.Empty || t.PairedId == null).ToList();
                List<ConnectedDeviceViewModel> availableDevices = new List<ConnectedDeviceViewModel>();

                availableUnits.ForEach(unit => {
                    availableDevices.Add(new ConnectedDeviceViewModel() {
                        Id = unit.Id,
                        PublicIP = unit.PublicIP,
                        PairedId = unit.PairedId,
                        IsConnected = unit.IsConnected,
                        SerialNumber = unit.SerialNumber,
                        Name = unit.Name
                    });
                });

                availableDevices.Insert(0, new ConnectedDeviceViewModel() { Id = Guid.Empty });
                return availableDevices;
                // return availableGuids.Select(t => t.ToString()).ToArray();
            }
            else
            {
                return new List<ConnectedDeviceViewModel>() { new ConnectedDeviceViewModel() { Id = Guid.Empty } };
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
