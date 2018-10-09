using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class FindByUsername : ICommand
    {
        public FindByUsername(string normalizedUsername)
        {
            NormalizedUsername = normalizedUsername;
        }

        public string NormalizedUsername { get; }
    }
}