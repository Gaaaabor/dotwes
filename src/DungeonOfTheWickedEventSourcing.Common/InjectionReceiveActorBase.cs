using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.DependencyInjection;
using DungeonOfTheWickedEventSourcing.Common.Actors.Diagnostics.Events;
using DungeonOfTheWickedEventSourcing.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;

namespace DungeonOfTheWickedEventSourcing.Common
{
    public abstract class InjectionReceiveActorBase<TActor> : ReceiveActor where TActor : ReceiveActor
    {
        private readonly IServiceScope _serviceScope;
        private readonly DependencyResolver _dependencyResolver;
        private readonly Meter _meter;
        private readonly UpDownCounter<int> _counter;

        protected ILogger Logger { get; }
        protected IActorRef Mediator { get; }

        public InjectionReceiveActorBase(IServiceProvider serviceProvider)
        {
            _serviceScope = serviceProvider.CreateScope();
            _dependencyResolver = DependencyResolver.For(Context.System);
            Logger = GetRequiredService<ILogger<TActor>>();

            Mediator = Context.System.GetMediator(Logger);

            _meter = new Meter("Meter");
            _counter = _meter.CreateUpDownCounter<int>(typeof(TActor).Name);
        }

        public override void AroundPreStart()
        {
            base.AroundPreStart();
            _counter.Add(1);
        }

        public override void AroundPostStop()
        {
            base.AroundPostStop();
            _counter.Add(-1);
        }

        protected override void PreStart()
        {
            Context.System.EventStream.Publish(new ActorStartedEvent
            {
                Id = Self.Path.Uid,
                Name = Self.Path.Name
            });

            Logger.LogInformation("{ActorName} started", Self.Path.Name);
        }

        protected override void PostStop()
        {
            Logger.LogInformation("{ActorName} stopped", Self.Path.Name);
            Context.System.EventStream.Publish(new ActorStoppedEvent { Id = Self.Path.Uid });
            _serviceScope?.Dispose();
        }

        protected TService GetRequiredService<TService>() where TService : class
        {
            return _serviceScope?.ServiceProvider?.GetRequiredService<TService>();
        }

        protected IActorRef CreateChildActor<TChild>(string name, params object[] args) where TChild : ActorBase
        {
            var props = _dependencyResolver.Props<TChild>(args).WithMailbox("tracedmailbox");
            return Context.ActorOf(props, name);
        }

        protected void Subscribe(string topic, Action onSuccess, IActorRef subscriber = null)
        {
            Receive<SubscribeAck>(ack =>
            {
                if (ack != null && string.Equals(ack.Subscribe?.Topic, topic) && ack.Subscribe.Ref.Equals(Self))
                {
                    Become(onSuccess);
                }
            });

            Mediator.Tell(new Subscribe(topic, subscriber ?? Self));
        }

        protected void Unsubscribe(string topic, IActorRef unsubscriber = null)
        {
            Mediator.Tell(new Unsubscribe(topic, unsubscriber ?? Self));
        }

        protected void Publish(string topic, object message, IActorRef sender = null)
        {
            Mediator.Tell(new Publish(topic, message), sender ?? Self);
        }
    }
}
