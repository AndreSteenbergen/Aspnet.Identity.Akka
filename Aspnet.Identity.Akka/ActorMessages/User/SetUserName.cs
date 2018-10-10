using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetUserName : IUserPropertyChange
    {
        public SetUserName(string userName, bool normalized)
        {
            UserName = userName;
            Normalized = normalized;
        }

        public string UserName { get; }
        public bool Normalized { get; }
    }
}