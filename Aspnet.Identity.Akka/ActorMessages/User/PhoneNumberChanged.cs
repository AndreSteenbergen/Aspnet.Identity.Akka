using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class PhoneNumberChanged : IEvent
    {
        public PhoneNumberChanged(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public string PhoneNumber { get; }
    }
}
