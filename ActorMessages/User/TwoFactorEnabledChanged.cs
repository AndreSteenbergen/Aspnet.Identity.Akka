using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class TwoFactorEnabledChanged : IEvent
    {
        public TwoFactorEnabledChanged(bool twoFactorEnabled)
        {
            TwoFactorEnabled = twoFactorEnabled;
        }

        public bool TwoFactorEnabled { get; }
    }
}
