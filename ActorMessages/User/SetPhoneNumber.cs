using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetPhoneNumber : IUserPropertyChange
    {
        public SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public string PhoneNumber { get; }
    }
}