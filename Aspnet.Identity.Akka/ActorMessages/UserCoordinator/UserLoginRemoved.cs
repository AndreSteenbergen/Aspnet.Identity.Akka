using Aspnet.Identity.Akka.Interfaces;
using Aspnet.Identity.Akka.Model;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.UserCoordinator
{
    public class UserLoginRemoved<TKey> : IEvent
    where TKey : IEquatable<TKey>
    {
        public UserLoginRemoved(TKey userId, ImmutableUserLoginInfo userLoginInfo)
        {
            UserId = userId;
            UserLoginInfo = userLoginInfo;
        }

        public TKey UserId { get; }
        public ImmutableUserLoginInfo UserLoginInfo { get; }
    }
}
