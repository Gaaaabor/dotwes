using DungeonOfTheWickedEventSourcing.Common.Actors.ActorWalker.Models;
using DungeonOfTheWickedEventSourcing.Common.Events;

namespace DungeonOfTheWickedEventSourcing.Common.Actors.ActorWalker.Events
{
    public class HierarchyDiscoveredEvent : IClientNotification
    {
        public RootNode Root { get; init; }
    }
}
