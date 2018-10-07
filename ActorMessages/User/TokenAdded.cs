using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class TokenAdded : IEvent
    {
        public TokenAdded(string loginProvider, string name, string value)
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
