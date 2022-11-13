using Akka.Actor;
using DungeonOfTheWickedEventSourcing.Api.Application.Dungeon;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Commands;
using DungeonOfTheWickedEventSourcing.Common.Akka;

namespace DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian
{
    public class DungeonGuardianActor : InjectionReceiveActorBase<DungeonGuardianActor>
    {
        public const string ActorName = "dungeonguardian";

        private readonly Dictionary<string, IActorRef> _dungeons = new Dictionary<string, IActorRef>(StringComparer.OrdinalIgnoreCase);

        public DungeonGuardianActor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Context.System.EventStream.Subscribe(Self, typeof(IDungeonGuardianCommand));

            Receive<GenerateDungeonCommand>(OnGenerateDungeonCommand);
        }

        private void OnGenerateDungeonCommand(GenerateDungeonCommand generateDungeonCommand)
        {
            var name = $"dungeon-{Guid.NewGuid():N}";
            var dungeon = CreateChildActor<DungeonActor>(name);
            dungeon.Forward(generateDungeonCommand);

            _dungeons.Add(name, dungeon);

            Sender.Tell(name);
        }
    }
}
