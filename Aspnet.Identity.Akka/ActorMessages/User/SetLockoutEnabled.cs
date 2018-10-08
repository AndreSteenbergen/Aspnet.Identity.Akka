using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetLockoutEnabled : IUserPropertyChange
    {
        public SetLockoutEnabled(bool lockoutEnabled)
        {
            LockoutEnabled = lockoutEnabled;
        }

        public bool LockoutEnabled { get; }
    }
}