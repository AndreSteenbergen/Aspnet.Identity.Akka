using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetAccessFailesCount : IUserPropertyChange
    {
        public SetAccessFailesCount(int accessFailedCount)
        {
            AccessFailedCount = accessFailedCount;
        }

        public int AccessFailedCount { get; }
    }
}