using DungeonOfTheWickedEventSourcing.Api.Application.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace DungeonOfTheWickedEventSourcing.Api.Akka.Hubs
{
    public class MainHub : Hub
    {
        public const string Path = $"/{nameof(MainHub)}";
        private readonly ISignalRProcessor _signalRProcessor;

        public MainHub(ISignalRProcessor signalRProcessor)
        {
            _signalRProcessor = signalRProcessor;
        }

        public void OnClientMessage(string message)
        {
            _signalRProcessor.Process(message);
        }
    }
}
