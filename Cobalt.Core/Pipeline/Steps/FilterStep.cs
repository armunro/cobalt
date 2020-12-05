using System;
using System.Linq.Expressions;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Operation
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