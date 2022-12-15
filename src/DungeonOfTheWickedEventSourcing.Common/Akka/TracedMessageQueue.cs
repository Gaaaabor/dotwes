using Akka.Actor;
using Akka.Dispatch;
using Akka.Dispatch.MessageQueues;
using Akka.Event;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace DungeonOfTheWickedEventSourcing.Common.Akka
{
    public class TracedMessageQueue : IMessageQueue, IUnboundedMessageQueueSemantics, IUnboundedDequeBasedMessageQueueSemantics
    {
        // doesn't need to be threadsafe - only called from within actor
        private readonly Stack<Envelope> _prependBuffer = new Stack<Envelope>();
        private readonly ConcurrentQueue<Envelope> _queue = new();
        private readonly IActorRef _owner;
        private readonly EventStream _eventStream;


        /// <inheritdoc cref="IMessageQueue"/>
        public bool HasMessages => Count > 0;

        /// <inheritdoc cref="IMessageQueue"/>
        public int Count => _queue.Count + _prependBuffer.Count;

        public TracedMessageQueue(IActorRef owner, EventStream eventStream)
        {
            _owner = owner;
            _eventStream = eventStream;
        }

        /// <inheritdoc cref="IMessageQueue"/>
        public void Enqueue(IActorRef receiver, Envelope envelope)
        {
            _queue.Enqueue(envelope);

            // TODO: Implement
            //var uid = (_owner?.Path.Uid ?? receiver?.Path.Uid) ?? 0;

            //_eventStream.Publish(new ActorGotMessage
            //{
            //    Nodes = new List<Node>
            //    {
            //        new Node { Id = uid.ToString(), Title = _owner?.Path.Name },
            //        new Node { Id = envelope.Sender.Path.Uid.ToString(), Title = envelope.Sender.Path.Name }
            //    },
            //    Edges = new List<Edge>
            //    {
            //        new Edge { Id = Guid.NewGuid().ToString(), Source = envelope.Sender.Path.Uid.ToString(), Target = uid.ToString() }
            //    }
            //});
        }

        /// <inheritdoc cref="IMessageQueue"/>
        public bool TryDequeue(out Envelope envelope)
        {
            var result = TryDequeueInternal(out envelope);
            return result;
        }

        /// <inheritdoc cref="IMessageQueue"/>
        public void CleanUp(IActorRef owner, IMessageQueue deadletters)
        {
            Envelope msg;
            while (TryDequeueInternal(out msg))
            {
                deadletters.Enqueue(owner, msg);
            }
        }

        /// <summary>
        /// Add a message to the front of the queue via the prepend buffer.
        /// </summary>
        /// <param name="envelope">The message we wish to append to the front of the queue.</param>
        public void EnqueueFirst(Envelope envelope)
        {
            _prependBuffer.Push(envelope);
        }

        private bool TryDequeueInternal(out Envelope envelope)
        {
            if (_prependBuffer.Count > 0)
            {
                envelope = _prependBuffer.Pop();
                return true;
            }

            return _queue.TryDequeue(out envelope);
        }

        public class Node
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("subtitle")]
            public string Subtitle { get; set; }

            [JsonPropertyName("arc__failed")]
            public double ArcFailed { get; set; }

            [JsonPropertyName("arc__passed")]
            public double ArcPassed { get; set; }

            [JsonPropertyName("detail__zone")]
            public string DetailZone { get; set; }
        }

        public class Edge
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("source")]
            public string Source { get; set; }

            [JsonPropertyName("target")]
            public string Target { get; set; }

            [JsonPropertyName("mainStat")]
            public string MainStat { get; set; }
        }

        public class ActorGotMessage
        {
            public List<Edge> Edges { get; set; } = new List<Edge>();

            public List<Node> Nodes { get; set; } = new List<Node>();
        }
    }
}
