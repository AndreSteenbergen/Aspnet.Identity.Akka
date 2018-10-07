using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class CreateUser<TKey, TUser> : ICommand

        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
    {
        public CreateUser(TUser user)
        {
            User = user;
        }

        public TUser User { get; }
    }
}
