using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class LockoutEnabledChanged : IEvent
    {
        public LockoutEnabledChanged(bool lockoutEnabled)
        {
            LockoutEnabled = lockoutEnabled;
        }

        public bool LockoutEnabled { get; }
    }
}
