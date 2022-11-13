namespace DungeonOfTheWickedEventSourcing.Api.Application
{
    public interface IActorSystem
    {
        Task<string> GenerateDungeonAsync();
    }
}
