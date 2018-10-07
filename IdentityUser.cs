using Aspnet.Identity.Akka.Interfaces;
using Aspnet.Identity.Akka.Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Aspnet.Identity.Akka
{
    public abstract class IdentityUser<TKey> where TKey : IEquatable<TKey>
    {
        public IdentityUser(TKey id)
        {
            Id = id;
        }

        public TKey Id { get; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public IList<Claim> Claims { get; set; }
        public IList<ImmutableUserLoginInfo> Logins { get; set; }
        public IList<ImmutableIdentityUserToken<TKey>> Tokens { get; set; }

        internal IList<IUserPropertyChange> Changes { get; } = new List<IUserPropertyChange>();
        public abstract IEnumerable<IUserPropertyChange> CompareDifferences(IdentityUser<TKey> other);
    }
}