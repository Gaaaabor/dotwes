namespace DungeonOfTheWickedEventSourcing.Api.Application
{
    public interface IDungeonWideCommand
    {
        public Guid ConnectionId { get; set; }
    }
}
