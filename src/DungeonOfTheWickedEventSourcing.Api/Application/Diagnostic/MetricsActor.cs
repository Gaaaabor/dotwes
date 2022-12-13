using DungeonOfTheWickedEventSourcing.Common.Akka;

namespace DungeonOfTheWickedEventSourcing.Api.Application.Diagnostic
{
    public class MetricsActor : InjectionReceiveActorBase<MetricsActor>
    {
        public const string ActorName = "metrics";

        public MetricsActor(IServiceProvider serviceProvider) : base(serviceProvider)
        { }

        protected override void PreStart()
        {
            base.PreStart();
        }
    }
}
