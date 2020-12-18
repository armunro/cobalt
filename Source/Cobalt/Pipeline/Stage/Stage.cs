using Cobalt.Interaction.Unit;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Steps
{
    public abstract class Stage
    {
        private UnitInteraction _unitInteractions;
        public abstract CobaltUnit RunStage(CobaltUnit unit);
    }
}