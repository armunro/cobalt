namespace Cobalt.Interaction.Unit
{
    public class NewFactInteraction : UnitInteraction
    {
        public string FactName { get; }
        public object FactValue { get; }

        public NewFactInteraction(string factName, object factValue)
        {
            FactName = factName;
            FactValue = factValue;
        }
       

        
    }
}