using System;
using System.Collections.Generic;

namespace Cobalt.Sourcing
{
    public class Source<TKey, T>
    {
        public Dictionary<TKey, SourceConnection<T>> Connections { get; }

        public Source()
        {
            Connections = new Dictionary<TKey, SourceConnection<T>>();
        }

        public Source<TKey, T> Connect(TKey key, Func<T> resolveFunc)
        {
            Connections.Add(key, new SourceConnection<T>(resolveFunc));
            return this;
        }
    }
}