using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mimo.Common.IManagers;
using System.Net;

namespace Mimo.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AchievementsController : ControllerBase
    {
        private readonly ILogger<AchievementsController> _logger;
        private readonly IResultsManager _resultsManager;

        public AchievementsController(ILogger<AchievementsController> logger, IResultsManager resultsManager)
        {
            _logger = logger;
            _resultsManager = resultsManager;
        }

        [HttpGet]
        [Route("{userGuid}")]
        public IActionResult GetUserAchievements(string userGuid)
        {
            var model = _resultsManager.GetUserAchievement(userGuid);
            if (!model.Success)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, model.Message);
            }
            return Ok(model.Model);
        }
    }
}
