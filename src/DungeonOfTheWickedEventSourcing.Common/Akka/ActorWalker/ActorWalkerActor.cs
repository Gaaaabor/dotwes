using Akka.Actor;
using Akka.Event;
using DungeonOfTheWickedEventSourcing.Common.Akka.ActorWalker.Commands;
using DungeonOfTheWickedEventSourcing.Common.Akka.ActorWalker.Events;

namespace DungeonOfTheWickedEventSourcing.Common.Akka.ActorWalker
{
    public class ActorWalkerActor : InjectionReceiveActorBase<ActorWalkerActor>
    {
        public const string ActorName = "actorwalker";

        public ActorWalkerActor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Context.System.EventStream.Subscribe<DiscoverHierarchyCommand>(Self);

            Receive<DiscoverHierarchyCommand>(OnDiscoverHierarchy);
        }

        private void OnDiscoverHierarchy(DiscoverHierarchyCommand discoverHierarchy)
        {
            var actorVisitor = new ActorVisitor();
            var rootNode = actorVisitor.Visit(Context.System);

            Sender.Tell(new HierarchyDiscoveredEvent
            {
                Root = rootNode
            });
        }
    }
}
