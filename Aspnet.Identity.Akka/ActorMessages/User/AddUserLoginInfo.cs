using Aspnet.Identity.Akka.Interfaces;
using Aspnet.Identity.Akka.Model;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class AddUserLoginInfo : IUserPropertyChange
    {
        public AddUserLoginInfo(ImmutableUserLoginInfo userloginInfo) {
            UserLoginInfo = userloginInfo;
        }

        public ImmutableUserLoginInfo UserLoginInfo { get; }
    }
}