using System;

namespace Aspnet.Identity.Akka.Model
{
    public class ImmutableIdentityUserToken
    {
        public ImmutableIdentityUserToken(string loginProvider, string name, string value)
        {
            LoginProvider = loginProvider;
            Name = name;
            Value = value;
        }
        public string LoginProvider { get; }
        public string Name { get; }
        public string Value { get; }
    }
}