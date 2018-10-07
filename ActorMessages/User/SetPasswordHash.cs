using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class SetPasswordHash : IUserPropertyChange
    {
        public SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public string PasswordHash { get; }
    }
}