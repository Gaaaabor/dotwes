using Akka.Configuration;
using System.Net;
using System.Text;

namespace DungeonOfTheWickedEventSourcing.Common.Akka
{
    public static class AkkaConfiguration
    {
        public const string ConnectionString = nameof(ConnectionString);
        public const string Roles = nameof(Roles);
        public const string SeedNodeHostNames = nameof(SeedNodeHostNames);
        public const string Port = nameof(Port);
        public const string ClientConnectionPort = nameof(ClientConnectionPort);

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

        public static Config GetClusterConfiguration(string actorSystemName, int port, string[] roles, string[] seedNodes)
        {
            var builder = new StringBuilder();
            builder.AppendLine("akka.actor.provider=cluster");
            builder.AppendLine("akka.extensions=[\"Akka.Cluster.Tools.PublishSubscribe.DistributedPubSubExtensionProvider,Akka.Cluster.Tools\"]");
            builder.AppendLine($"akka.remote.dot-netty.tcp.port={port}");

            var hostName = Dns.GetHostName();
            var hostAddresses = Dns.GetHostAddresses(hostName, System.Net.Sockets.AddressFamily.InterNetwork);
            var hostAddress = hostAddresses.ElementAtOrDefault(0) ?? IPAddress.Loopback;
            builder.AppendLine($"akka.remote.dot-netty.tcp.hostname={hostAddress}");

            if (seedNodes?.Any(x => !string.IsNullOrWhiteSpace(x)) ?? false)
            {
                var translatedSeedNodes = TranslateSeedNodes(actorSystemName, seedNodes);
                builder.AppendLine($"akka.cluster.seed-nodes=[{translatedSeedNodes}]");
            }

            if (roles?.Any(x => !string.IsNullOrWhiteSpace(x)) ?? false)
            {
                var clusterRoles = string.Join(',', roles.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => $"\"{x}\""));
                builder.AppendLine($"akka.cluster.roles=[{clusterRoles}]");
            }

            var config = ConfigurationFactory.ParseString(builder.ToString());
            return config;
        }

        /// <summary>
        /// Translates the container hosts to IP4 addresses.
        /// </summary>
        /// <param name="seedNodes">Nodes to translate with or w/o port</param>
        /// <returns>A translated addresses separated with commas between quotation marks.</returns>
        private static string TranslateSeedNodes(string actorSystemName, string[] seedNodes)
        {
            var clusterSeedNodeBuilder = new StringBuilder();

            for (var i = 0; i < seedNodes.Length; i++)
            {
                var seedNode = seedNodes[i];
                if (string.IsNullOrWhiteSpace(seedNode))
                {
                    continue;
                }

                var addressAndPort = seedNode.Split(':', StringSplitOptions.RemoveEmptyEntries);
                var hostAddresses = Dns.GetHostAddresses(addressAndPort[0], System.Net.Sockets.AddressFamily.InterNetwork);
                if (hostAddresses.Length > 0)
                {
                    if (clusterSeedNodeBuilder.Length > 0)
                    {
                        clusterSeedNodeBuilder.Append(',');
                    }

                    var port = addressAndPort.ElementAtOrDefault(1) ?? "8091";
                    clusterSeedNodeBuilder.AppendFormat("\"akka.tcp://{0}@{1}:{2}\"", actorSystemName, hostAddresses[0], port);
                }
            }

            return clusterSeedNodeBuilder.ToString();
        }
    }
}
