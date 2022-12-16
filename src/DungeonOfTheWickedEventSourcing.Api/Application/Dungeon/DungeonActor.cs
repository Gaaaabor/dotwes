using Akka.Actor;
using Akka.Event;
using DungeonOfTheWickedEventSourcing.Api.Application.Dungeon.Commands;
using DungeonOfTheWickedEventSourcing.Api.Application.Dungeon.Events;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Commands;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Events;
using DungeonOfTheWickedEventSourcing.Api.Application.Enemy;
using DungeonOfTheWickedEventSourcing.Api.Application.Loot;
using DungeonOfTheWickedEventSourcing.Common;
using System.Drawing;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Dungeon
{
    public class DungeonActor : InjectionReceiveActorBase<DungeonActor>
    {
        private readonly Dictionary<string, IActorRef> _loots = new Dictionary<string, IActorRef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, IActorRef> _enemies = new Dictionary<string, IActorRef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, IActorRef> _floors = new Dictionary<string, IActorRef>(StringComparer.OrdinalIgnoreCase);
        private readonly Guid _dungeonId;
        private Size _size;

        public DungeonActor(Guid dungeonId, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Context.System.EventStream.Subscribe<IDungeonCommand>(Self);

            Receive<GenerateDungeonCommand>(OnGenerateCommand);
            _dungeonId = dungeonId;
        }

        private void OnGenerateCommand(GenerateDungeonCommand generateCommand)
        {
            GenerateDimensions();
            GenerateLoot();
            GenerateEnemies();
            GenerateFloors();

            Become(Undiscovered);

            Sender.Tell(new DungeonGeneratedEvent
            {
                DungeonId = _dungeonId,
                Size = _size
            });
        }

        #region //Generation logic (This part should be moved to a DungeonBuilder class later)

        private void GenerateDimensions()
        {
            var width = Random.Shared.Next(5, 101);
            var height = Random.Shared.Next(5, 101);

            _size = new Size(width, height);
        }

        private void GenerateLoot()
        {
            var name = $"loot-{Guid.NewGuid():N}";
            var loot = CreateChildActor<LootActor>(name);
            _loots.Add(name, loot);
        }

        private void GenerateEnemies()
        {
            var name = $"enemy-{Guid.NewGuid():N}";
            var enemy = CreateChildActor<EnemyActor>(name);
            _enemies.Add(name, enemy);
        }

        private void GenerateFloors()
        {
            var floorId = Guid.NewGuid();
            var floorName = $"floor-{floorId:N}";
            var floor = CreateChildActor<DungeonActor>(name: floorName, floorId);
            _floors.Add(floorName, floor);
        }

        #endregion //Generation logic

        #region //States

        private void Undiscovered()
        {
            Receive<DiscoverDungeonCommand>(x =>
            {                
                Become(Discovered);
                Sender.Tell(new DungeonDiscoveredEvent());
            });
        }

        private void Discovered()
        {
            Receive<ExploreDungeonCommand>(OnExploreDungeonCommand);
        }

        private void Cleared()
        { }

        #endregion //States

        private void OnExploreDungeonCommand(ExploreDungeonCommand exploreDungeonCommand)
        {
            Sender.Tell(new DungeonExploredEvent(_size, _enemies.Keys, _loots.Keys, _floors.Keys));
        }
    }
}
