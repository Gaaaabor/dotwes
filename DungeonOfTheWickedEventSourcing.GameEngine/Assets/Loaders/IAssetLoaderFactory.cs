namespace DungeonOfTheWickedEventSourcing.GameEngine.Assets.Loaders
{
    public interface IAssetLoaderFactory
    {
        IAssetLoader<TAsset> Get<TAsset>() where TAsset : IAsset;
    }
}