namespace Cobalt.BuiltIn.Changes.Unit
{
    public class NewFactChange : UnitChange
    {
        public string FactName { get; }
        public object FactValue { get; }

        public NewFactChange(string factName, object factValue)
        {
            FactName = factName;
            FactValue = factValue;
        }
    }
}