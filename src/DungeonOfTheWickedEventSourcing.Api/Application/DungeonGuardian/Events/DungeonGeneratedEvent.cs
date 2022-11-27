using System.Drawing;

namespace DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Events
{
    public class DungeonGeneratedEvent : IDungeonEvent
    {
        public Guid DungeonId { get; internal set; }
        public Size Size { get; internal set; }
    }
}
