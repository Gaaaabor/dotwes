namespace DungeonOfTheWickedEventSourcing.Common.Actors.SignalR.Events
{
    public class EdgesUpdatedEvent : SignalREventBase
    {
        public long ReceiverId { get; init; }
        public long SenderId { get; init; }
    }
}
