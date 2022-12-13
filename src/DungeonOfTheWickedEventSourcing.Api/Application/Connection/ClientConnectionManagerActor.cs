using Akka.Actor;
using Akka.IO;
using DungeonOfTheWickedEventSourcing.Common.Akka;
using System.Net;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Connection
{
    public class ClientConnectionManagerActor : InjectionReceiveActorBase<ClientConnectionManagerActor>
    {
        public const string ActorName = "clientconnectionmanager";

        public ClientConnectionManagerActor(IServiceProvider serviceProvider, int port) : base(serviceProvider)
        {
            Context.System
                .Tcp()
                .Tell(new Tcp.Bind(Self, new IPEndPoint(IPAddress.Any, port), options: new[] { new Inet.SO.ReceiveBufferSize(1024) }));

            Receive<Tcp.Bound>(OnBound);
            Receive<Tcp.Connected>(OnConnected);
        }

        protected virtual void OnBound(Tcp.Bound bound)
        {
            Logger.LogInformation("{ActorName} Listening on {LocalAddress}", nameof(ClientConnectionManagerActor), bound.LocalAddress);
        }

        protected virtual void OnConnected(Tcp.Connected connected)
        {
            Logger.LogInformation("{ActorName} received a connection from {RemoteAddress}", nameof(ClientConnectionManagerActor), connected.RemoteAddress);

            var connectionId = Guid.NewGuid();
            var name = $"clientconnection_{connected.RemoteAddress}_{connectionId:N}";
            var clientConnectionActor = CreateChildActor<ClientConnectionActor>(name: name, connectionId, connected.RemoteAddress);            
            Sender.Tell(new Tcp.Register(clientConnectionActor));            
        }
    }
}
