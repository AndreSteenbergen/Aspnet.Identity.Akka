using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;

namespace Aspnet.Identity.Akka.ExtensionMethods
{
    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder AddIdentity<TUser>(this IServiceCollection services, string defaultSignInScheme)
            where TUser : class
        {
            return services.AddIdentity<TUser>(defaultSignInScheme, setupAction: null);
        }

        public static IdentityBuilder AddIdentity<TUser>(
            this IServiceCollection services,
            string defaultSignInScheme,
            Action<IdentityOptions> setupAction)
            where TUser : class
        {
            // Services used by identity
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = defaultSignInScheme;
            });

            // Hosting doesn't add IHttpContextAccessor by default
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Identity services
            services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
            services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();

            // No interface for the error describer so we can add errors without rev'ing the interface
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<TUser>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser>>();
            services.TryAddScoped<UserManager<TUser>, UserManager<TUser>>();
            services.TryAddScoped<SignInManager<TUser>, SignInManager<TUser>>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new IdentityBuilder(typeof(TUser), services);
        }

        public static IdentityBuilder AddUserStore(this IdentityBuilder builder)
        {
            AddStores(builder.Services, builder.UserType);
            return builder;
        }

        private static void AddStores(IServiceCollection services, Type userType)
        {
            var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
            if (identityUserType == null)
            {
                throw new InvalidOperationException("No usertype found in DI");
            }

            var keyType = identityUserType.GenericTypeArguments[0];
            var userStoreType = typeof(UserStore<,>).MakeGenericType(keyType, userType);

            services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
        }

        public static IdentityBuilder AddIdentityServerUserClaimsPrincipalFactory(this IdentityBuilder builder)
        {
            var interfaceType = typeof(IUserClaimsPrincipalFactory<>);
            interfaceType = interfaceType.MakeGenericType(builder.UserType);

            var classType = typeof(UserClaimsPrincipalFactory<>);
            classType = classType.MakeGenericType(builder.UserType);

            builder.Services.AddScoped(interfaceType, classType);

            return builder;
        }

        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}
