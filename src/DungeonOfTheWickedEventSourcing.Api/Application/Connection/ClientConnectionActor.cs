using Akka.Actor;
using Akka.IO;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Commands;
using DungeonOfTheWickedEventSourcing.Common.Akka;
using DungeonOfTheWickedEventSourcing.Common.Akka.ActorWalker.Commands;
using DungeonOfTheWickedEventSourcing.Common.Akka.Events;
using DungeonOfTheWickedEventSourcing.Common.Tools;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Connection
{
    public class ClientConnectionActor : InjectionReceiveActorBase<ClientConnectionActor>
    {
        private readonly Guid _connectionId;
        private readonly System.Net.EndPoint _endPoint;
        private IActorRef _client;

        private FramedWebSocketMessage _framedWebSocketMessage;

        public ClientConnectionActor(Guid connectionId, IServiceProvider serviceProvider, System.Net.EndPoint endPoint) : base(serviceProvider)
        {
            _connectionId = connectionId;
            _endPoint = endPoint;

            Receive<string>(OnStringReceived);

            ReceiveAsync<Tcp.Received>(OnReceivedAsync);
            Receive<Tcp.PeerClosed>(OnPeerClosed);

            ReceiveAsync<IClientNotification>(OnNotifyClientAsync);
        }

        private void OnStringReceived(string message)
        {
            try
            {
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
                Logger.LogError(ex, "Error during {Name}!", nameof(OnStringReceived));
            }
        }

        private async Task OnReceivedAsync(Tcp.Received received)
        {
            if (_client is null)
            {
                _client = Sender;
            }

            try
            {
                var secWebSocketKey = WebSocketMessageTools.GetSecWebSocketKey(received.Data.ToString());
                if (!string.IsNullOrEmpty(secWebSocketKey))
                {
                    Sender.Tell(Tcp.Write.Create(ByteString.FromString(WebSocketMessageTools.CreateAck(secWebSocketKey))));
                    return;
                }

                var receivedBytes = received.Data.ToArray();
                var messageType = WebSocketMessageTools.GetMessageType(receivedBytes);
                switch (messageType)
                {
                    case StandardWebSocketMessageType.Binary:
                        throw new NotSupportedException("Binary messages are not supported!");

                    case StandardWebSocketMessageType.Ping:
                        Logger.LogInformation("Received a Ping!");
                        // Ping is not used yet!
                        //_client.Tell(Tcp.Write.Create(ByteString.FromBytes(WebSocketMessageTools.PongMessage)));
                        return;
                    case StandardWebSocketMessageType.Pong:
                        Logger.LogInformation("Received a Pong!");
                        // Pong is not used yet!
                        //_client.Tell(Tcp.Write.Create(ByteString.FromBytes(WebSocketMessageTools.PingMessage)));
                        return;
                    case StandardWebSocketMessageType.Close:
                        Logger.LogInformation("Connection close message received!");
                        _client.Tell(Tcp.Write.Create(ByteString.FromBytes(WebSocketMessageTools.CloseMessage)));
                        return;
                }

                var totalLength = WebSocketMessageTools.GetMessageTotalLength(receivedBytes);
                if (totalLength > (ulong)receivedBytes.Length)
                {
                    _framedWebSocketMessage = new FramedWebSocketMessage(totalLength);
                    _framedWebSocketMessage.Write(receivedBytes);
                    BecomeStacked(() =>
                    {
                        ReceiveAsync<object>(OnFrameReceivedAsync);
                    });

                    return;
                }

                var receivedMessage = await TcpTools.ReadMessageBytesAsync(receivedBytes);
                Self.Forward(receivedMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error during {Name}!", nameof(OnReceivedAsync));
                await Self.GracefulStop(TimeSpan.FromSeconds(5));
            }
        }

        private async Task OnFrameReceivedAsync(object rawFrame)
        {
            try
            {
                if (rawFrame is Tcp.Received frame)
                {
                    if (_framedWebSocketMessage is null)
                    {
                        UnbecomeStacked();
                        Self.Forward(frame);
                        return;
                    }

                    _framedWebSocketMessage.Write(frame.Data.ToArray());
                }
                else
                {
                    Self.Forward(rawFrame);
                    return;
                }

                if (_framedWebSocketMessage.IsCompleted())
                {
                    UnbecomeStacked();
                    var receivedMessage = await TcpTools.ReadMessageBytesAsync(_framedWebSocketMessage.ReadAllBytes());
                    Self.Forward(receivedMessage);
                    _framedWebSocketMessage.Close();
                    _framedWebSocketMessage = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error during {Name}!", nameof(OnFrameReceivedAsync));
            }
        }

        private void OnPeerClosed(Tcp.PeerClosed peerClosed)
        {
            Logger.LogInformation("{ActorName} on {Endpoint} received a peerClosed message: {cause}", Self.Path.Name, _endPoint.ToString(), peerClosed.Cause);
            Context.Stop(Self);
        }

        private async Task OnNotifyClientAsync(IClientNotification notifyClient)
        {
            var message = await TcpTools.CreateTcpWriteMessageAsync(notifyClient);
            _client?.Tell(message);
        }

        protected override void PreStart()
        {
            Logger.LogInformation("{ActorName} started on {Endpoint}", Self.Path.Name, _endPoint.ToString());
        }
    }
}
