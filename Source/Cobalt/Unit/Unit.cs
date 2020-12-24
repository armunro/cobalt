using System.Collections.Generic;
using System.Linq;
using Cobalt.Interaction.Unit;
using Cobalt.Unit.Fact.Map.Persistent;

namespace Cobalt.Unit
{
    public class Unit : IInteractionReciever<NewFactInteraction>
    {
        public PersistentFactMap Facts { get; set; }
        public UnitQualities Qualities { get; set; } = new UnitQualities();
        public List<UnitInteraction> Interactions { get; set; } = new List<UnitInteraction>();

        // [ctor]
        internal Unit() : this(PersistentFactMap.Empty)
        {
            
        }

        // [ctor]
        private Unit(PersistentFactMap facts)
        {
            Facts = facts;
        }


        public void ReceiveInteraction(NewFactInteraction newFact)
        {
           Facts = Facts.Add(newFact.FactName, newFact.FactValue);
        }

        public override string ToString()
        {
            return string.Join(", ", Facts.Select(pair => pair.Key + " = " + pair.Value ?? "(null)").ToArray());
        }




        public static Unit Make(IEnumerable<KeyValuePair<string, object>> existingValues)
        {
            return new Unit(PersistentFactMap.Empty
                .Add(existingValues,
                    x => x.Key,
                    x => x.Value));
        }
    }

    public interface IInteractionReciever<T>
    {
        void ReceiveInteraction(T newFact);
    }
}