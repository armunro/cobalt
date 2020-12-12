using System;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Steps
{
    public class InlineStep : CobaltStep
    {
        private readonly Func<CobaltUnit, CobaltUnit> _stepFunc;

        public InlineStep(Func<CobaltUnit, CobaltUnit> stepFunc)
        {
            _stepFunc = stepFunc;
        }
        public override CobaltUnit ExecuteStep(CobaltUnit unit)
        {
            return _stepFunc(unit);
        }
    }
}