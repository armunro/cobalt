using Cobalt.Unit;

namespace Cobalt.Pipeline.Steps
{
    public abstract class CobaltStep
    {

        public abstract CobaltUnit ExecuteStep(CobaltUnit unit);
    }
}