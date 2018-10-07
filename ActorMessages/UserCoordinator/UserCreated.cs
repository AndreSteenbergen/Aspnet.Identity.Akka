using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.UserCoordinator
{
    class UserCreated<TKey> : IEvent
        where TKey : IEquatable<TKey>
    {
        public UserCreated(
            TKey userId,
            string userName,
            string normalizedUserName,
            string email,
            string normalizedEmail)
        {
            UserId = userId;
            UserName = userName;
            NormalizedUserName = normalizedUserName;
            Email = email;
            NormalizedEmail = normalizedEmail;
        }

        public TKey UserId { get; }
        public string UserName { get; }
        public string NormalizedUserName { get; }
        public string Email { get; }
        public string NormalizedEmail { get; }
    }
}
