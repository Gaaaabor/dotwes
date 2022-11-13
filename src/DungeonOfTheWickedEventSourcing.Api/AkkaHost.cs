using Akka.Actor;
using DungeonOfTheWickedEventSourcing.Api.Application;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Commands;
using DungeonOfTheWickedEventSourcing.Common.Akka;

namespace DungeonOfTheWickedEventSourcing.Api
{
    public class AkkaHost : AkkaHostServiceBase, IActorSystem
    {
        private IActorRef _dungeonGuardian;

        public AkkaHost(IServiceProvider serviceProvider) : base(serviceProvider)
        { }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            _dungeonGuardian = CreateChildActor<DungeonGuardianActor>(DungeonGuardianActor.ActorName);
        }

        public Task<string> GenerateDungeonAsync()
        {
            return _dungeonGuardian.Ask<string>(new GenerateCommand());
        }
    }
}
