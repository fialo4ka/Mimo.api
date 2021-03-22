using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mimo.Common.IManagers;
using System.Net;

namespace Mimo.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LessonsController : ControllerBase
    {
        private readonly ILogger<LessonsController> _logger;
        private readonly IResultsManager _resultsManager;

        public LessonsController(ILogger<LessonsController> logger, IResultsManager resultsManager)
        {
            _logger = logger;
            _resultsManager = resultsManager;
        }

        [HttpGet]
        [Route("progress/{userGuid}")]
        public IActionResult GetProgress(string userGuid)
        {
            var model = _resultsManager.GetCompletedLessons(userGuid);
            if (!model.Success)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, model.Message);
            }
            return Ok(model.Model);
        }
    }
}
