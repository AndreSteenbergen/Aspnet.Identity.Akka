using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Models;
using System;
using System.Threading.Tasks;

namespace Profile
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Deze login wordt al gebruikt door een andere gebruiker." }; }
        public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"Gebruikersnaam '{userName}' is ongeldig, gebruik alleen letters en cijfers." }; }
        public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = $"Email '{email}' is ongeldig." }; }
        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = $"Gebruikersnaam '{userName}' is al in gebruik." }; }
        public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = $"Email '{email}' is al in gebruik." }; }
        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"Wachtwoord moet minstens {length} karakters bevatten." }; }
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) { return new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = $"Wachtwoord moet minstens {uniqueChars} unieke karakters bevatten." }; }
    }

    public class UsernameAsPasswordValidator<TKey, TUser> : IPasswordValidator<TUser>
        where TKey : IEquatable<TKey>
        where TUser : Aspnet.Identity.Akka.IdentityUser<TKey>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            if (string.Equals(user.UserName, password, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "UsernameAsPassword",
                    Description = "Gebruikersnaam en wachtwoord dienen te verschillen"
                }));
            }
            if (string.Equals(user.Email, password, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "EamilAsPassword",
                    Description = "Email en wachtwoord dienen te verschillen"
                }));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }


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

                o.Password.RequiredLength = 10;
                o.Password.RequiredUniqueChars = 4;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireDigit = false;
            })
            .AddSignInManager()
            .AddPasswordValidator<UsernameAsPasswordValidator<Guid, ApplicationUser>>()
            .AddErrorDescriber<CustomIdentityErrorDescriber>()
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
