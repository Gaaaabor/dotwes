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
                .Tell(new Tcp.Bind(Self, new IPEndPoint(IPAddress.Any, port)));

            Receive<Tcp.Bound>(OnBound);
            Receive<Tcp.Connected>(OnConnected);
        }

        protected virtual void OnBound(Tcp.Bound bound)
        {
            Logger.LogInformation("Listening on {LocalAddress}", bound.LocalAddress);
        }

        protected virtual void OnConnected(Tcp.Connected connected)
        {
            Logger.LogInformation("Connected to {RemoteAddress}", connected.RemoteAddress);

            var connectionId = Guid.NewGuid().ToString("N");
            var clientConnectionActor = CreateChildActor<ClientConnectionActor>(name: connectionId, connected.RemoteAddress);
            Sender.Tell(new Tcp.Register(clientConnectionActor));
        }
    }
}
