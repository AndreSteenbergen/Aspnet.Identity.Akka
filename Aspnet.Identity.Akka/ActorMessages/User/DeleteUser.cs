using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    public class DeleteUser<TKey> :ICommand where TKey : IEquatable<TKey>
    {
        public DeleteUser(TKey userId)
        {
            UserId = userId;
        }

        public TKey UserId { get; }
    }
}
