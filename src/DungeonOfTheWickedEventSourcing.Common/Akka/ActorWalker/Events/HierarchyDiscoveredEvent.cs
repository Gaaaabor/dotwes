using DungeonOfTheWickedEventSourcing.Common.Akka.ActorWalker.Models;
using DungeonOfTheWickedEventSourcing.Common.Akka.Events;

namespace DungeonOfTheWickedEventSourcing.Common.Akka.ActorWalker.Events
{
    public class HierarchyDiscoveredEvent : IClientNotification
    {
        public RootNode Root { get; init; }
    }
}
