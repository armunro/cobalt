using Cobalt.Unit;

namespace Cobalt.Interaction.Unit
{
    public class CreateFactInteraction : UnitInteraction
    {
        private readonly string _factName;
        private readonly object _factValue;

        public CreateFactInteraction(string factName, object factValue)
        {
            _factName = factName;
            _factValue = factValue;
        }
        public override void Run(CobaltUnit unit)
        {
            
        }
    }
}