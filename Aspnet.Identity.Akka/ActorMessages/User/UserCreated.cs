using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class UserCreated<TKey, TUser> : IEvent
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public UserCreated(TUser user)
        {
            User = (TUser) user.Clone();
        }

        public TUser User { get; }
    }
}
