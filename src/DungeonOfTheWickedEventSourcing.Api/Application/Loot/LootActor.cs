using Akka.Event;
using DungeonOfTheWickedEventSourcing.Api.Application.Loot.Commands;
using DungeonOfTheWickedEventSourcing.Common.Akka;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Loot
{
    public class LootActor : InjectionReceiveActorBase<LootActor>
    {
        public LootActor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Context.System.EventStream.Subscribe<ILootCommand>(Self);
        }
    }
}
