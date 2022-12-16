using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Microsoft.Extensions.Logging;

namespace DungeonOfTheWickedEventSourcing.Common.Extensions
{
    public static class ActorSystemExtensions
    {
        public static IActorRef GetMediator(this ActorSystem actorSystem, ILogger logger)
        {
            var mediator = actorSystem.DeadLetters;

            try
            {
                mediator = DistributedPubSub.Get(actorSystem).Mediator;
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Failed to get DistributedPubSub's Mediator!");
            }

            return mediator;
        }
    }
}
