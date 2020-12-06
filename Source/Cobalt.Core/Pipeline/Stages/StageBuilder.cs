using System.Collections.Generic;
using Cobalt.Pipeline.Operation;

namespace Cobalt.Pipeline.Stages
{
    public class StageBuilder
    {
        private List<CobaltStep> Operations { get; set; }

        // [ctor]
        public StageBuilder()
        {
            Operations = new List<CobaltStep>();
        }
        
        public StageBuilder Step(CobaltStep step)
        {
            Operations.Add(step);
            return this;
        }

        public CobaltStage BuildStage()
        {
            return new CobaltStage();
        }
    }
}