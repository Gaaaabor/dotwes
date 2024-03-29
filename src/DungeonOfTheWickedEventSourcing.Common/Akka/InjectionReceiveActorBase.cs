﻿using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.DependencyInjection;
using DungeonOfTheWickedEventSourcing.Common.Akka.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DungeonOfTheWickedEventSourcing.Common.Akka
{
    public abstract class InjectionReceiveActorBase<TActor> : ReceiveActor where TActor : ReceiveActor
    {
        private readonly IServiceScope _serviceScope;
        private readonly DependencyResolver _dependencyResolver;        

        protected ILogger Logger { get; }
        protected IActorRef Mediator { get; }

        public InjectionReceiveActorBase(IServiceProvider serviceProvider)
        {
            _serviceScope = serviceProvider.CreateScope();
            _dependencyResolver = DependencyResolver.For(Context.System);
            Logger = GetRequiredService<ILogger<TActor>>();

            Mediator = Context.System.GetMediator(Logger);
        }

        protected override void PreStart()
        {
            Logger.LogInformation("{ActorName} started", Self.Path.Name);
        }

        protected override void PostStop()
        {
            Logger.LogInformation("{ActorName} stopped", Self.Path.Name);
            _serviceScope?.Dispose();
        }

        protected TService GetRequiredService<TService>() where TService : class
        {
            return _serviceScope?.ServiceProvider?.GetRequiredService<TService>();
        }

        protected IActorRef CreateChildActor<TChild>(string name, params object[] args) where TChild : ActorBase
        {
            var props = _dependencyResolver.Props<TChild>(args);
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
