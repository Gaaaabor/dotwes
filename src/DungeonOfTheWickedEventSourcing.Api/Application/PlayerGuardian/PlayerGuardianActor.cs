using DungeonOfTheWickedEventSourcing.Common;

namespace DungeonOfTheWickedEventSourcing.Api.Application.PlayerGuardian
{
    public class PlayerGuardianActor : InjectionReceiveActorBase<PlayerGuardianActor>
    {
        public const string ActorName = "playerguardian";

        public PlayerGuardianActor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
