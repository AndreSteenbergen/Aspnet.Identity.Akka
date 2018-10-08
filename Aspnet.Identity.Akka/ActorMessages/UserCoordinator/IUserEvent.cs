using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.UserCoordinator
{
    class IUserEvent<TKey> : IEvent
    {
        public IUserEvent(TKey userId, IEvent evt)
        {
            UserId = userId;
            Evt = evt;
        }

        public TKey UserId { get; }
        public IEvent Evt { get; }
    }
}
