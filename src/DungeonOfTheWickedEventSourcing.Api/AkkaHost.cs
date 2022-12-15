using Akka.Actor;
using DungeonOfTheWickedEventSourcing.Api.Application;
using DungeonOfTheWickedEventSourcing.Api.Application.Connection;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian;
using DungeonOfTheWickedEventSourcing.Common.Akka;
using DungeonOfTheWickedEventSourcing.Common.Akka.ActorWalker.Commands;
using DungeonOfTheWickedEventSourcing.Common.Akka.ActorWalker.Events;
using DungeonOfTheWickedEventSourcing.Common.Akka.ActorWalker.Models;
using System.Text.Json;
using static DungeonOfTheWickedEventSourcing.Common.Akka.TracedMessageQueue;

namespace DungeonOfTheWickedEventSourcing.Api
{
    public class AkkaHost : AkkaHostServiceBase, IGraphDataProvider
    {
        public AkkaHost(IServiceProvider serviceProvider, ILogger<AkkaHost> logger) : base(serviceProvider, logger)
        { }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            CreateChildActor<DungeonGuardianActor>(DungeonGuardianActor.ActorName);

            var configuration = ServiceProvider.GetService<IConfiguration>();
            var clientConnectionPort = configuration.GetValue(AkkaConfiguration.ClientConnectionPort, 0);

            CreateChildActor<ClientConnectionManagerActor>(name: ClientConnectionManagerActor.ActorName, clientConnectionPort);
        }

        public string GetGraphFields()
        {
            return @"{
  ""edges_fields"": [
    {
      ""field_name"": ""id"",
      ""type"": ""string""
    },
    {
      ""field_name"": ""source"",
      ""type"": ""string""
    },
    {
      ""field_name"": ""target"",
      ""type"": ""string""
    },
    {
      ""field_name"": ""mainStat"",
      ""type"": ""number""
    }
  ],
  ""nodes_fields"": [
    {
      ""field_name"": ""id"",
      ""type"": ""string""
    },
    {
      ""field_name"": ""title"",
      ""type"": ""string""
    },
    {
      ""field_name"": ""mainStat"",
      ""type"": ""string""
    },
    {
      ""field_name"": ""secondaryStat"",
      ""type"": ""number""
    },
    {
      ""color"": ""red"",
      ""field_name"": ""arc__failed"",
      ""type"": ""number""
    },
    {
      ""color"": ""green"",
      ""field_name"": ""arc__passed"",
      ""type"": ""number""
    },
    {
      ""displayName"": ""Role"",
      ""field_name"": ""detail__role"",
      ""type"": ""string""
    }
  ]
}";
        }


        public async Task<string> GetGraphData()
        {
            var result = await ActorWalker.Ask<HierarchyDiscoveredEvent>(new DiscoverHierarchyCommand());

            var actorHierarchyNodes = new Queue<ActorHierarchyNode>(new[] { result.Root });

            var nodes = new List<Node>();
            var edges = new List<Edge>();

            while (actorHierarchyNodes.Count > 0)
            {
                var actorHierarchyNode = actorHierarchyNodes.Dequeue();
                nodes.Add(new Node { Id = actorHierarchyNode.Name, Title = actorHierarchyNode.Name });

                foreach (var child in actorHierarchyNode.Children)
                {
                    nodes.Add(new Node { Id = child.Name, Title = child.Name });

                    edges.Add(new Edge
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Source = actorHierarchyNode.Name,
                        Target = child.Name
                    });

                    actorHierarchyNodes.Enqueue(child);
                }
            }

            return JsonSerializer.Serialize(new { edges, nodes });

        }
    }
}
