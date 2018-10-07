using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class SetLockoutEndDate : IUserPropertyChange
    {
        public SetLockoutEndDate(DateTimeOffset? lockoutEnd)
        {
            LockoutEnd = lockoutEnd;
        }

        public DateTimeOffset? LockoutEnd { get; }
    }
}