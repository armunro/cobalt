using System;

namespace Cobalt.Resolver.Resolution
{
    public class CachedResolution<T> : Resolution<T>
    {
        public CachedResolution(Func<T> resolveFunc) : base(resolveFunc)
        {
        }

        public CachedResolution(T resolveValue) : base(() => resolveValue)
        {
        }
    }
}