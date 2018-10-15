using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class SecurityStampChanged : IEvent
    {
        public SecurityStampChanged(string stamp)
        {
            Stamp = stamp;
        }

        public string Stamp { get; }
    }
}
