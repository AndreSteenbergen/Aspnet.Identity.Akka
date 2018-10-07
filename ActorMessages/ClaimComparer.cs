using System.Collections.Generic;
using System.Security.Claims;

namespace Aspnet.Identity.Akka.ActorMessages
{
    class ClaimComparer : IEqualityComparer<Claim>
    {
        public bool Equals(Claim x, Claim y)
        {
            if ((x == null) && (y == null)) return true;
            if ((x == null) || (y == null)) return false;

            return string.Equals(x.Type, y.Type) && string.Equals(y.Value, x.Value);
        }

        public int GetHashCode(Claim obj)
        {
            if (obj == null) return 7;

            var hashCode = -1267435237;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(obj.Type);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(obj.Value);
            return hashCode;
        }

        private ClaimComparer()
        {

        }
        public static readonly ClaimComparer Instance = new ClaimComparer();
    }
}
