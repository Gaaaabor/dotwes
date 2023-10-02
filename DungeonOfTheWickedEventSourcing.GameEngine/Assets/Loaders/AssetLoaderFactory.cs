using System;
using System.Collections.Generic;

namespace DungeonOfTheWickedEventSourcing.GameEngine.Assets.Loaders
{
    public class AssetLoaderFactory : IAssetLoaderFactory
    {
        private readonly IDictionary<Type, object> _assetLoaders = new Dictionary<Type, object>();

        public void Register<TAsset>(IAssetLoader<TAsset> assetLoader) where TAsset : IAsset
        {
            var assetType = typeof(TAsset);
            if (!_assetLoaders.ContainsKey(assetType))
            {
                _assetLoaders.Add(assetType, null);
            }

            _assetLoaders[assetType] = assetLoader;
        }

        public IAssetLoader<TAsset> Get<TAsset>() where TAsset : IAsset
        {
            var assetType = typeof(TAsset);
            if (!_assetLoaders.ContainsKey(assetType))
            {
                throw new ArgumentOutOfRangeException($"Invalid asset type: {assetType.FullName}");
            }

            return _assetLoaders[assetType] as IAssetLoader<TAsset>;
        }
    }
}