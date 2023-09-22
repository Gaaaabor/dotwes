using Akka.Configuration;
using Akka.DependencyInjection;
using Akka.Hosting;
using Akka.Management;
using DungeonOfTheWickedEventSourcing.Api.Application.Connection;
using DungeonOfTheWickedEventSourcing.Api.Application.Diagnostics;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian;
using DungeonOfTheWickedEventSourcing.Api.Application.PlayerGuardian;
using DungeonOfTheWickedEventSourcing.Api.Application.SignalR;
using System.Text;

namespace DungeonOfTheWickedEventSourcing.Api.Akka.Configuration
{
    public static class AkkaConfiguration
    {
        public const string ConnectionString = nameof(ConnectionString);
        public const string Roles = nameof(Roles);
        public const string SeedNodeHostNames = nameof(SeedNodeHostNames);
        public const string Port = nameof(Port);
        public const string ClientConnectionPort = nameof(ClientConnectionPort);

        public static IServiceCollection ConfigureAkka(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var clientConnectionPort = configuration.GetValue(ClientConnectionPort, 0);

            serviceCollection.AddAkka<SignalRProcessor>("dotwes", (builder, _) =>
            {
                var connectionString = configuration.GetValue<string>(ConnectionString);

                var config = GetRedisPersistenceConfiguration(connectionString)
                    .WithFallback(@"tracedmailbox {
mailbox-type : ""DungeonOfTheWickedEventSourcing.Api.Akka.Mailbox.TracedMailbox, DungeonOfTheWickedEventSourcing.Api""
}");

                const int ManagementPort = 8558;
                //const string Role = "DungeonMaster";
                //const string TraefikEndpoint = "http://weather-traefik:8080";

                builder
                    .AddHocon(config, HoconAddMode.Append)
                    //.WithRemoting(hostname: Dns.GetHostName(), port: 8091)
                    //.WithClustering(new ClusterOptions { Roles = new[] { Role } })
                    //.WithClusterBootstrap(serviceName: "exampleservice")
                    .WithAkkaManagement(port: ManagementPort)
                    //.WithTraefikDiscovery(options =>
                    //{
                    //    options.Endpoint = TraefikEndpoint;
                    //    options.Ports = new List<int> { ManagementPort };
                    //    options.Filters = new List<Filter>
                    //    {
                    //        new Filter("type", "loadbalancer"),
                    //        new Filter("provider", "docker")
                    //    };
                    //})
                    //.WithShardRegion<SimpleShardRegion>(nameof(SimpleShardRegion), SimpleShardRegion.ActorFactory, new SimpleMessageExtractor(), new ShardOptions
                    //{
                    //    Role = Role
                    //})
                    .WithActors((actorSystem, actorRegistry) =>
                    {
                        var dependencyResolver = DependencyResolver.For(actorSystem);

                        actorSystem.ActorOf(dependencyResolver.Props<PlayerGuardianActor>(), PlayerGuardianActor.ActorName);
                        actorSystem.ActorOf(dependencyResolver.Props<DungeonGuardianActor>(), DungeonGuardianActor.ActorName);


                        actorSystem.ActorOf(dependencyResolver.Props<ClientConnectionManagerActor>(clientConnectionPort), ClientConnectionManagerActor.ActorName);
                        actorSystem.ActorOf(dependencyResolver.Props<ActorDiagnosticsActor>(), ActorDiagnosticsActor.ActorName);
                    });
            });

            return serviceCollection;
        }

        public static Config GetRedisPersistenceConfiguration(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return Config.Empty;
            }

            var builder = new StringBuilder();

            builder.AppendLine("akka.persistence.journal.plugin=\"akka.persistence.journal.redis\"");
            builder.AppendLine("akka.persistence.journal.redis.class=\"Akka.Persistence.Redis.Journal.RedisJournal, Akka.Persistence.Redis\"");
            builder.AppendLine($"akka.persistence.journal.redis.configuration-string={connectionString}");
            builder.AppendLine("akka.persistence.journal.redis.key-prefix=\"\"");

            builder.AppendLine("akka.persistence.snapshot-store.plugin=\"akka.persistence.snapshot-store.redis\"");
            builder.AppendLine("akka.persistence.snapshot-store.redis.class=\"Akka.Persistence.Redis.Snapshot.RedisSnapshotStore, Akka.Persistence.Redis\"");
            builder.AppendLine($"akka.persistence.snapshot-store.redis.configuration-string={connectionString}");
            builder.AppendLine("akka.persistence.snapshot-store.redis.key-prefix=\"\"");

            var config = ConfigurationFactory.ParseString(builder.ToString());
            return config;
        }
    }

    public class SignalRProcessor : AkkaHostedService, ISignalRProcessor
    {
        public SignalRProcessor(AkkaConfigurationBuilder configurationBuilder, IServiceProvider serviceProvider, ILogger<AkkaHostedService> logger, IHostApplicationLifetime applicationLifetime) : base(configurationBuilder, serviceProvider, logger, applicationLifetime)
        {
        }

        public void Process(string message)
        {
            throw new NotImplementedException();
        }
    }
}
