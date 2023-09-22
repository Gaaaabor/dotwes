namespace DungeonOfTheWickedEventSourcing.Api.Application.Diagnostics.Events
{
    public class ActorStartedEvent : ActorDiagnosticEvent
    {
        public string Name { get; init; }
        public int Depth { get; init; }
    }
}
