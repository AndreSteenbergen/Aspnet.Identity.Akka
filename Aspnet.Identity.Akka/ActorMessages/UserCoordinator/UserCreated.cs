using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.UserCoordinator
{
    public class UserCreated<TKey> : IEvent
        where TKey : IEquatable<TKey>
    {
        public UserCreated(
            TKey userId,
            string normalizedUserName,
            string normalizedEmail)
        {
            UserId = userId;
            NormalizedUserName = normalizedUserName;
            NormalizedEmail = normalizedEmail;
        }

        public TKey UserId { get; }
        public string NormalizedUserName { get; }
        public string NormalizedEmail { get; }
    }
}
