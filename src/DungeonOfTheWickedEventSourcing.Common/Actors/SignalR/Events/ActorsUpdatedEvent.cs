namespace DungeonOfTheWickedEventSourcing.Common.Actors.SignalR.Events
{
    public class ActorsUpdatedEvent : SignalREventBase
    {
        public long Added { get; init; }
        public int Depth { get; init; }
        public string Name { get; init; }
        public long Removed { get; init; }
    }
}
