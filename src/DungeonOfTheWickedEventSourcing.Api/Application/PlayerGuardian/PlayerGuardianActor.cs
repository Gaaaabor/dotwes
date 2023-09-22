using DungeonOfTheWickedEventSourcing.Api.Akka.Base;

namespace DungeonOfTheWickedEventSourcing.Api.Application.PlayerGuardian
{
    public class PlayerGuardianActor : InjectionReceiveActorBase<PlayerGuardianActor>
    {
        public const string ActorName = "playerguardian";

        public PlayerGuardianActor()
        {
        }
    }
}
