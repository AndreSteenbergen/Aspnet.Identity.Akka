using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class RemoveToken : IUserPropertyChange
    {
        public RemoveToken(string loginProvider, string name)
        {
            LoginProvider = loginProvider;
            Name = name;
        }

        public string LoginProvider { get; }
        public string Name { get; }
    }
}