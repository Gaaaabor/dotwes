using Akka.Actor;
using Akka.IO;
using DungeonOfTheWickedEventSourcing.Common.Akka;
using DungeonOfTheWickedEventSourcing.Common.Tools;
using System.Net.WebSockets;
using System.Text;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Connection
{
    public class ClientConnectionActor : InjectionReceiveActorBase<ClientConnectionActor>
    {
        /// <summary>
        /// The magic number 8 is the first 8 bytes of the first frame, which contains the total length of the framed message.
        /// </summary>
        private const int LengthHeaderLength = 8;

        private readonly System.Net.EndPoint _endPoint;
        private IActorRef _client;
        private List<byte> _frames;
        private ulong _messageLength;
        private ulong _bytes;

        public ClientConnectionActor(IServiceProvider serviceProvider, System.Net.EndPoint endPoint) : base(serviceProvider)
        {
            _endPoint = endPoint;

            Receive<string>(OnDecoded);
            ReceiveAsync<byte[]>(OnBytesReceivedAsync);

            Receive<Tcp.Received>(OnReceived);
            Receive<Tcp.PeerClosed>(OnPeerClosed);
        }

        private void OnDecoded(string message)
        {
            Logger.LogInformation("{ActorName} started on {Endpoint} received a string message: {message}", Self.Path.Name, _endPoint.ToString(), message);

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

                var webSocket = WebSocket.CreateFromStream(memoryStream, new WebSocketCreationOptions
                {
                    IsServer = true
                });

                var buffer = WebSocket.CreateServerBuffer(receivedBytes.Length);
                var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        var message = Encoding.UTF8.GetString(buffer[..result.Count]);
                        Self.Tell(message);
                        return;

                    case WebSocketMessageType.Binary:
                        throw new NotSupportedException("Binary messages are not supported!");

                    case WebSocketMessageType.Close:
                        Logger.LogWarning("Connection close message received!");
                        Sender.Tell(Tcp.Write.Create(ByteString.FromBytes(WebSocketMessageTools.CloseMessage)));
                        return;

                    default:
                        return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error during OnReceivedAsync!");
            }
        }

        private void OnReceived(Tcp.Received received)
        {
            if (_client is null)
            {
                _client = Sender;
            }

            var key = WebSocketMessageTools.GetSecWebSocketKey(received.Data.ToString());
            if (!string.IsNullOrEmpty(key))
            {
                Sender.Tell(Tcp.Write.Create(ByteString.FromString(WebSocketMessageTools.CreateAck(key))));
                return;
            }

            _messageLength = WebSocketMessageTools.GetMessageTotalLength(received.Data.ToArray());
            if (_messageLength > (ulong)received.Data.Count)
            {
                _frames = new List<byte>();
                _frames.AddRange(received.Data.ToArray());
                _bytes += (ulong)received.Data.Count;
                BecomeStacked(OnFrameReceived);
                return;
            }
            else
            {
                Self.Tell(received.Data.ToArray());
            }
        }

        private void OnFrameReceived(object rawFrame)
        {
            if (rawFrame is Tcp.Received frame)
            {
                if (_frames is null)
                {
                    UnbecomeStacked();
                    Self.Tell(frame);
                    return;
                }

                _frames.AddRange(frame.Data.ToArray());
                _bytes += (ulong)frame.Data.Count;
            }
            else
            {
                Self.Tell(rawFrame);
                return;
            }

            if (_bytes == (_messageLength + LengthHeaderLength))
            {
                UnbecomeStacked();
                Self.Tell(_frames.ToArray());
                _frames = null;
                _bytes = 0;
                _messageLength = 0;
            }
        }

        private void OnPeerClosed(Tcp.PeerClosed peerClosed)
        {
            Logger.LogInformation("{ActorName} started on {Endpoint} received a peerClosed message: {cause}", Self.Path.Name, _endPoint.ToString(), peerClosed.Cause);
        }

        protected override void PreStart()
        {
            Logger.LogInformation("{ActorName} started on {Endpoint}", Self.Path.Name, _endPoint.ToString());
        }
    }
}
