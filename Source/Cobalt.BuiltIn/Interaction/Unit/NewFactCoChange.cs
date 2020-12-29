namespace Cobalt.BuiltIn.Interaction.Unit
{
    public class NewFactCoChange : UnitCoChange
    {
        public string FactName { get; }
        public object FactValue { get; }

        public NewFactCoChange(string factName, object factValue)
        {
            FactName = factName;
            FactValue = factValue;
        }
       

        
    }
}