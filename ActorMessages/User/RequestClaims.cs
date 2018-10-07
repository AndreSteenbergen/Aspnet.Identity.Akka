using System;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class RequestClaims<TKey> : IGetUserProperties
        where TKey : IEquatable<TKey>
    {
        public RequestClaims(TKey userId)
        {
            UserId = userId;
        }

        public TKey UserId { get; }
    }
}