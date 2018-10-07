using Aspnet.Identity.Akka.Interfaces;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Claims;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class ClaimsAdded : IEvent
    {
        public ClaimsAdded(IEnumerable<Claim> claimsToAdd)
        {
            ClaimsToAdd = claimsToAdd.ToImmutableArray();
        }

        public ImmutableArray<Claim> ClaimsToAdd { get; }
    }
}
