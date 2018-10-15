using Aspnet.Identity.Akka.ActorMessages.User;
using Aspnet.Identity.Akka.ActorMessages.UserCoordinator;
using Aspnet.Identity.Akka.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    internal static class AspnetIdentityResolverGetFormatterHelper
    {
        private static readonly Dictionary<Type, Func<Type, object>> FormatterMap = new Dictionary<Type, Func<Type, object>>
        {
            {typeof(IEvent), (_) => new IEventFormatter()},
            {typeof(AccessFailedCountChanged), (_) => new AccessFailedCountChangedEventFormatter()},
            {typeof(PhoneNumberConfirmed), (_) => new PhoneNumberConfirmedEventFormatter()},
            {typeof(TwoFactorEnabledChanged), (_) => new TwoFactorEnabledChangedEventFormatter()},
            {typeof(UserDeleted), (_) => new UserDeletedEventFormatter()},
            {typeof(EmailConfirmed), (_) => new EmailConfirmedEventFormatter()},
            {typeof(PasswordHashChanged), (_) => new PasswordHashChangedEventFormatter() },
            {typeof(EmailChanged), (_) => new EmailChangedEventFormatter() },
            {typeof(LoginRemoved), (_) => new LoginRemovedEventFormatter() },
            {typeof(SecurityStampChanged), (_) => new SecurityStampChangedEventFormatter() },
            {typeof(TokenAdded), (_) => new TokenAddedEventFormatter() },
            {typeof(Claim), (_) => new ClaimFormatter() },
            {typeof(TokenUpdated), (_) => new TokenUpdatedEventFormatter() },
            {typeof(UserLoginInfoAdded), (_) => new UserLoginInfoAddedEventFormatter()},
            {typeof(LockoutEndDateChanged), (_) => new LockoutEndDateChangedEventFormatter()},
            {typeof(LockoutEnabledChanged), (_) => new LockoutEnabledChangedEventFormatter()},
            {typeof(UserNameChanged), (_) => new UserNameChangedEventFormatter()},
            {typeof(PhoneNumberChanged), (_) => new PhoneNumberChangedEventFormatter()},
            {typeof(TokenRemoved), (_) => new TokenRemovedEventFormatter()},
            {typeof(UserDeleted<>), (t) => {
                Type[] types = t.GetGenericArguments();
                var formatterType = typeof(UserDeletedEventFormatter<>).MakeGenericType(types[0]);
                return Activator.CreateInstance(formatterType);
            } },
            {typeof(ClaimsRemoved), (_) => new ClaimsRemovedEventFormatter() },
            {typeof(ClaimsAdded), (_) => new ClaimsAddedEventFormatter() },
            {typeof(UserClaimsAdded<>), (t) => {
                Type[] types = t.GetGenericArguments();
                var formatterType = typeof(UserClaimsAddedEventFormatter<>).MakeGenericType(types[0]);
                return Activator.CreateInstance(formatterType);
            } },
            {typeof(UserClaimsRemoved<>), (t) => {
                Type[] types = t.GetGenericArguments();
                var formatterType = typeof(UserClaimsRemovedEventFormatter<>).MakeGenericType(types[0]);
                return Activator.CreateInstance(formatterType);
            } },
            {typeof(UserCreated<>), (t) => {
                Type[] types = t.GetGenericArguments();
                var formatterType = typeof(UserCreatedEventFormatter<>).MakeGenericType(types[0]);
                return Activator.CreateInstance(formatterType);
            } },
            {typeof(UserEmailChanged<>), (t) => {
                Type[] types = t.GetGenericArguments();
                var formatterType = typeof(UserEmailChangedEventFormatter<>).MakeGenericType(types[0]);
                return Activator.CreateInstance(formatterType);
            } },
            {typeof(UserLoginAdded<>), (t) => {
                Type[] types = t.GetGenericArguments();
                var formatterType = typeof(UserLoginAddedEventFormatter<>).MakeGenericType(types[0]);
                return Activator.CreateInstance(formatterType);
            } },
            {typeof(UserLoginRemoved<>), (t) => {
                Type[] types = t.GetGenericArguments();
                var formatterType = typeof(UserLoginRemovedEventFormatter<>).MakeGenericType(types[0]);
                return Activator.CreateInstance(formatterType);
            } },
            {typeof(UserNameChanged<>), (t) => {
                Type[] types = t.GetGenericArguments();
                var formatterType = typeof(UserNameChangedEventFormatter<>).MakeGenericType(types[0]);
                return Activator.CreateInstance(formatterType);
            } },
            {typeof(UserCreated<,>), (t) => {
                Type[] types = t.GetGenericArguments();
                var formatterType = typeof(UserCreatedEventFormatter<,>).MakeGenericType(types[0], types[1]);
                return Activator.CreateInstance(formatterType);
            } }

        };

        private static readonly Dictionary<Type, object> InstanceFormmatterMap = new Dictionary<Type, object>();

        internal static object GetFormatter(Type t)
        {
            if (!InstanceFormmatterMap.TryGetValue(t, out var formatter))
            {
                if (t.GetGenericArguments().Length > 0)
                {
                    var genericTypeDef = t.GetGenericTypeDefinition();
                    InstanceFormmatterMap[t] = formatter = FormatterMap.TryGetValue(genericTypeDef, out var formatterFunc) ? formatterFunc(t) : null;
                }
                else
                {
                    InstanceFormmatterMap[t] = formatter = FormatterMap.TryGetValue(t, out var formatterFunc) ? formatterFunc(t) : null;
                }
            }
            return formatter;
        }
    }
}
