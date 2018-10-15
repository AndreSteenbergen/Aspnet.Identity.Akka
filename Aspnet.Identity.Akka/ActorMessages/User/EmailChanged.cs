using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class EmailChanged : IEvent
    {
        public EmailChanged(string email, bool normalized)
        {
            Email = email;
            Normalized = normalized;
        }

        public string Email { get; }
        public bool Normalized { get; }
    }
}
