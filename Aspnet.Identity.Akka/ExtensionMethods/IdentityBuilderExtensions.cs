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
