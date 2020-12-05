using Cobalt.Pipeline.Rules.Builder;

namespace Cobalt.Pipeline.Rules
{
    public abstract class CobaltRuleSet<TTarget>
    {
        protected CobaltRuleBuilder<TTarget> Rule()
        {
            return new CobaltRuleBuilder<TTarget>();
        }
    }
}