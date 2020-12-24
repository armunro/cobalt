using System;
using System.Collections.Generic;

namespace Cobalt.Interaction
{
    public class InteractionPlan<TWith>
    {
        public List<Interaction> Interactions { get; set; } = new List<Interaction>();

        public InteractionPlan()
        {
            
        }

        public InteractionPlan<TWith> Interact(Func<InteractionBuilder, Interaction> interact)
        {
            Interaction unitInteraction = interact(new InteractionBuilder());
           
            Interactions.Add(unitInteraction);
            return this;
        }
    }
   
}