using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AgerDevice.Redis;

namespace AgerDeviceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private ILogger _logger;

        public SettingsController(ILogger<SettingsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Test API Endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/test")]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
