namespace Cobalt.Unit.Fact
{
    public class FactName
    {
        private string _name;

        public FactName(string name)
        {
            _name = name;
        }

        public static FactName Create(string name)
        {
            return new FactName(name);
        }
    }
}