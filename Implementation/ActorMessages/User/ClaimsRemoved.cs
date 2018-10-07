using Aspnet.Identity.Akka.Interfaces;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Claims;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class ClaimsRemoved : IEvent
    {
        public ClaimsRemoved(IEnumerable<Claim> claimsToRemove)
        {
            ClaimsToRemove = claimsToRemove.ToImmutableArray();
        }

        public ImmutableArray<Claim> ClaimsToRemove { get; }
    }
}
