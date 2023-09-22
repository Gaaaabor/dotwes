namespace DungeonOfTheWickedEventSourcing.Api.Application.Diagnostics.Events
{
    public class ActorReceivedMessageEvent : ActorDiagnosticEvent
    {
        public long SenderId { get; init; }
    }
}
