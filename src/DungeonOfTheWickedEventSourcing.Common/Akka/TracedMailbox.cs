using Akka.Actor;
using Akka.Configuration;
using Akka.Dispatch;
using Akka.Dispatch.MessageQueues;

namespace DungeonOfTheWickedEventSourcing.Common.Akka
{
    public class TracedMailbox : MailboxType, IProducesMessageQueue<TracedMessageQueue>
    {
        public override IMessageQueue Create(IActorRef owner, ActorSystem system)
        {
            return new TracedMessageQueue(owner, system.EventStream);
        }

        public TracedMailbox() : this(null, null)
        { }

        public TracedMailbox(Settings settings, Config config) : base(settings, config)
        {}
    }
}
