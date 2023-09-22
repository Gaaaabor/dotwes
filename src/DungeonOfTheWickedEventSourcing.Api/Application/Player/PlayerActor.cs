using DungeonOfTheWickedEventSourcing.Common;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Player
{
    public class PlayerActor : InjectionReceiveActorBase<PlayerActor>
    {
        public PlayerActor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
