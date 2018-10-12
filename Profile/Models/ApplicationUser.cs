using Aspnet.Identity.Akka;
using Aspnet.Identity.Akka.Interfaces;
using System;
using System.Collections.Generic;

namespace Profile.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser() : this(Guid.NewGuid())
        {
            
        }

        public ApplicationUser(Guid id) : base(id)
        {
        }

        public override IEnumerable<IUserPropertyChange> CompareDifferences(IdentityUser<Guid> other)
        {
            return new IUserPropertyChange[0];
        }
    }
}
