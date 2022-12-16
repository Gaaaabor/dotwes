namespace DungeonOfTheWickedEventSourcing.Api.Application
{
    public interface ISignalRProcessor
    {
        void Process(string message);
    }
}
