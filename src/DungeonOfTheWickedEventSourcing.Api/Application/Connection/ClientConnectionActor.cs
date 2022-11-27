using Akka.Actor;
using Akka.IO;
using DungeonOfTheWickedEventSourcing.Api.Application.DungeonGuardian.Commands;
using DungeonOfTheWickedEventSourcing.Common.Akka;
using DungeonOfTheWickedEventSourcing.Common.Akka.ActorWalker.Commands;
using DungeonOfTheWickedEventSourcing.Common.Akka.Events;
using DungeonOfTheWickedEventSourcing.Common.Tools;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

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

            Receive<string>(OnDecoded);
            ReceiveAsync<byte[]>(OnBytesReceivedAsync);

            Receive<Tcp.Received>(OnReceived);
            ReceiveAsync<Tcp.PeerClosed>(OnPeerClosedAsync);

            Receive<IClientNotification>(OnNotifyClient);
        }

        private void OnDecoded(string message)
        {
            Logger.LogInformation("{ActorName} on {Endpoint} received a string message: {message}", Self.Path.Name, _endPoint.ToString(), message);

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

            // TODO: Deserialize into typed messages!
            // Then publish it to the eventstream
        }

        private async Task OnBytesReceivedAsync(byte[] receivedBytes)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                memoryStream.Write(receivedBytes.ToArray());
                memoryStream.Position = 0;

                using var webSocket = WebSocket.CreateFromStream(memoryStream, new WebSocketCreationOptions
                {
                    IsServer = true
                });

                var buffer = WebSocket.CreateServerBuffer(receivedBytes.Length);
                var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        var message = Encoding.UTF8.GetString(buffer[..result.Count]);                        
                        Self.Forward(message);
                        return;

                    case WebSocketMessageType.Binary:                        
                        throw new NotSupportedException("Binary messages are not supported!");

                    default:
                        return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error during OnReceivedAsync!");
                await Self.GracefulStop(TimeSpan.FromSeconds(5));
            }
        }

        private void OnReceived(Tcp.Received received)
        {
            if (_client is null)
            {
                _client = Sender;
            }

            var webSocketKey = WebSocketMessageTools.GetSecWebSocketKey(received.Data.ToString());
            if (!string.IsNullOrEmpty(webSocketKey))
            {
                Sender.Tell(Tcp.Write.Create(ByteString.FromString(WebSocketMessageTools.CreateAck(webSocketKey))));
                return;
            }

            var receivedBytes = received.Data.ToArray();
            var messageType = WebSocketMessageTools.GetMessageType(receivedBytes);
            switch (messageType)
            {
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
                BecomeStacked(OnFrameReceived);
                return;
            }
            else
            {
                Self.Forward(receivedBytes);
            }
        }

        private void OnFrameReceived(object rawFrame)
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
                Self.Forward(_framedWebSocketMessage.ReadAllBytes());
                _framedWebSocketMessage.Close();
                _framedWebSocketMessage = null;
            }
        }

        private async Task OnPeerClosedAsync(Tcp.PeerClosed peerClosed)
        {
            Logger.LogInformation("{ActorName} on {Endpoint} received a peerClosed message: {cause}", Self.Path.Name, _endPoint.ToString(), peerClosed.Cause);
            await Self.GracefulStop(TimeSpan.FromSeconds(5));
        }

        private void OnNotifyClient(IClientNotification notifyClient)
        {
            var firstNonInterfaceType = GetFirstNonInterfaceType(notifyClient.GetType());
            _client?.Tell(TcpTools.CreateWsClientMessage(JsonSerializer.Serialize(notifyClient, firstNonInterfaceType, JsonSerializerOptions.Default)));
        }

        private Type GetFirstNonInterfaceType(Type type)
        {
            if (type?.IsInterface ?? false)
            {
                var result = GetFirstNonInterfaceType(type.UnderlyingSystemType);
                return result ?? type;
            }

            return type;
        }

        protected override void PreStart()
        {
            Logger.LogInformation("{ActorName} started on {Endpoint}", Self.Path.Name, _endPoint.ToString());
        }
    }
}
