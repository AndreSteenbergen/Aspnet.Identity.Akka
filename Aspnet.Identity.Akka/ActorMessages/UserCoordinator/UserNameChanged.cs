using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.UserCoordinator
{
    public class UserNameChanged<TKey> : IEvent
        where TKey : IEquatable<TKey>
    {
        public UserNameChanged(TKey userId, string userName)
        {
            UserId = userId;
            UserName = userName;            
        }

        public TKey UserId { get; }
        public string UserName { get; }        
    }
}
