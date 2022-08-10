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

        public UnitController(ILogger<UsersController> logger, UnitManager unitManager)
        {
            _logger = logger;
            _unitManager = unitManager;
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
