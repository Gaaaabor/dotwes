using Akka.Actor;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Commands;
using DungeonOfTheWickedEventSourcing.Common.Akka;

namespace DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian
{
    public class DungeonGuardianActor : InjectionReceiveActorBase<DungeonGuardianActor>
    {
        public const string ActorName = "dungeonguardian";

        public DungeonGuardianActor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Receive<GenerateCommand>(OnGenerateCommand);
        }

        private void OnGenerateCommand(GenerateCommand generateCommand)
        {
            Sender.Tell("Dungeon generated!");
        }
    }
}
