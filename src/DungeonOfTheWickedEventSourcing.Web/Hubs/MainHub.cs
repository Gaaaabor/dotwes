using Microsoft.AspNetCore.SignalR;

namespace DungeonOfTheWickedEventSourcing.Web.Hubs
{
    public class MainHub : Hub
    {
        public const string Path = $"/{nameof(MainHub)}";
    }
}
