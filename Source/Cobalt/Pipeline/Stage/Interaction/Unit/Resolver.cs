using System;
using System.Collections.Generic;

//Purpose: quick and dirty IoT container ( 1/2 resolver - 1/2 - activator)
namespace Cobalt.Interaction.Unit
{
    public class Resolver<TKey, TVal>
    {
        Dictionary<TKey, TVal> _registry = new Dictionary<TKey, TVal>();
    
        public Resolver<TKey, TVal> Resolve<TValue>(Func<TKey> when)
        {
            TVal instance = Activator.CreateInstance<TVal>();
            TKey key = when();
            _registry.Add(key,instance);
            return this;
        }
    }
}