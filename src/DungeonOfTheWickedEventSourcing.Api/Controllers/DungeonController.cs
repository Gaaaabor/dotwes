using Microsoft.AspNetCore.Mvc;

namespace DungeonOfTheWickedEventSourcing.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DungeonController : ControllerBase
    {
        private readonly ILogger<DungeonController> _logger;

        public DungeonController(ILogger<DungeonController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public ActionResult Get()
        {
            _logger.LogInformation("Called Dungeon/Get");
            return Ok("Welcome!");
        }
    }
}