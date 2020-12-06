namespace Cobalt.Pipeline.Rules
{
    public class CobaltRule<TTarget>
    {
        public CobaltRule(CobaltRuleFunction<TTarget> cobaltRuleFunction)
        {
            CobaltRuleFunction = cobaltRuleFunction;
        }

        private CobaltRuleFunction<TTarget> CobaltRuleFunction { get; set; }

        protected CobaltRule<TTarget> FromDelegate(CobaltRuleFunction<TTarget> cobaltRuleFunction)
        {
            CobaltRuleFunction = cobaltRuleFunction;
            return this;
        }

        public CobaltRule<TTarget> WithSeverity()
        {
            return this;
        }

        public CobaltRuleResult IsSatisfiedBy(TTarget target)
        {
            return CobaltRuleFunction.Invoke(target);
        }
    }
}