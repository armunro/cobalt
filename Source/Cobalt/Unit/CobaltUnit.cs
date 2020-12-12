using System;
using System.Linq;
using Cobalt.Unit.Fact;
using Cobalt.Unit.Fact.Map.Persistent;

namespace Cobalt.Unit
{
    public class CobaltUnit
    {
        private readonly PersistentFactMap _facts;

        // [ctor]
        public CobaltUnit() : this(PersistentFactMap.Empty) { }

        // [ctor]
        private CobaltUnit(PersistentFactMap facts)
        {
            _facts = facts;
        }

        public override string ToString()
        {
            return string.Join(", ", _facts.Select(pair => pair.Key + " = " + pair.Value ?? "(null)").ToArray());
        }

       
    }
}