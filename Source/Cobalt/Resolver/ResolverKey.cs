using System.Collections.Generic;

namespace Cobalt.Resolver
{
    public abstract class ResolverKey : ValueObject
    {
        protected abstract override IEnumerable<object> GetEqualityComponents();

    }
}