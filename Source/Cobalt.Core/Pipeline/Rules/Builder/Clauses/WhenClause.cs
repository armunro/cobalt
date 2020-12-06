using System;

namespace Cobalt.Pipeline.Rules.Builder.Clauses
{
    public class WhenClause<TRootProduct>
    {
        public Func<TRootProduct, bool> Func { get; set; }
    }
}