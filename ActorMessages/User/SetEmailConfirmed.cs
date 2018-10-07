using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetEmailConfirmed : IUserPropertyChange
    {
        public SetEmailConfirmed(bool confirmed)
        {
            Confirmed = confirmed;
        }

        public bool Confirmed { get; }
    }
}