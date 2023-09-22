using Akka.Actor;
using DungeonOfTheWickedEventSourcing.Api.Application.Dungeon;

namespace DungeonOfTheWickedEventSourcing.Api.Akka.Sharding
{
    public class DungeonShardRegion
    {
        public static Props ActorFactory(string entityId)
        {
            return Props.Create<DungeonActor>(entityId);
        }
    }
}
