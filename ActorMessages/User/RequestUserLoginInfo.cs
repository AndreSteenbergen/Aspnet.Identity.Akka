using System;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class RequestUserLoginInfo<TKey> : IGetUserProperties
        where TKey : IEquatable<TKey>
    {
        public RequestUserLoginInfo(TKey userId)
        {
            UserId = userId;
        }

        public TKey UserId { get; }
    }
}