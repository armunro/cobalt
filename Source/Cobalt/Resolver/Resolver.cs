using System;
using System.Collections.Generic;
using Cobalt.Resolver.Resolution;

namespace Cobalt.Resolver
{
    
    public class Resolver<T>
    {
        public Dictionary<ResolverKey, Resolution<T>> Registered { get; } = new Dictionary<ResolverKey, Resolution<T>>();

        public Resolver<T> Volatile(ResolverKey key, Func<T> resolutionFunc)
        {
            Registered.Add(key, new Resolution<T>(resolutionFunc));
            return this;
        }

        public Resolver<T> Cached(ResolverKey key, Func<T> resolutionFunc)
        {
            Registered.Add(key, new CachedResolution<T>(resolutionFunc));
            return this;
        }

        public Resolver<T> Cached(ResolverKey key, T instance)
        {
            Registered.Add(key, new CachedResolution<T>(instance));
            return this;
        }
    }
}