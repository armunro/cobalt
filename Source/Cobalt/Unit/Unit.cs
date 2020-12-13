﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cobalt.Unit.Fact;
using Cobalt.Unit.Fact.Map.Persistent;

namespace Cobalt.Unit
{
    public class CobaltUnit
    {
        private readonly PersistentFactMap _facts;

        // [ctor]
        internal CobaltUnit() : this(PersistentFactMap.Empty)
        {
        }

        // [ctor]
        private CobaltUnit(PersistentFactMap facts)
        {
            _facts = facts;
        }

        public override string ToString()
        {
            return string.Join(", ", _facts.Select(pair => pair.Key + " = " + pair.Value ?? "(null)").ToArray());
        }

        public static CobaltUnit Make()
        {
            return new CobaltUnit();
        }

        public static CobaltUnit Make(IEnumerable<KeyValuePair<string, object>> existingValues)
        {
            return new CobaltUnit(PersistentFactMap.Empty
                .Add(existingValues,
                    x => x.Key,
                    x => x.Value));
        }
    }
}