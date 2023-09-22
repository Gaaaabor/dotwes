using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;

namespace DungeonOfTheWickedEventSourcing.Api.Akka.Base
{
    public abstract class InjectionReceiveActorBase<TActor> : ReceiveActor where TActor : ReceiveActor
    {
        private readonly IResolverScope _resolverScope;
        private readonly DependencyResolver _dependencyResolver;

        protected ILogger Logger { get; }

        public InjectionReceiveActorBase()
        {
            _dependencyResolver = DependencyResolver.For(Context.System);
            _resolverScope = _dependencyResolver.Resolver.CreateScope();
            Logger = GetService<ILogger<TActor>>();
        }

        protected override void PreStart()
        {
            Logger.LogInformation("{ActorName} - {ActorPath} started", typeof(TActor).Name, Self.Path.Name);
        }

        protected override void PostStop()
        {
            Logger.LogInformation("{ActorName} - {ActorPath} stopped", typeof(TActor).Name, Self.Path.Name);
            _resolverScope?.Dispose();
        }

        protected TService GetService<TService>() where TService : class
        {
            return _resolverScope.Resolver.GetService<TService>();
        }

        protected IActorRef CreateChildActor<TChild>(string name, params object[] args) where TChild : ActorBase
        {
            var props = _dependencyResolver.Props<TChild>(args).WithMailbox("tracedmailbox");
            return Context.ActorOf(props, name);
        }

        protected void Subscribe<TChannel>() where TChannel : class
        {
            Context.System.EventStream.Subscribe<TChannel>(Self);
        }

        protected void Unsubscribe<TChannel>() where TChannel : class
        {
            Context.System.EventStream.Unsubscribe<TChannel>(Self);
        }

        protected void Publish(object message)
        {
            Context.System.EventStream.Publish(message);
        }
    }
}
