using System.Collections.Generic;

namespace Cobalt.Pipeline.Rules
{
    public class CobaltRuleResult
    {
        public List<CobaltReason> Explanations;
        public bool IsSatisfied;

        public CobaltRuleResult(bool isSatisfied, IEnumerable<CobaltReason> explanations)
        {
            Explanations = new List<CobaltReason>(explanations);
            IsSatisfied = isSatisfied;
        }

        public CobaltRuleResult(CobaltReason cobaltReason)
            : this(cobaltReason.IsSatisfied, new List<CobaltReason> {cobaltReason})
        {
        }
    }
}