using System;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class RequestTokens<TKey> : IGetUserProperties
        where TKey : IEquatable<TKey>
    {
        public RequestTokens(TKey userId)
        {
            UserId = userId;
        }

        public TKey UserId { get; }
    }
}