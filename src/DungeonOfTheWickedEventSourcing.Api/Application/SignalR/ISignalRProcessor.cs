namespace DungeonOfTheWickedEventSourcing.Api.Application.SignalR
{
    public interface ISignalRProcessor
    {
        void Process(string message);
    }
}
