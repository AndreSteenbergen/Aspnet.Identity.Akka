using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class AccessFailedCountChanged : IEvent
    {
        public AccessFailedCountChanged(int accessFailedCount)
        {
            AccessFailedCount = accessFailedCount;
        }

        public int AccessFailedCount { get; }
    }
}
