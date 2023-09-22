namespace DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Events
{
    public class ActorReceivedMessageEvent : ActorDiagnosticEvent
    {
        public long SenderId { get; init; }
    }
}
