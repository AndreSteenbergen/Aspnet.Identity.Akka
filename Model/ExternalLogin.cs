using System.Collections.Generic;

namespace Aspnet.Identity.Akka.Model
{
    public struct ExternalLogin
    {
        public ExternalLogin(string loginProvider, string providerKey)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
        }

        public string LoginProvider { get; }
        public string ProviderKey { get; }

        public override bool Equals(object obj)
        {
            if (!(obj is ExternalLogin))
            {
                return false;
            }

            var login = (ExternalLogin)obj;
            return LoginProvider == login.LoginProvider &&
                   ProviderKey == login.ProviderKey;
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
