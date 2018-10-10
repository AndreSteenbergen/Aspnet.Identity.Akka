using System.Collections.Generic;

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
        public string DisplayName { get; }

        public override bool Equals(object obj)
        {
            var info = obj as ImmutableUserLoginInfo;
            return info != null &&
                   LoginProvider == info.LoginProvider &&
                   ProviderKey == info.ProviderKey;
        }

        public override int GetHashCode()
        {
            var hashCode = 1582216818;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LoginProvider);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProviderKey);
            return hashCode;
        }
    }
}