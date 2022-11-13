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

        // TODO: Route to Akka OR use its "own" TCP messaging...
        //[Route("/ws")]
        //public async Task Get()
        //{
        //    if (HttpContext.WebSockets.IsWebSocketRequest)
        //    {
        //        using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

        //        var buffer = new byte[1024 * 4];
        //        var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //        var rawMessage = new ArraySegment<byte>(buffer, 0, receiveResult.Count);
        //        var message = Encoding.UTF8.GetString(rawMessage);
        //    }
        //    else
        //    {
        //        HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        //    }
        //}
    }
}