using Aspnet.Identity.Akka.Interfaces;
using Aspnet.Identity.Akka.Model;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.UserCoordinator
{
    class UserLoginAdded<TKey> : IEvent
        where TKey : IEquatable<TKey>
    {
        public UserLoginAdded(TKey userId, ImmutableUserLoginInfo userLoginInfo)
        {
            UserId = userId;
            UserLoginInfo = userLoginInfo;
        }

        public TKey UserId { get; }
        public ImmutableUserLoginInfo UserLoginInfo { get; }
    }



    //case UserLoginChanged<TKey> ulc:
    //                break;
    //            case UserLoginRemoved<TKey> ulr:
    //                break;

    //            case UserClaimAdded<TKey> uca:
    //                break;
    //            case UserClaimRemoved<TKey> ucr:
    //                break;
}
