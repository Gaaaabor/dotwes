using Akka.Actor;
using Akka.Event;
using DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Commands;
using DungeonOfTheWickedEventSourcing.Common.Actors.SignalR.Events;
using DungeonOfTheWickedEventSourcing.Common.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DungeonOfTheWickedEventSourcing.Common.Actors.SignalR
{
    public class SignalRActor : InjectionReceiveActorBase<SignalRActor>, IWithTimers
    {
        public const string ActorName = "signalr";

        private readonly IHubContext<MainHub> _hubContext;

        public ITimerScheduler Timers { get; set; }

        public SignalRActor(IServiceProvider serviceProvider, IHubContext<MainHub> hubContext) : base(serviceProvider)
        {
            _hubContext = hubContext;

            Context.System.EventStream.Subscribe<SignalREventBase>(Self);
            Context.System.EventStream.Subscribe<ActorsRequestedCommandResponse>(Self);

            Receive<string>(OnClientMessageReceived);
            ReceiveAsync<ActorsUpdatedEvent>(OnActorsUpdatedEventAsync);
            ReceiveAsync<EdgesUpdatedEvent>(OnEdgesUpdatedEventAsync);
            ReceiveAsync<ActorsRequestedCommandResponse>(OnActorsRequestedCommandResponseAsync);
        }

        private void OnClientMessageReceived(string message)
        {
            Logger.LogInformation("Received: {0}", message);
            if (string.Equals(message, "Refresh", StringComparison.OrdinalIgnoreCase))
            {
                Context.System.EventStream.Publish(new ActorsRequestedCommand());
            }
        }

        private async Task OnActorsUpdatedEventAsync(ActorsUpdatedEvent actorsUpdatedEvent)
        {
            await _hubContext.Clients.All.SendAsync("OnActorsUpdatedEvent", actorsUpdatedEvent);
        }

        private async Task OnEdgesUpdatedEventAsync(EdgesUpdatedEvent edgesUpdatedEvent)
        {
            await _hubContext.Clients.All.SendAsync("OnEdgesUpdatedEvent", edgesUpdatedEvent);
        }

        private async Task OnActorsRequestedCommandResponseAsync(ActorsRequestedCommandResponse actorsRequestedCommandResponse)
        {
            await _hubContext.Clients.All.SendAsync("OnRefresh", actorsRequestedCommandResponse);
        }
    }
}
