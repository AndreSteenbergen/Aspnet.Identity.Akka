using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class PhoneNumberConfirmed : IEvent
    {
        public PhoneNumberConfirmed(bool phoneNumberConfirmed)
        {
            Confirmed = phoneNumberConfirmed;
        }

        public bool Confirmed { get; }
    }
}
