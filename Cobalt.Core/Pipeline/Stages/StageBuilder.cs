using System.Collections.Generic;
using Cobalt.Pipeline.Operation;

namespace Cobalt.Pipeline.Stages
{
    public class StageBuilder
    {
        private List<CobaltStep> Operations { get; set; }

        public StageBuilder()
        {
            Operations = new List<CobaltStep>();
        }
        
        public StageBuilder Op(CobaltStep step)
        {
            Operations.Add(step);
            return this;
        }
    }
}