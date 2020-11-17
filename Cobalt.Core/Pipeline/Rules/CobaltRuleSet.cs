using Cobalt.Pipeline.Rules.Builder;

namespace Cobalt.Pipeline.Rules
{
    public abstract class CobaltRuleSet<TTarget>
    {
        protected CobaltRuleBuilder<TTarget> Specify()
        {
            return new CobaltRuleBuilder<TTarget>();
        }
    }
}