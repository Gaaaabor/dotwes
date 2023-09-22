using Akka.Util.Internal;
using DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Commands;
using DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Events;
using DungeonOfTheWickedEventSourcing.Common.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics
{
    public class ActorDiagnosticsActor : InjectionReceiveActorBase<ActorDiagnosticsActor>
    {
        public const string ActorName = "actordiagnostics";

        private readonly Dictionary<long, ActorNode> _actorNodes = new();
        private readonly Dictionary<long, List<long>> _actorEdges = new();
        private readonly IHubContext<MainHub> _hubContext;

        public ActorDiagnosticsActor(IServiceProvider serviceProvider, IHubContext<MainHub> hubContext) : base(serviceProvider)
        {
            _hubContext = hubContext;

            ReceiveAsync<ActorStartedEvent>(OnActorStartedEventAsync);
            ReceiveAsync<ActorStoppedEvent>(OnActorStoppedEventAsync);
            ReceiveAsync<ActorReceivedMessageEvent>(OnActorReceivedMessageEventAsync);

            ReceiveAsync<QueryActorsCommand>(OnQueryActorsCommandAsync);
        }

        private async Task OnActorStartedEventAsync(ActorStartedEvent actorStartedEvent)
        {
            if (!_actorNodes.ContainsKey(actorStartedEvent.Id))
            {
                _actorNodes.Add(actorStartedEvent.Id, new ActorNode
                {
                    Depth = actorStartedEvent.Depth,
                    Name = actorStartedEvent.Name
                });

                if (!_actorEdges.ContainsKey(actorStartedEvent.Id))
                {
                    _actorEdges.Add(actorStartedEvent.Id, new List<long>());
                }

                await NotifyClientsAsync(actorStartedEvent);
            }
        }

        private async Task OnActorStoppedEventAsync(ActorStoppedEvent actorStoppedEvent)
        {
            _actorNodes.Remove(actorStoppedEvent.Id);
            _actorEdges.Remove(actorStoppedEvent.Id);

            _actorEdges.Values
                .Where(x => x.Contains(actorStoppedEvent.Id))
                .ForEach(x => x.Remove(actorStoppedEvent.Id));

            await NotifyClientsAsync(actorStoppedEvent);
        }

        private async Task OnActorReceivedMessageEventAsync(ActorReceivedMessageEvent actorReceivedMessageEvent)
        {
            if (!_actorEdges.TryGetValue(actorReceivedMessageEvent.Id, out var edges))
            {
                edges = new List<long>();
                _actorEdges.Add(actorReceivedMessageEvent.Id, edges);
            }

            if (!edges.Contains(actorReceivedMessageEvent.SenderId))
            {
                edges.Add(actorReceivedMessageEvent.SenderId);
            }

            await NotifyClientsAsync(actorReceivedMessageEvent);
        }

        private async Task OnQueryActorsCommandAsync(QueryActorsCommand actorsRequestedCommand)
        {
            await NotifyClientsAsync(new QueryActorsFinishedEvent
            {
                Actors = _actorNodes
            });
        }

        private async Task NotifyClientsAsync<TMessage>(TMessage message)
        {
            await _hubContext.Clients.All.SendAsync($"On{typeof(TMessage).Name}", message);
        }
    }
}
