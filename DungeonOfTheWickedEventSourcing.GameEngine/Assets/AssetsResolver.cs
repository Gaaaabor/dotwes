using DungeonOfTheWickedEventSourcing.GameEngine.Assets.Loaders;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Assets
{
    public class AssetsResolver : IAssetsResolver
    {
        private readonly ConcurrentDictionary<string, IAsset> _assets;
        private readonly IAssetLoaderFactory _assetLoaderFactory;

        public AssetsResolver(IAssetLoaderFactory assetLoaderFactory)
        {
            _assetLoaderFactory = assetLoaderFactory;
            _assets = new ConcurrentDictionary<string, IAsset>();
        }

        public async ValueTask<TAsset> Load<TAsset>(string path) where TAsset : IAsset
        {
            var loader = _assetLoaderFactory.Get<TAsset>();
            var asset = await loader.Load(path);

            if (null == asset)
            {
                throw new TypeLoadException($"unable to load asset type '{typeof(TAsset)}' from path '{path}'");
            }

            _assets.AddOrUpdate(path, k => asset, (k, v) => asset);
            return asset;
        }

        public TAsset Get<TAsset>(string path) where TAsset : class, IAsset
        {
            return _assets[path] as TAsset;
        }
    }
}