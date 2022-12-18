namespace DungeonOfTheWickedEventSourcing.Common.Actors.SignalR
{
    public interface ISignalRProcessor
    {
        void Process(string message);
    }
}
