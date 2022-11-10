using AgerDevice.Core.Models;
using AgerDevice.Core.Query;
using AgerDevice.Core.ViewModels;
using AgerDevice.Managers;
using AgerDevice.Services;
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
        private readonly AcquisitionService _acquisitionService;

        public UnitController(ILogger<UsersController> logger, UnitManager unitManager, UserManager userManager, AcquisitionService acquisitionService)
        {
            _logger = logger;
            _unitManager = unitManager;
            _userManager = userManager;
            _acquisitionService = acquisitionService;
        }

        /// <summary>
        /// Endpoint to remove unit from user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // [HttpPut]
        // [Route("Unlink")]
        // public async Task<ActionResult<bool>> UnlinkUnit(UnitViewModel model)
        // {
        //     PagedResult<Unit> units = await _unitManager.QueryAsync(new UnitQuery() { Id = model.Id });

        //     if(units.FilteredCount > 0)
        //     {
        //         // units[0].PairedId = Guid.Empty;
        //         units[0].Modified = DateTime.Now;
        //         units[0].Name = null;

        //         await _unitManager.UpdateAsync(units[0]);
        //         await _unitManager.NotifyUnitUnlinked(units[0]);

        //         return true;
        //     }

        //     return false;
        // }


        /// <summary></summary>
        /// Endpoint to link unit to user
        /// <param name="model"></param>
        /// <returns></returns>
        // [HttpPut]
        // [Route("Link")]
        // public async Task<ActionResult<bool>> LinkUnit(UnitViewModel model)
        // {
        //     PagedResult<Unit> units = await _unitManager.QueryAsync(new UnitQuery() { Id = model.Id });

        //     if(units.FilteredCount > 0)// && model.PairedId.HasValue)
        //     {
        //         // units[0].PairedId = model.PairedId.Value;
        //         units[0].Modified = DateTime.Now;

        //         await _unitManager.UpdateAsync(units[0]);
        //         await _unitManager.NotifyUnitLinked(units[0]);

        //         return true;
        //     }

        //     return false;
        // }

        /// <summary>
        /// Endpoint to get a user ID from a passed in device ID
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UnitData")]
        public async Task<ActionResult<string>> IncomingUnit(UnitViewModel model)
        {
            PagedResult<Unit> results = await _unitManager.QueryAsync(new UnitQuery() { SerialNumber = model.SerialNumber });

            if(results.FilteredCount > 0)
            {
                Unit connectingUnit = results[0];
                return connectingUnit.Id.ToString();
            }

            Guid newUnitGuid = Guid.NewGuid();
            Unit newUnit = new Unit() {
                Id = newUnitGuid,
                Modified = DateTime.Now,
                IsDeleted = false,
                SerialNumber = model.SerialNumber == null ? String.Empty : model.SerialNumber,
                Name = String.IsNullOrEmpty(model.Name) ? newUnitGuid.ToString() : model.Name
            };

            await _unitManager.CreateAsync(newUnit);

            return newUnit.Id.ToString();
        }



        #region NEEDED_FOR_DEMONSTRATION

        /// <summary>
        /// Endpoint to get the units that are currently linked with the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<UnitViewModel>>> GetUserUnits(Guid userId)
        {
            PagedResult<Unit> units = await _unitManager.QueryAsync(new UnitQuery() { IsDeleted = false, PairedId = userId });
            List<UnitViewModel> response = new List<UnitViewModel>();

            for(int i = 0; i < units.FilteredCount; i++) {
                    response.Add(new UnitViewModel() {
                        IsConnected = units[i].IsConnected,
                        SerialNumber = units[i].SerialNumber,
                        Id = units[i].Id,
                        Name = units[i].Name
                    });
            }

            return response;
        }

        /// <summary>
        /// Endpoint to pair a device to a user
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add/{serialNumber}/{userId}")]
        public async Task<ActionResult<bool>> AssignUnitToUser(string serialNumber, Guid userId)
        {
            if(serialNumber != null && userId != null)
            {
                PagedResult<Unit> unitResults = await _unitManager.QueryAsync(new UnitQuery() { SerialNumber = serialNumber });

                if(unitResults.FilteredCount > 0)
                {
                    Unit selectedUnit = unitResults.First();
                    selectedUnit.UpdatePairings(userId, true);

                    await _unitManager.UpdateAsync(selectedUnit);

                    PagedResult<User> userResults = await _userManager.QueryAsync(new UserQuery() { Id = userId });

                    if(userResults.FilteredCount > 0) {
                        User currentUser = userResults.First();

                        await _unitManager.JoinMonitorGroup(currentUser, selectedUnit.Id);
                        await _unitManager.NotifyStatusChange(selectedUnit);
                    }

                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }

        /// <summary>
        /// Endpoint remove user-unit pairing
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("remove/{unitId}/{userId}")]
        public async Task<ActionResult> RemoveUnitFromUser(Guid unitId, Guid userId)
        {
            if(unitId != null && userId != null)
            {
                PagedResult<Unit> unitResults = await _unitManager.QueryAsync(new UnitQuery() { Id = unitId });

                if(unitResults.FilteredCount > 0)
                {
                    Unit selectedUnit = unitResults.First();
                    selectedUnit.UpdatePairings(userId, false);

                    await _unitManager.UpdateAsync(selectedUnit);

                    PagedResult<User> userResults = await _userManager.QueryAsync(new UserQuery() { Id = userId });

                    if(userResults.FilteredCount > 0) {
                        User currentUser = userResults.First();

                        await _unitManager.LeaveMonitorGroup(currentUser, selectedUnit.Id);
                    }

                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }

        /// <summary>
        /// Endpoint to update device nickname
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="unitName"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("update-name/{unitId}/{unitName}")]
        public async Task<ActionResult> UpdateUnitName(Guid unitId, string unitName)
        {
            if(unitId != null)
            {
                PagedResult<Unit> results = await _unitManager.QueryAsync(new UnitQuery() { Id = unitId });

                if(results.FilteredCount > 0)
                {
                    Unit selectedUnit = results.First();
                    selectedUnit.Name = unitName;

                    await _unitManager.UpdateAsync(selectedUnit);
                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }

        /// <summary>
        /// Endpoint to update settings for the Unit
        /// </summary>
        /// <params name="model"></params>
        /// <returns></returns>
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> UpdateUnit(UnitViewModel model)
        {
            if(!model.Id.HasValue)
            {
                PagedResult<Unit> result = await _unitManager.QueryAsync(new UnitQuery() { Id = model.Id });

                if(result.FilteredCount > 0) {
                    result[0].Name = model.Name;
                    result[0].Modified = DateTime.Now;

                    await _unitManager.UpdateAsync(result[0]);
                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("start-acquisition/{unitId}")]
        public async Task<ActionResult> StartAcquisition(Guid unitId)
        {
            if(unitId != null)
            {
                PagedResult<Unit> result = await _unitManager.QueryAsync(new UnitQuery() { Id = unitId });

                if(result.FilteredCount > 0) {

                    Unit currentUnit = result.First();

                    try {
                    await _acquisitionService.StartAcquisition(currentUnit.Id);
                    } catch {}

                    await _unitManager.SendCommand(currentUnit.ConnectionId, "START");
                    currentUnit.IsAcquisitioning = true;

                    await _unitManager.UpdateAsync(currentUnit);
                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("stop-acquisition/{unitId}")]
        public async Task<ActionResult> StopAcquisition(Guid unitId)
        {
            if(unitId != null)
            {
                PagedResult<Unit> result = await _unitManager.QueryAsync(new UnitQuery() { Id = unitId });

                if(result.FilteredCount > 0) {
                    Unit currentUnit = result.First();

                    try {
                    await _acquisitionService.StopAcquisition(currentUnit.Id);
                    } catch {}

                    await _unitManager.SendCommand(currentUnit.ConnectionId, "STOP");

                    currentUnit.IsAcquisitioning = false;
                    await _unitManager.UpdateAsync(currentUnit);

                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("activate-humidity/{unitId}")]
        public async Task<ActionResult> ActivateHumidity(Guid unitId)
        {
            if(unitId != null)
            {
                PagedResult<Unit> result = await _unitManager.QueryAsync(new UnitQuery() { Id = unitId });

                if(result.FilteredCount > 0) 
                {
                    await _unitManager.SendCommand(result.First().ConnectionId, "humidity-start");
                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("deactivate-humidity/{unitId}")]
        public async Task<ActionResult> DeactivateHumidity(Guid unitId)
        {
            if(unitId != null)
            {
                PagedResult<Unit> result = await _unitManager.QueryAsync(new UnitQuery() { Id = unitId });

                if(result.FilteredCount > 0) 
                {
                    await _unitManager.SendCommand(result.First().ConnectionId, "humidity-stop");
                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("is-acquisitioning/{unitId}")]
        public async Task<ActionResult<bool>> IsAcquisitioning(Guid unitId)
        {
            if(unitId != null) {
                PagedResult<Unit> result = await _unitManager.QueryAsync(new UnitQuery() { Id = unitId });

                if(result.FilteredCount > 0) {
                    return result.First().IsAcquisitioning;
                }

                return NotFound(false);
            }

            return BadRequest(false);
        }

        #endregion NEEDED_FOR_DEMONSTRATION
    }
}
