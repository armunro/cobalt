using System;
using System.Linq;
using Cobalt.Unit.Fact;
using Cobalt.Unit.Fact.Map.Persistent;

namespace Cobalt.Unit
{
    public class CobaltUnit
    {
        private readonly PersistentFactMap<object> _facts;

        // [ctor]
        public CobaltUnit() : this(PersistentFactMap<object>.Empty) { }

        // [ctor]
        private CobaltUnit(PersistentFactMap<object> facts)
        {
            _facts = facts;
        }

        public override string ToString()
        {
            return String.Join(", ", _facts.Select(pair => pair.Key + " = " + pair.Value ?? "(null)").ToArray());
        }

       
    }
}