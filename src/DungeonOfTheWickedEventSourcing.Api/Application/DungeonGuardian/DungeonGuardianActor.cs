using Akka.Actor;
using Akka.Event;
using DungeonOfTheWickedEventSourcing.Api.Application.Dungeon;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Commands;
using DungeonOfTheWickedEventSourcing.Common;

namespace DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian
{
    public class DungeonGuardianActor : InjectionReceiveActorBase<DungeonGuardianActor>
    {
        public const string ActorName = "dungeonguardian";

        private readonly Dictionary<Guid, IActorRef> _dungeons = new Dictionary<Guid, IActorRef>();

        public DungeonGuardianActor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Context.System.EventStream.Subscribe<IDungeonGuardianCommand>(Self);

            Receive<GenerateDungeonCommand>(OnGenerateDungeonCommand);            
        }

        private void OnGenerateDungeonCommand(GenerateDungeonCommand generateDungeonCommand)
        {
            var dungeonId = Guid.NewGuid();
            var dungeon = CreateChildActor<DungeonActor>(name: $"dungeon-{dungeonId:N}", dungeonId);
            dungeon.Forward(generateDungeonCommand);

            _dungeons.Add(dungeonId, dungeon);
        }
    }
}
