using System.Collections.Generic;
using System.Linq;
using Cobalt.Collections.Map;

namespace Cobalt
{
    public class Unit
    {
        public PersistentFactMap Facts { get; }


        // [ctor]
        internal Unit() : this(PersistentFactMap.Empty)
        {
        }

        // [ctor]
        private Unit(PersistentFactMap facts)
        {
            Facts = facts;
        }


        public override string ToString() => 
            string.Join(", ", Facts.Select(pair => pair.Key + " = " + pair.Value ?? "(null)").ToArray());


        public static Unit Make(IEnumerable<KeyValuePair<string, object>> existingValues) => 
            new Unit(PersistentFactMap.Empty.Add(existingValues, x => x.Key, x => x.Value));

        public static Unit Make() => new Unit(PersistentFactMap.Empty);
    }
}