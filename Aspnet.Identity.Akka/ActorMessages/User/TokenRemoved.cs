using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class TokenRemoved : IEvent
    {
        public TokenRemoved(string loginProvider, string name)
        {
            LoginProvider = loginProvider;
            Name = name;
        }

        public string LoginProvider { get; }
        public string Name { get; }
    }
}
