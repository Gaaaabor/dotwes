using Akka.Event;
using DungeonOfTheWickedEventSourcing.Api.Application.Enemy.Commands;
using DungeonOfTheWickedEventSourcing.Common;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Enemy
{
    public class EnemyActor : InjectionReceiveActorBase<EnemyActor>
    {
        public EnemyActor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Context.System.EventStream.Subscribe<IEnemyCommand>(Self);
        }
    }
}
