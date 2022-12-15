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
    public class RunsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UnitManager _unitManager;
        private readonly RunsManager _runsManager;
        private readonly AcquisitionService _acquisitionService;

        public RunsController(ILogger<UsersController> logger, UnitManager unitManager, RunsManager runsManager, AcquisitionService acquisitionService)
        {
            _logger = logger;
            _unitManager = unitManager;
            _runsManager = runsManager;
            _acquisitionService = acquisitionService;
        }


        #region NEEDED_FOR_DEMONSTRATION

        [HttpPut]
        [Route("start-acquisition/{unitId}")]
        public async Task<ActionResult<string>> StartAcquisition(Guid unitId)
        {
            if(unitId != null)
            {
                PagedResult<Unit> result = await _unitManager.QueryAsync(new UnitQuery() { Id = unitId });

                if(result.FilteredCount > 0) {

                    Unit currentUnit = result.First();
                    DateTime? startTime = DateTime.Now;

                    Guid newRunId = Guid.NewGuid();
                    Run newRun = new Run() 
                    {
                        Id = newRunId,
                        StartTime = startTime,
                        AssociatedUnit = currentUnit.Id,
                        Modified = DateTime.Now
                    };

                    await _runsManager.CreateAsync(newRun);

                    try {
                    startTime = await _acquisitionService.StartAcquisition(currentUnit.Id, newRunId);
                    } catch {}

                    // await _unitManager.SendCommand(currentUnit.ConnectionId, "START");
                    currentUnit.IsAcquisitioning = true;
                    currentUnit.Modified = DateTime.Now;

                    await _unitManager.UpdateAsync(currentUnit);

                    if(startTime.HasValue)
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(startTime.Value));

                    return Ok(null);
                }

                return NotFound(null);
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
        [Route("acquisition-start-time/{unitId}")]
        public async Task<ActionResult<string>> AcquisitionStartTime(Guid unitId)
        {
            if(unitId != null) {
                PagedResult<Unit> result = await _unitManager.QueryAsync(new UnitQuery() { Id = unitId });

                if(result.FilteredCount > 0) {
                    DateTime? startTime = await _acquisitionService.GetStartTime(result.First().Id);

                    if(startTime != null) {
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(startTime.Value));
                    }

                    return Ok(null);
                }

                return NotFound(null);
            }

            return BadRequest();
        }

        #endregion NEEDED_FOR_DEMONSTRATION
    }
}
