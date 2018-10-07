namespace Aspnet.Identity.Akka.Model
{
    public class ImmutableUserLoginInfo
    {
        public ImmutableUserLoginInfo(string loginProvider, string providerKey, string displayName)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            DisplayName = displayName;
        }
                
        public string LoginProvider { get; }
        public string ProviderKey { get; }
        public string ProviderDisplayName { get; }
        public string DisplayName { get; }
    }
}