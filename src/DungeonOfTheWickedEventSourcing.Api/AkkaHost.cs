using Akka.Actor;
using DungeonOfTheWickedEventSourcing.Api.Application.Connection;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian;
using DungeonOfTheWickedEventSourcing.Api.Application.PlayerGuardian;
using DungeonOfTheWickedEventSourcing.Common;
using DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics;
using DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Commands;
using DungeonOfTheWickedEventSourcing.Common.Actors.SignalR;
using DungeonOfTheWickedEventSourcing.Common.Configuration;

namespace DungeonOfTheWickedEventSourcing.Api
{
    public class AkkaHost : AkkaHostServiceBase, ISignalRProcessor
    {
        private IActorRef _actorDiagnosticsActor;

        public AkkaHost(IServiceProvider serviceProvider, ILogger<AkkaHost> logger) : base(serviceProvider, logger)
        { }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            CreateChildActor<DungeonGuardianActor>(DungeonGuardianActor.ActorName);
            CreateChildActor<PlayerGuardianActor>(PlayerGuardianActor.ActorName);

            var configuration = ServiceProvider.GetService<IConfiguration>();
            var clientConnectionPort = configuration.GetValue(AkkaConfiguration.ClientConnectionPort, 0);

            CreateChildActor<ClientConnectionManagerActor>(name: ClientConnectionManagerActor.ActorName, clientConnectionPort);
            _actorDiagnosticsActor = CreateChildActor<ActorDiagnosticsActor>(name: ActorDiagnosticsActor.ActorName);            
        }

        public void Process(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            if (string.Equals(message, typeof(QueryActorsCommand).Name, StringComparison.OrdinalIgnoreCase))
            {
                _actorDiagnosticsActor.Tell(new QueryActorsCommand());
            }
        }
    }
}
