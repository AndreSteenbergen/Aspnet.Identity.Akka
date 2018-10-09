using Aspnet.Identity.Akka.Interfaces;
using Aspnet.Identity.Akka.Model;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class FindByLogin : ICommand
    {
        public FindByLogin(ExternalLogin externalLogin)
        {
            ExternalLogin = externalLogin;
        }

        public ExternalLogin ExternalLogin { get; }
    }
}