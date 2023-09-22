using DungeonOfTheWickedEventSourcing.Api.Akka.Base;
using DungeonOfTheWickedEventSourcing.Api.Application.Enemy.Commands;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Enemy
{
    public class EnemyActor : InjectionReceiveActorBase<EnemyActor>
    {
        public EnemyActor()
        {
            Subscribe<IEnemyCommand>();
        }
    }
}
