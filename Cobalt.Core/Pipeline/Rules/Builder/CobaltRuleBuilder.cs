using System;
using Cobalt.Pipeline.Rules.Builder.Clauses;

namespace Cobalt.Pipeline.Rules.Builder
{
    public class CobaltRuleBuilder<TTarget>
    {
        private readonly ResultClause _elseResult = new ResultClause();
        private readonly ResultClause _result = new ResultClause();
        private readonly ThenClause _then = new ThenClause();
        private readonly WhenClause<TTarget> _when = new WhenClause<TTarget>();


        public CobaltRuleBuilder<TTarget> When(Func<TTarget, bool> condition)
        {
            _when.Func = condition;
            return this;
        }

        public CobaltRuleBuilder<TTarget> When(bool condition)
        {
            _when.Func = target => condition;
            return this;
        }

        public CobaltRuleBuilder<TTarget> NotSatisfied()
        {
            _then.WillBeSatisfied = false;
            return this;
        }

        public CobaltRuleBuilder<TTarget> Then(string message)
        {
            _result.CobaltReason = new CobaltReason(true, message);
            return this;
        }

        public CobaltRuleBuilder<TTarget> Else(string message)
        {
            _elseResult.CobaltReason = new CobaltReason(true, message);
            return this;
        }


        public CobaltRule<TTarget> Build()
        {
            return new CobaltRule<TTarget>(target =>
            {
                var conditionResult = _when.Func.Invoke(target);

                //TODO CLEAN THIS UP
                if (conditionResult && _then.WillBeSatisfied)
                    return new CobaltRuleResult(_result.CobaltReason);
                if (!conditionResult && !_then.WillBeSatisfied)
                    return new CobaltRuleResult(_elseResult.CobaltReason);
                if (conditionResult && !_then.WillBeSatisfied)
                    return new CobaltRuleResult(_result.CobaltReason);
                if (!conditionResult && _then.WillBeSatisfied)
                    return new CobaltRuleResult(_result.CobaltReason);
                return null;
            });
        }
    }
}