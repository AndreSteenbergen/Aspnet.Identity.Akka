using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetEmail : IUserPropertyChange
    {
        public SetEmail(string email, bool normalized)
        {
            Email = email;
            Normalized = normalized;
        }

        public string Email { get; }
        public bool Normalized { get; }
    }
}