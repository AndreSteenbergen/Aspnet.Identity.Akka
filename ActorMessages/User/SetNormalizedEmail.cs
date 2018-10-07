using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetNormalizedEmail : IUserPropertyChange
    {
        public SetNormalizedEmail(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}