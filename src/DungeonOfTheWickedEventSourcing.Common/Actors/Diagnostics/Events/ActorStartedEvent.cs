namespace DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Events
{
    public class ActorStartedEvent : ActorDiagnosticEvent
    {
        public int Depth { get; init; }
        public string Name { get; init; }
    }
}
