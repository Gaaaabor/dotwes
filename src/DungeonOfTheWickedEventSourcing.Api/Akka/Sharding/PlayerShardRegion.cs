using Akka.Actor;
using DungeonOfTheWickedEventSourcing.Api.Application.Player;

namespace DungeonOfTheWickedEventSourcing.Api.Akka.Sharding
{
    public class PlayerShardRegion
    {
        public static Props ActorFactory(string entityId)
        {
            return Props.Create<PlayerActor>(entityId);
        }
    }
}
