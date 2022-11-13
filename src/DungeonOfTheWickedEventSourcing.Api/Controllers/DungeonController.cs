using DungeonOfTheWickedEventSourcing.Api.Application;
using Microsoft.AspNetCore.Mvc;

namespace DungeonOfTheWickedEventSourcing.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DungeonController : ControllerBase
    {
        private readonly ILogger<DungeonController> _logger;
        private readonly IActorSystem _actorSystem;

        public DungeonController(ILogger<DungeonController> logger, IActorSystem actorSystem)
        {
            _logger = logger;
            _actorSystem = actorSystem;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _actorSystem.GenerateDungeonAsync();
            return Ok(result);
        }
    }
}