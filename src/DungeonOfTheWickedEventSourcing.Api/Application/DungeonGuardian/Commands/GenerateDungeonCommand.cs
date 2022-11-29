namespace DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Commands
{
    public class GenerateDungeonCommand : IDungeonGuardianCommand
    {
        public Guid ConnectionId { get; set; }
    }
}
