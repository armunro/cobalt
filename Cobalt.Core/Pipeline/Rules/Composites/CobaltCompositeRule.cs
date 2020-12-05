using System.Collections.Generic;
using System.Linq;

namespace Cobalt.Pipeline.Rules.Composites
{
    public abstract class CobaltCompositeRule<T> : CobaltRule<T>
    {
        protected CobaltCompositeRule(IEnumerable<CobaltRule<T>> specs) : base(null)
        {
            FromDelegate(target =>
            {
                var results = specs.Select(x => x.IsSatisfiedBy(target));
                return IsCompositeSatisfied(results);
            });
        }

        protected abstract CobaltRuleResult IsCompositeSatisfied(IEnumerable<CobaltRuleResult> results);
    }
}