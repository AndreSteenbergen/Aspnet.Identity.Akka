using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class PasswordHashChanged : IEvent
    {
        public PasswordHashChanged(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public string PasswordHash { get; }
    }
}
