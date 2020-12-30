using System.Collections.Generic;
using System.Linq;

namespace Cobalt.Sourcing
{
    public abstract  class ConnectKey
    {
        protected abstract IEnumerable<object> GetEqualityComponents();
        
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public override string ToString()
        {
            return $"{{ {string.Join(", ",  GetEqualityComponents().Select( x=> $"{x}"))} }}";
        }
    }
}