using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class UserDeleted<TKey> : IEvent
        where TKey : IEquatable<TKey>
    {
        public UserDeleted(TKey userId)
        {
            UserId = userId;
        }

        public TKey UserId { get; }
    }    
}
