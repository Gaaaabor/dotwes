using DungeonOfTheWickedEventSourcing.Common.Akka;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Dungeon
{
    public class DungeonActor : InjectionReceiveActorBase<DungeonActor>
    {
        public DungeonActor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
