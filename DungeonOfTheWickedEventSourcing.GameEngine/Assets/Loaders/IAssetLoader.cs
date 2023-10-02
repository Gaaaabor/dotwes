using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Assets.Loaders
{
    public interface IAssetLoader<TAsset> where TAsset : IAsset
    {
        ValueTask<TAsset> Load(string path);
    }
}