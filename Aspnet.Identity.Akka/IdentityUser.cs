using Aspnet.Identity.Akka.Interfaces;
using Aspnet.Identity.Akka.Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Aspnet.Identity.Akka
{
    public abstract class IdentityUser<TKey> : ICloneable
        where TKey : IEquatable<TKey>
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
        public IList<ImmutableIdentityUserToken> Tokens { get; set; }

        internal IList<IUserPropertyChange> Changes { get; } = new List<IUserPropertyChange>();

        public object Clone()
        {
            //memberwise clone
            var obj = MemberwiseClone() as IdentityUser<TKey>;

            //all lists need to be deep copied, because when mutated, the original list is mutated
            if (Claims != null) obj.Claims = new List<Claim>(Claims);
            if (Logins != null) obj.Logins = new List<ImmutableUserLoginInfo>(Logins);
            if (Tokens != null) obj.Tokens = new List<ImmutableIdentityUserToken>(Tokens);

            return obj;
        }

        public abstract IEnumerable<IUserPropertyChange> CompareDifferences(IdentityUser<TKey> other);
    }
}