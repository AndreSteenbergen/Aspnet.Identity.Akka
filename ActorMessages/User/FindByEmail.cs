namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class FindByEmail
    {
        public FindByEmail(string normalizedEmail)
        {
            NormalizedEmail = normalizedEmail;
        }

        public string NormalizedEmail { get; }
    }
}