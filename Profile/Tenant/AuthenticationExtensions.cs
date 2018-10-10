using Finbuckle.MultiTenant.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Profile.Models;

namespace Profile.Tenant
{
    public static class AuthenticationExtensions
    {
        public static MultiTenantBuilder AddPerTenantCookieAuthentication(this MultiTenantBuilder builder)
        {
            return builder.WithPerTenantOptions<CookieAuthenticationOptions>((options, tenantContext) =>
                {
                    // Set a unique cookie name for this tenant.         
                    options.Cookie.Name = tenantContext.Id + "-cookie";
                });
        }

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
