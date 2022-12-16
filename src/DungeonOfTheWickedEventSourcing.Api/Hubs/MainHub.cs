using DungeonOfTheWickedEventSourcing.Api.Application;
using Microsoft.AspNetCore.SignalR;

namespace DungeonOfTheWickedEventSourcing.Api.Hubs
{
    public class MainHub : Hub
    {
        public const string Path = $"/{nameof(MainHub)}";
        private readonly ISignalRProcessor _signalRProcessor;

        public MainHub(ISignalRProcessor signalRProcessor)
        {
            _signalRProcessor = signalRProcessor;
        }

        public void Test(string message)
        {
            _signalRProcessor.Process(message);
        }
    }
}
