using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class UserNameChanged : IEvent
    {
        public UserNameChanged(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }
    }
}
