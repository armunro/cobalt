namespace Cobalt.BuiltIn.Interaction.Unit
{
    public class InteractionBuilder
    {
        public  UnitInteractions Unit =>  new UnitInteractions();
    }
    
    public class UnitFactInteractions
    {
        public NewFactCoChange Create(string factName, object factValue) =>
            new NewFactCoChange(factName, factValue);
    }
    
    public class UnitInteractions
    {
        public  UnitFactInteractions Facts => new UnitFactInteractions();
    }
}