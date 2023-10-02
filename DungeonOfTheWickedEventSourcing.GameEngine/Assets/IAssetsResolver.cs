using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Assets
{
    public interface IAssetsResolver
    {
        ValueTask<TAsset> Load<TAsset>(string path) where TAsset : IAsset;
        TAsset Get<TAsset>(string path) where TAsset : class, IAsset;
    }
}