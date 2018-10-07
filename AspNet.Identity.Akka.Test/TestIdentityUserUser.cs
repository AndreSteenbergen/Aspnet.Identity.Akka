using Aspnet.Identity.Akka;
using Aspnet.Identity.Akka.Interfaces;
using System;
using System.Collections.Generic;

namespace AspNet.Identity.Akka.Test
{
    public class TestIdentityUser : IdentityUser<Guid>
    {
        public TestIdentityUser(Guid id) : base(id)
        {
        }

        public override IEnumerable<IUserPropertyChange> CompareDifferences(IdentityUser<Guid> other)
        {
            return new IUserPropertyChange[0];
        }
    }
}
