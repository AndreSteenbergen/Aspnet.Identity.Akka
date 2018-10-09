using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class FindByEmail : ICommand
    {
        public FindByEmail(string normalizedEmail)
        {
            NormalizedEmail = normalizedEmail;
        }

        public string NormalizedEmail { get; }
    }
}