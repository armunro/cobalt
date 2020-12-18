using System;
using System.Collections.Generic;
using System.Linq;
using Cobalt.Interaction.Builder;
using Cobalt.Interaction.Unit;
using Cobalt.Unit.Fact;
using Cobalt.Unit.Fact.Map.Persistent;

namespace Cobalt.Unit
{
    public class CobaltUnit
    {
        public PersistentFactMap Facts { get; } 
        public UnitQualities Qualities { get; set; } = new UnitQualities();
        public List<UnitInteraction> Interactions { get; set; } = new List<UnitInteraction>();

        // [ctor]
        internal CobaltUnit() : this(PersistentFactMap.Empty)
        {
            
        }

        // [ctor]
        private CobaltUnit(PersistentFactMap facts)
        {
            Facts = facts;
        }

        public override string ToString()
        {
            return string.Join(", ", Facts.Select(pair => pair.Key + " = " + pair.Value ?? "(null)").ToArray());
        }


        public void Interact(Func<Interactions, UnitInteraction> interact)
        {
            var unitInteraction = interact(new Interactions());
            unitInteraction.Run(this);
            Interactions.Add(unitInteraction);
        }

        public static CobaltUnit Make(IEnumerable<KeyValuePair<string, object>> existingValues)
        {
            return new CobaltUnit(PersistentFactMap.Empty
                .Add(existingValues,
                    x => x.Key,
                    x => x.Value));
        }
    }



    public class UnitQualities
    {
    }
}