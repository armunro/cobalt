using Cobalt.Unit;

namespace Cobalt.Pipeline.Steps
{
    public abstract class CobaltStep
    {
        private Interaction _interaction;
        public abstract CobaltUnit ExecuteStep(CobaltUnit unit);
    }
}