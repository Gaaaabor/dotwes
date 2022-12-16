using Akka.Actor;
using Akka.IO;
using Akka.IO.TcpTools;
using Akka.IO.TcpTools.Actor;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Commands;
using DungeonOfTheWickedEventSourcing.Common.Actors.ActorWalker.Commands;
using DungeonOfTheWickedEventSourcing.Common.Events;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Connection
{
    public class ClientConnectionActor : WebSocketConnectionActorBase
    {
        private readonly Guid _connectionId;
        private readonly System.Net.EndPoint _endPoint;
        private IActorRef _client;

        public ClientConnectionActor(Guid connectionId, ILogger<ClientConnectionActor> logger, System.Net.EndPoint endPoint) : base(logger)
        {
            _connectionId = connectionId;
            _endPoint = endPoint;

            ReceiveAsync<IClientNotification>(OnNotifyClientAsync);
        }

        protected override async Task OnStringReceivedAsync(string message)
        {
            try
            {
                if (_client is null)
                {
                    _client = Sender;
                }

                Logger.LogInformation("{ActorName} on {Endpoint} received a string message: {message}", Self.Path.Name, _endPoint.ToString(), message);

                // TODO: Deserialize into typed messages using a base type!
                // Then publish it to the eventstream

                if (message.Contains(nameof(GenerateDungeonCommand)))
                {
                    Context.System.EventStream.Publish(new GenerateDungeonCommand { ConnectionId = _connectionId });
                    return;
                }

                if (message.Contains(nameof(DiscoverHierarchyCommand)))
                {
                    Context.System.EventStream.Publish(new DiscoverHierarchyCommand());
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error during {Name}!", nameof(OnStringReceivedAsync));
            }

            await Task.CompletedTask;
        }

        private async Task OnNotifyClientAsync(IClientNotification notifyClient)
        {
            var message = await ByteStringWriter.WriteAsTextAsync(notifyClient);
            _client?.Tell(Tcp.Write.Create(message));
        }

        protected override void PreStart()
        {
            Logger.LogInformation("{ActorName} started on {Endpoint}", Self.Path.Name, _endPoint.ToString());
        }
    }
}
