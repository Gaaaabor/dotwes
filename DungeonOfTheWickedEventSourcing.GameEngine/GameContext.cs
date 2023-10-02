using DungeonOfTheWickedEventSourcing.GameEngine.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DungeonOfTheWickedEventSourcing.GameEngine
{
    public abstract class GameContext
    {
        private bool _isInitialized = false;
        private readonly Dictionary<Type, IGameService> _services = new();

        public GameTime GameTime { get; } = new();
        public Display Display { get; } = new();

        protected void AddService(IGameService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            _services[service.GetType()] = service;
        }

        protected abstract ValueTask Init();

        public TService GetService<TService>() where TService : class, IGameService
        {
            return _services.TryGetValue(typeof(TService), out var service)
                ? service as TService
                : null;
        }

        public async ValueTask Step()
        {
            if (!_isInitialized)
            {
                await Init();

                GameTime.Start();

                _isInitialized = true;
            }

            GameTime.Step();

            foreach (var service in _services.Values)
            {
                await service.Step();
            }
        }
    }
}