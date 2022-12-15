namespace DungeonOfTheWickedEventSourcing.Api.Application
{
    public interface IGraphDataProvider
    {
        string GetGraphFields();
        Task<string> GetGraphData();
    }
}
