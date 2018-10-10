using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.UserCoordinator
{
    class UserEmailChanged<TKey> : IEvent
        where TKey : IEquatable<TKey>
    {
        public UserEmailChanged(TKey userId, string email)
        {
            UserId = userId;
            Email = email;            
        }

        public TKey UserId { get; }
        public string Email { get; }
    }
}
