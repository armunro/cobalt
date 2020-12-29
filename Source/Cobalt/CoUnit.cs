using System.Collections.Generic;
using System.Linq;
using Cobalt.Collections.Map;

namespace Cobalt
{
    public class CoUnit
    {
        public PersistentFactMap Facts { get; }


        // [ctor]
        internal CoUnit() : this(PersistentFactMap.Empty)
        {
        }

        // [ctor]
        private CoUnit(PersistentFactMap facts)
        {
            Facts = facts;
        }


        public override string ToString()
        {
            return string.Join(", ", Facts.Select(pair => pair.Key + " = " + pair.Value ?? "(null)").ToArray());
        }


        public static CoUnit Make(IEnumerable<KeyValuePair<string, object>> existingValues)
        {
            return new CoUnit(PersistentFactMap.Empty
                .Add(existingValues,
                    x => x.Key,
                    x => x.Value));
        }
        
        public static CoUnit Make()
        {
            return new CoUnit(PersistentFactMap.Empty);
        }
    }
}