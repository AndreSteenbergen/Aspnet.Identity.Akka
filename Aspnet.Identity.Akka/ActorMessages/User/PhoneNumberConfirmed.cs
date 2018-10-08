using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class PhoneNumberConfirmed : IEvent
    {
        public PhoneNumberConfirmed(bool phoneNumberConfirmed)
        {
            Confirmed = phoneNumberConfirmed;
        }

        public bool Confirmed { get; }
    }
}
