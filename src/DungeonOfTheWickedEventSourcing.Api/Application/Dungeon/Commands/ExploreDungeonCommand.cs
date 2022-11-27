namespace DungeonOfTheWickedEventSourcing.Api.Application.Dungeon.Commands
{
    public class ExploreDungeonCommand : IDungeonCommand
    {
        public Guid ConnectionId { get; set; }
        public Guid DungeonId { get; set; }
        public string Name { get; init; }
    }
}
