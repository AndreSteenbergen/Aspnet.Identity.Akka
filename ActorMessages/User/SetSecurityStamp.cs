using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetSecurityStamp : IUserPropertyChange
    {
        public SetSecurityStamp(string stamp)
        {
            Stamp = stamp;
        }

        public string Stamp { get; }
    }
}