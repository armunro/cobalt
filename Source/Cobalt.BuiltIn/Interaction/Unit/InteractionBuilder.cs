namespace Cobalt.BuiltIn.Interaction.Unit
{
    public class InteractionBuilder
    {
        public  UnitInteractions Unit =>  new UnitInteractions();
    }
    
    public class UnitFactInteractions
    {
        public NewFactChange Create(string factName, object factValue) =>
            new NewFactChange(factName, factValue);
    }
    
    public class UnitInteractions
    {
        public  UnitFactInteractions Facts => new UnitFactInteractions();
    }
}