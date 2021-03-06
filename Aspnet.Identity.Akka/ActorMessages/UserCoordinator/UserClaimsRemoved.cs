﻿using Aspnet.Identity.Akka.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Claims;

namespace Aspnet.Identity.Akka.ActorMessages.UserCoordinator
{
    public class UserClaimsRemoved<TKey> : IEvent
        where TKey : IEquatable<TKey>
    {
        public UserClaimsRemoved(TKey userId, IEnumerable<Claim> claims)
        {
            Claims = (claims ?? new Claim[0]).ToImmutableArray();
            UserId = userId;
        }

        public ImmutableArray<Claim> Claims { get; }
        public TKey UserId { get; }
    }
}
