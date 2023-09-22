namespace DungeonOfTheWickedEventSourcing.Api.Application.Diagnostics.Events
{
    public class QueryActorsFinishedEvent
    {
        public Dictionary<long, ActorNode> Actors { get; init; }
    }
}
