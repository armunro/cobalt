using Cobalt.Interaction.Unit;

namespace Cobalt.Interaction
{
    public class InteractionBuilder
    {
        public  UnitInteractions Unit =>  new UnitInteractions();
    }
    
    public class UnitFactInteractions
    {
        public NewFactInteraction Create(string factName, object factValue) =>
            new NewFactInteraction(factName, factValue);
    }
    
    public class UnitInteractions
    {
        public  UnitFactInteractions Facts => new UnitFactInteractions();
    }
}