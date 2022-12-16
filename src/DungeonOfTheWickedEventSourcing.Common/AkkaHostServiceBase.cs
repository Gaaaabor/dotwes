using Akka.Actor;
using Akka.DependencyInjection;
using DungeonOfTheWickedEventSourcing.Common.Actors.ActorWalker;
using DungeonOfTheWickedEventSourcing.Common.Configuration;
using DungeonOfTheWickedEventSourcing.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DungeonOfTheWickedEventSourcing.Common
{
    public abstract class AkkaHostServiceBase : IHostedService
    {
        public const string ActorSystemName = "dotwes";
        private const char Separator = ';';
        private readonly ILogger _logger;
        private DependencyResolver _dependencyResolver;

        protected IServiceProvider ServiceProvider { get; }
        protected ActorSystem ActorSystem { get; private set; }
        protected IActorRef Mediator { get; private set; }

        protected IActorRef ActorWalker { get; private set; }

        public AkkaHostServiceBase(IServiceProvider serviceProvider, ILogger logger)
        {
            ServiceProvider = serviceProvider;
            _logger = logger;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("AkkaHostServiceBase initializing...");

            ActorSystem = CreateActorSystem();
            _dependencyResolver = DependencyResolver.For(ActorSystem);
            ActorWalker = CreateChildActor<ActorWalkerActor>(ActorWalkerActor.ActorName);

            Mediator = ActorSystem.GetMediator(_logger);

            _logger.LogInformation("AkkaHostServiceBase successfully started!");
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("AkkaHostServiceBase stopping...");

            // strictly speaking this may not be necessary - terminating the ActorSystem would also work
            // but this call guarantees that the shutdown of the cluster is graceful regardless            
            await CoordinatedShutdown.Get(ActorSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }

        protected virtual ActorSystem CreateActorSystem()
        {
            var configuration = ServiceProvider.GetService<IConfiguration>();
            var connectionString = configuration.GetValue<string>(AkkaConfiguration.ConnectionString);
            var roles = configuration.GetValue<string>(AkkaConfiguration.Roles)?.Split(Separator);
            var seedNodeHostNames = configuration.GetValue<string>(AkkaConfiguration.SeedNodeHostNames)?.Split(Separator);
            var port = configuration.GetValue(AkkaConfiguration.Port, 0);
            
            var config = AkkaConfiguration.GetRedisPersistenceConfiguration(connectionString)
                .WithFallback(AkkaConfiguration.GetClusterConfiguration(ActorSystemName, port, roles, seedNodeHostNames))
                .WithFallback(@"tracedmailbox {
mailbox-type : ""DungeonOfTheWickedEventSourcing.Common.Mailbox.TracedMailbox, DungeonOfTheWickedEventSourcing.Common""
}");

            var bootstrapSetup = BootstrapSetup
                .Create()
                .WithConfig(config);

            var dependencyResolverSetup = DependencyResolverSetup.Create(ServiceProvider);
            var actorSystemSetup = bootstrapSetup.And(dependencyResolverSetup);
            var actorSystem = ActorSystem.Create(ActorSystemName, actorSystemSetup);

            return actorSystem;
        }

        /// <summary>
        /// This method create an actor directly under the actorsystem, Do not use it in constructor!
        /// </summary>
        /// <typeparam name="TActor"></typeparam>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual IActorRef CreateChildActor<TActor>(string name, params object[] args) where TActor : ActorBase
        {
            var props = _dependencyResolver.Props<TActor>(args).WithMailbox("tracedmailbox");
            return ActorSystem.ActorOf(props, name);
        }
    }
}
