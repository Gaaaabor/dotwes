namespace DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Events
{
    public class ActorStartedEvent : ActorDiagnosticEvent
    {
        public string Name { get; init; }
    }
}
