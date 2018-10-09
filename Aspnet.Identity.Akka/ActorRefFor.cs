using Akka.Actor;

namespace Aspnet.Identity.Akka
{
    public class ActorRefFor<T>
    {
        public ActorRefFor(IActorRef actorRef)
        {
            ActorRef = actorRef;
        }

        public IActorRef ActorRef { get; }
    }
}
