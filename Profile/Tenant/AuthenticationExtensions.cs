using Finbuckle.MultiTenant.AspNetCore;
using Profile.Models;

namespace Profile.Tenant
{
    public static class AuthenticationExtensions
    {
        public static MultiTenantBuilder AddPerTenantSocialLogins(this MultiTenantBuilder authBuilder)
        {
            foreach (var socialLogin in SocialLoginProvider.Instance.Get)
            {
                authBuilder = socialLogin.AddSocialLoginTenantOptions(authBuilder);
            }

            return authBuilder;
        }
    }
}
