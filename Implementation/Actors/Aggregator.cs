using Akka.Actor;
using Aspnet.Identity.Akka.ActorMessages;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Aspnet.Identity.Akka.Actors
{
    class AggregatedReply<T>
    {
        public AggregatedReply(IEnumerable<T> rst)
        {
            Rst = rst.ToImmutableArray();
        }

        public ImmutableArray<T> Rst { get; }
    }

    class Aggregator<T> : ReceiveActor
    {
        private IActorRef originalSender;
        private ISet<IActorRef> refs;

        public Aggregator(IEnumerable<IActorRef> refs)
        {
            this.refs = new HashSet<IActorRef>(refs);
            // this operation will finish after 30 sec of inactivity
            // (when no new message arrived)
            Context.SetReceiveTimeout(TimeSpan.FromSeconds(30));
            ReceiveAny(x =>
            {
                originalSender = Sender;
                foreach (var aref in refs) aref.Tell(x);
                Become(Aggregating);
            });
        }

        private void Aggregating()
        {
            var replies = new List<T>();
            // when timeout occurred, we reply with what we've got so far
            Receive<ReceiveTimeout>(_ => ReplyAndStop(replies));
            Receive<NilMessage>(x =>
            {
                refs.Remove(Sender);
                if (refs.Count == 0) ReplyAndStop(replies);
            });
            Receive<T>(x =>
            {
                if (refs.Remove(Sender)) replies.Add(x);
                if (refs.Count == 0) ReplyAndStop(replies);
            });
        }

        private void ReplyAndStop(List<T> replies)
        {
            originalSender.Tell(new AggregatedReply<T>(replies));
            Context.Stop(Self);
        }
    }

}
