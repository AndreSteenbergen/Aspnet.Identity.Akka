namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class FindByUsername
    {
        public FindByUsername(string normalizedUsername)
        {
            NormalizedUsername = normalizedUsername;
        }

        public string NormalizedUsername { get; }
    }
}