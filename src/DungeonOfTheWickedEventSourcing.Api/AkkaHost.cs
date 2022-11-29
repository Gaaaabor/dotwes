using DungeonOfTheWickedEventSourcing.Api.Application.Connection;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian;
using DungeonOfTheWickedEventSourcing.Common.Akka;

namespace DungeonOfTheWickedEventSourcing.Api
{
    public class AkkaHost : AkkaHostServiceBase
    {
        public AkkaHost(IServiceProvider serviceProvider) : base(serviceProvider)
        { }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            CreateChildActor<DungeonGuardianActor>(DungeonGuardianActor.ActorName);

            var configuration = ServiceProvider.GetService<IConfiguration>();
            var clientConnectionPort = configuration.GetValue(AkkaConfiguration.ClientConnectionPort, 0);

            CreateChildActor<ClientConnectionManagerActor>(name: ClientConnectionManagerActor.ActorName, clientConnectionPort);
        }
    }
}
