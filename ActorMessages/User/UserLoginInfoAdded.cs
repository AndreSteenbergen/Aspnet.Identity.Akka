using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class UserLoginInfoAdded : IEvent
    {
        public UserLoginInfoAdded(string loginProvider, string providerDisplayName, string providerKey)
        {
            LoginProvider = loginProvider;
            ProviderDisplayName = providerDisplayName;
            ProviderKey = providerKey;
        }

        public string LoginProvider { get; }
        public string ProviderDisplayName { get; }
        public string ProviderKey { get; }
    }
}
