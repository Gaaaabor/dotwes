using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;

namespace DungeonOfTheWickedEventSourcing.Common.Akka.Extensions
{
    public static class ActorSystemExtensions
    {
        public static IActorRef GetMediator(this ActorSystem actorSystem)
        {
            IActorRef mediator = actorSystem.DeadLetters;

            try
            {
                mediator = DistributedPubSub.Get(actorSystem).Mediator;
            }
            catch { }

            return mediator;
        }
    }
}
