using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetUserName : IUserPropertyChange
    {
        public SetUserName(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }
    }
}