using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Models;
using System;

namespace Profile
{
    public static class ServiceCollectionExtensions
    {
        public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration)
            where TConfig : class, new()
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var config = new TConfig();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }

        public static IdentityBuilder AddCalqoDefaultIdentity<TUser>(this IServiceCollection services) where TUser : class => services.AddCalqoDefaultIdentity<TUser>(_ => { });

        public static IdentityBuilder AddCalqoDefaultIdentity<TUser>(this IServiceCollection services, Action<IdentityOptions> configureOptions) where TUser : class
        {
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddSocialLogins()
            .AddIdentityCookies(builder =>
            {
                builder.ApplicationCookie.PostConfigure(o =>
                {
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    o.SlidingExpiration = true;
                });
            })
            ;

            return services.AddIdentityCore<TUser>(o =>
            {
                o.Stores.MaxLengthForKeys = 128;
                configureOptions?.Invoke(o);
            })
            .AddSignInManager()
            .AddDefaultTokenProviders();
        }

        private static AuthenticationBuilder AddSocialLogins(this AuthenticationBuilder authBuilder)
        {
            foreach (var socialLogin in SocialLoginProvider.Instance.Get)
            {
                authBuilder = socialLogin.AddSocialLoginDefaultOptions(authBuilder);
            }

            return authBuilder;
        }
    }
}
