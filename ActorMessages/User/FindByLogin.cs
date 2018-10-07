namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class FindByLogin
    {
        public FindByLogin(ExternalLogin externalLogin)
        {
            ExternalLogin = externalLogin;
        }

        public ExternalLogin ExternalLogin { get; }
    }
}