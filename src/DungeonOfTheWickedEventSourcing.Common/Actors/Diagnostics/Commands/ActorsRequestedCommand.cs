namespace DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Commands
{
    public class ActorsRequestedCommand
    { }

    public class ActorsRequestedCommandResponse
    {
        public Dictionary<long, string> Actors { get; init; }
    }
}
