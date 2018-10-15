using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class UserNameChanged : IEvent
    {
        public UserNameChanged(string userName, bool normalized)
        {
            UserName = userName;
            Normalized = normalized;
        }

        public string UserName { get; }
        public bool Normalized { get; }
    }
}
