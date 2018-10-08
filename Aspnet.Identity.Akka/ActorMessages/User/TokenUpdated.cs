using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class TokenUpdated : IEvent
    {
        public TokenUpdated(string loginProvider, string name, string value)
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
