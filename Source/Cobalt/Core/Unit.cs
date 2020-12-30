using System.Collections.Generic;
using Cobalt.Core.Structures.FactMap;

namespace Cobalt.Core
{
    public class Unit
    {
        public PersistentFactMap Facts { get; }

        // [ctor]
        private Unit(PersistentFactMap facts)
        {
            Facts = facts;
        }


        public static Unit Make(IEnumerable<KeyValuePair<string, object>> existingValues) =>
            new Unit(PersistentFactMap.Empty.Add(existingValues, x => x.Key, x => x.Value));

        public static Unit Make() => new Unit(PersistentFactMap.Empty);
    }
}