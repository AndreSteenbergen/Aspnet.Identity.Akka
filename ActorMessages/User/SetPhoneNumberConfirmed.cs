using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetPhoneNumberConfirmed : IUserPropertyChange
    {
        public SetPhoneNumberConfirmed(bool phoneNumberConfirmed)
        {
            PhoneNumberConfirmed = phoneNumberConfirmed;
        }

        public bool PhoneNumberConfirmed { get; }
    }
}