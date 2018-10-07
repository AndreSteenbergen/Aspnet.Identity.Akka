using Aspnet.Identity.Akka.Interfaces;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Claims;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class RemoveClaims : IUserPropertyChange
    {
        public RemoveClaims(IEnumerable<Claim> claims)
        {
            Claims = claims.ToImmutableArray();
        }

        public ImmutableArray<Claim> Claims { get; }
    }
}