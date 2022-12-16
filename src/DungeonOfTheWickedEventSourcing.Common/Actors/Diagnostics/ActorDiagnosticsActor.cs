using Akka.Event;
using DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Events;

namespace DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics
{
    public class ActorDiagnosticsActor : InjectionReceiveActorBase<ActorDiagnosticsActor>
    {
        private readonly Dictionary<long, ActorNode> _actorNodes = new();

        public ActorDiagnosticsActor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Context.System.EventStream.Subscribe<ActorDiagnosticEvent>(Self);
            Receive<ActorStartedEvent>(OnActorStartedEvent);
            Receive<ActorStoppedEvent>(OnActorStoppedEvent);
            Receive<ActorReceivedMessageEvent>(OnActorReceivedMessageEvent);
        }

        private void OnActorStartedEvent(ActorStartedEvent actorStartedEvent)
        {
            if (!_actorNodes.ContainsKey(actorStartedEvent.Id))
            {
                _actorNodes.Add(actorStartedEvent.Id, new ActorNode { });
            }
        }

        private void OnActorStoppedEvent(ActorStoppedEvent actorStoppedEvent)
        {
            _actorNodes.Remove(actorStoppedEvent.Id);
        }

        private void OnActorReceivedMessageEvent(ActorReceivedMessageEvent actorReceivedMessageEvent)
        {

        }
    }

    public class ActorNode
    {
        public string Name { get; init; }
    }
}
