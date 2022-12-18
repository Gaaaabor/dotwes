using Akka.Actor;
using Akka.Event;
using Akka.Util.Internal;
using DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Commands;
using DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Events;
using DungeonOfTheWickedEventSourcing.Common.Actors.SignalR.Events;

namespace DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics
{
    public class ActorDiagnosticsActor : InjectionReceiveActorBase<ActorDiagnosticsActor>
    {
        public const string ActorName = "actordiagnostics";

        private readonly Dictionary<long, ActorNode> _actorNodes = new();
        private readonly Dictionary<long, List<long>> _actorEdges = new();

        public ActorDiagnosticsActor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Context.System.EventStream.Subscribe<ActorDiagnosticEvent>(Self);
            Context.System.EventStream.Subscribe<ActorsRequestedCommand>(Self);

            Receive<ActorStartedEvent>(OnActorStartedEvent);
            Receive<ActorStoppedEvent>(OnActorStoppedEvent);
            Receive<ActorReceivedMessageEvent>(OnActorReceivedMessageEvent);
            Receive<ActorsRequestedCommand>(OnActorsRequestedCommand);
        }

        private void OnActorStartedEvent(ActorStartedEvent actorStartedEvent)
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

                Context.System.EventStream.Publish(new ActorsUpdatedEvent
                {
                    Depth = actorStartedEvent.Depth,
                    Added = actorStartedEvent.Id,
                    Name = actorStartedEvent.Name
                });
            }
        }

        private void OnActorStoppedEvent(ActorStoppedEvent actorStoppedEvent)
        {
            _actorNodes.Remove(actorStoppedEvent.Id);
            _actorEdges.Remove(actorStoppedEvent.Id);

            _actorEdges.Values
                .Where(x => x.Contains(actorStoppedEvent.Id))
                .ForEach(x => x.Remove(actorStoppedEvent.Id));

            Context.System.EventStream.Publish(new ActorsUpdatedEvent
            {
                Removed = actorStoppedEvent.Id
            });
        }

        private void OnActorReceivedMessageEvent(ActorReceivedMessageEvent actorReceivedMessageEvent)
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

            Context.System.EventStream.Publish(new EdgesUpdatedEvent
            {
                ReceiverId = actorReceivedMessageEvent.Id,
                SenderId = actorReceivedMessageEvent.SenderId
            });
        }

        private void OnActorsRequestedCommand(ActorsRequestedCommand actorsRequestedCommand)
        {
            Sender.Tell(new ActorsRequestedCommandResponse
            {
                Actors = _actorNodes.ToDictionary(x => x.Key, y => y.Value.Name)
            });
        }
    }

    public class ActorNode
    {
        public int Depth { get; init; }
        public string Name { get; init; }
    }
}
