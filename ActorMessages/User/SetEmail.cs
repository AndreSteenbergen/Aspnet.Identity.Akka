using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetEmail : IUserPropertyChange
    {
        public SetEmail(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}