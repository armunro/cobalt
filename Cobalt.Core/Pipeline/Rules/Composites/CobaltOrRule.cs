using System.Collections.Generic;
using System.Linq;

namespace Cobalt.Pipeline.Rules.Composites
{
    public class CobaltCompositeOrRule<T> : CobaltCompositeRule<T>
    {
        public CobaltCompositeOrRule(IEnumerable<CobaltRule<T>> specs) : base(specs)
        {
        }

        protected override CobaltRuleResult IsCompositeSatisfied(IEnumerable<CobaltRuleResult> results)
        {
            IEnumerable<CobaltRuleResult> specificationResults =
                results as CobaltRuleResult[] ?? results.ToArray();
            var overallSatisfied = specificationResults.Any(x => x.IsSatisfied);
            return new CobaltRuleResult(overallSatisfied, specificationResults.SelectMany(x => x.Explanations));
        }
    }
}