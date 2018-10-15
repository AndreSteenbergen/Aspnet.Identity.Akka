using Aspnet.Identity.Akka.Interfaces;
using Aspnet.Identity.Akka.Model;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class UserLoginInfoAdded : IEvent
    {
        public UserLoginInfoAdded(ImmutableUserLoginInfo userloginInfo)
        {
            UserloginInfo = userloginInfo;
        }

        public ImmutableUserLoginInfo UserloginInfo { get; }
    }
}
