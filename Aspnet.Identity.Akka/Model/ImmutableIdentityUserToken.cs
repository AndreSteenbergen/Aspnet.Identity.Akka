using System;

namespace Aspnet.Identity.Akka.Model
{
    public class ImmutableIdentityUserToken<TKey> where TKey : IEquatable<TKey>
    {
        public ImmutableIdentityUserToken(TKey userId, string loginProvider, string name, string value)
        {
            UserId = userId;
            LoginProvider = loginProvider;
            Name = name;
            Value = value;
        }

        public TKey UserId { get; }
        public string LoginProvider { get; }
        public string Name { get; }
        public string Value { get; }
    }
}