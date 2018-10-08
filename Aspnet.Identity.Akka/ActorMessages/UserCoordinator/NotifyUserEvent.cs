using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.UserCoordinator
{
    class NotifyUserEvent : ICommand
    {
        public NotifyUserEvent(IEvent evt)
        {
            Evt = evt;
        }

        public IEvent Evt { get; }
    }
}
