using Akka.Actor;
using DungeonOfTheWickedEventSourcing.Api.Hubs;
using DungeonOfTheWickedEventSourcing.Common;
using Microsoft.AspNetCore.SignalR;

namespace DungeonOfTheWickedEventSourcing.Api.Application.SignalR
{
    public class SignalRActor : InjectionReceiveActorBase<SignalRActor>, IWithTimers
    {
        public const string ActorName = "signalr";

        private readonly IHubContext<MainHub> _hubContext;

        public ITimerScheduler Timers { get; set; }

        public SignalRActor(IServiceProvider serviceProvider, IHubContext<MainHub> hubContext) : base(serviceProvider)
        {
            _hubContext = hubContext;

            Timers.StartPeriodicTimer("MessageDummy", 10, TimeSpan.FromSeconds(1));
            ReceiveAsync<string>(OnStringReceivedAsync);
            ReceiveAsync<int>(async x =>
            {
                await _hubContext.Clients.All.SendAsync("OnMessage", $"Message {DateTime.Now}");
            });
        }

        private async Task OnStringReceivedAsync(string message)
        {
            Logger.LogInformation("Received: {0}", message);
        }
    }
}
