using Aspnet.Identity.Akka.Interfaces;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Claims;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class AddClaims : IUserPropertyChange
    {
        public AddClaims(IEnumerable<Claim> claims)
        {
            Claims = (claims ?? new Claim[0]).ToImmutableArray();
        }

        public ImmutableArray<Claim> Claims { get; }
    }
}