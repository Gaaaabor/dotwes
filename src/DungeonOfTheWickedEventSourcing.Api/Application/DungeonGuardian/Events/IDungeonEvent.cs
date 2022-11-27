using DungeonOfTheWickedEventSourcing.Common.Akka.Events;

namespace DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Events
{
    public interface IDungeonEvent : IClientNotification
    {
        public Guid DungeonId { get; }
    }
}
