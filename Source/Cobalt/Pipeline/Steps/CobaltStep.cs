using Cobalt.Fact;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Steps
{
    public abstract class CobaltStep
    {
        private UnitInteraction _unitInteractions;
        public abstract CobaltUnit ExecuteStep(CobaltUnit unit);
    }
}