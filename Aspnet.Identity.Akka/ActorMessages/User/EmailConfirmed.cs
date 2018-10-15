using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class EmailConfirmed : IEvent
    {
        public EmailConfirmed(bool confirmed)
        {
            Confirmed = confirmed;
        }

        public bool Confirmed { get; }
    }
}
