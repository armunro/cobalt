using System;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Steps.Interaction.Local
{
    public class FilterStep : CobaltStep
    {
        private readonly Predicate<CobaltUnit> _filter;

        public FilterStep(Predicate<dynamic> filter)
        {
            _filter = filter;
        }

        public override void ExecuteStep()
        {
            
        }
    }
}