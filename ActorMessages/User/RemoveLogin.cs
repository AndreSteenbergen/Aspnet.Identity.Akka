using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class RemoveLogin : IUserPropertyChange
    {
        public RemoveLogin(string loginProvider, string providerKey)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
        }

        public string LoginProvider { get; }
        public string ProviderKey { get; }
    }
}