using System;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Steps.Local
{
    public class ChangeStep : CobaltStep
    {
        private readonly Action<CobaltUnit> _changeAction;

        public ChangeStep(Action<CobaltUnit> changeAction)
        {
            _changeAction = changeAction;
        }
        
        public override CobaltUnit ExecuteStep(CobaltUnit unit)
        {
            _changeAction(unit);
            return unit;
        }
    }
}