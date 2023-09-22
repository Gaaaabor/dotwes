using DungeonOfTheWickedEventSourcing.Api.Akka.Base;
using DungeonOfTheWickedEventSourcing.Api.Application.Loot.Commands;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Loot
{
    public class LootActor : InjectionReceiveActorBase<LootActor>
    {
        public LootActor()
        {
            Subscribe<ILootCommand>();
        }
    }
}
