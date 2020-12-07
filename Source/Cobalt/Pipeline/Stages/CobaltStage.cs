using System.Collections.Generic;
using Cobalt.Pipeline.Steps;

namespace Cobalt.Pipeline.Stages
{
    public  class CobaltStage
    {
        public List<CobaltStep> Steps { get; }

        // [ctor]
        public CobaltStage(List<CobaltStep> steps)
        {
            Steps = steps;
        }
        
        
        public CobaltStage()
        {
           Steps = new List<CobaltStep>();
        }
    }
}