using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.UserCoordinator
{
    class UserNameChanged<TKey> : IEvent
        where TKey : IEquatable<TKey>
    {
        public UserNameChanged(TKey userId, string userName, string normalizedUsername)
        {
            UserId = userId;
            UserName = userName;
            NormalizedUsername = normalizedUsername;
        }

        public TKey UserId { get; }
        public string UserName { get; }
        public string NormalizedUsername { get; }
    }



    

    //            case UserClaimAdded<TKey> uca:
    //                break;
    //            case UserClaimRemoved<TKey> ucr:
    //                break;
}
