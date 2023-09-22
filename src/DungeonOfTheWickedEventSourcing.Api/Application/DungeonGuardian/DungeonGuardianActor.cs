using Akka.Actor;
using DungeonOfTheWickedEventSourcing.Api.Akka.Base;
using DungeonOfTheWickedEventSourcing.Api.Application.Dungeon;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Commands;

namespace DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian
{
    public class DungeonGuardianActor : InjectionReceiveActorBase<DungeonGuardianActor>
    {
        public const string ActorName = "dungeonguardian";

        public DungeonGuardianActor()
        {
            Subscribe<IDungeonGuardianCommand>();

            Receive<GenerateDungeonCommand>(OnGenerateDungeonCommand);
        }

        private void OnGenerateDungeonCommand(GenerateDungeonCommand generateDungeonCommand)
        {
            var dungeonId = Guid.NewGuid();
            var dungeon = CreateChildActor<DungeonActor>(name: $"dungeon-{dungeonId:N}", dungeonId);
            dungeon.Forward(generateDungeonCommand);
        }
    }
}
