using Akka.Actor;
using Akka.IO;
using DungeonOfTheWickedEventSourcing.Common.Akka;
using DungeonOfTheWickedEventSourcing.Common.Tools;
using System.Text;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Connection
{
    public class ClientConnectionActor : InjectionReceiveActorBase<ClientConnectionActor>
    {
        private readonly System.Net.EndPoint _endPoint;
        private IActorRef _client;

        public ClientConnectionActor(IServiceProvider serviceProvider, System.Net.EndPoint endPoint) : base(serviceProvider)
        {
            _endPoint = endPoint;

            Receive<string>(x =>
            {
                Logger.LogInformation("{ActorName} started on {Endpoint} received a string message: {message}", Self.Path.Name, _endPoint.ToString(), x);

                // TODO: Deserialize into typed messages!
                // Then publish it to the eventstream
            });

            Receive<Tcp.Received>(OnReceived);
            Receive<Tcp.PeerClosed>(OnPeerClosed);
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

            var decodedBytes = WebSocketMessageTools.Decode(received.Data.ToArray());
            if (WebSocketMessageTools.IsCloseMessage(decodedBytes))
            {
                Logger.LogWarning("Connection close message received!");
                Sender.Tell(Tcp.Write.Create(ByteString.FromBytes(WebSocketMessageTools.CloseMessage)));
                return;
            }

            Self.Tell(Encoding.UTF8.GetString(decodedBytes));
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
