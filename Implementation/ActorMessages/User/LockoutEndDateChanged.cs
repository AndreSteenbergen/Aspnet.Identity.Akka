using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class LockoutEndDateChanged : IEvent
    {
        public LockoutEndDateChanged(DateTimeOffset? lockoutEnd)
        {
            LockoutEnd = lockoutEnd;
        }

        public DateTimeOffset? LockoutEnd { get; }
    }
}
