namespace DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Events
{
    public class QueryActorsFinishedEvent
    {
        public Dictionary<long, ActorNode> Actors { get; init; }
    }
}
