using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class UpdateUser<TKey, TUser> : ICommand<TKey>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public UpdateUser(TUser user)
        {
            User = user;
        }

        public TKey Key => User.Id;

        public TUser User { get; }
    }
}
