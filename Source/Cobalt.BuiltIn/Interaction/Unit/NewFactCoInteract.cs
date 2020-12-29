namespace Cobalt.BuiltIn.Interaction.Unit
{
    public class NewFactCoInteract : UnitCoInteract
    {
        public string FactName { get; }
        public object FactValue { get; }

        public NewFactCoInteract(string factName, object factValue)
        {
            FactName = factName;
            FactValue = factValue;
        }
       

        
    }
}