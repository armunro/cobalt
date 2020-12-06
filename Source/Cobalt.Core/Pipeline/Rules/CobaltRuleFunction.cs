namespace Cobalt.Pipeline.Rules
{
    public delegate CobaltRuleResult CobaltRuleFunction<in TTarget>(TTarget target);
}