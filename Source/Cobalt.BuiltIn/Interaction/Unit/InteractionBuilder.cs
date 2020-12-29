namespace Cobalt.BuiltIn.Interaction.Unit
{
    public class InteractionBuilder
    {
        public  UnitInteractions Unit =>  new UnitInteractions();
    }
    
    public class UnitFactInteractions
    {
        public NewFactCoInteract Create(string factName, object factValue) =>
            new NewFactCoInteract(factName, factValue);
    }
    
    public class UnitInteractions
    {
        public  UnitFactInteractions Facts => new UnitFactInteractions();
    }
}