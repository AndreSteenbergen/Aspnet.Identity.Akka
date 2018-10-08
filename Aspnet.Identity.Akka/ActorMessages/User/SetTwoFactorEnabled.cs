using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetTwoFactorEnabled : IUserPropertyChange
    {
        public SetTwoFactorEnabled(bool twoFactorEnabled)
        {
            TwoFactorEnabled = twoFactorEnabled;
        }

        public bool TwoFactorEnabled { get; }
    }
}