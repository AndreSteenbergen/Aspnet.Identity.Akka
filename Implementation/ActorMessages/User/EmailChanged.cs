using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class EmailChanged : IEvent
    {
        public EmailChanged(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
