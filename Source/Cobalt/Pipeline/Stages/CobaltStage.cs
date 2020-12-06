using System.Collections.Generic;
using Cobalt.Pipeline.Operation;

namespace Cobalt.Pipeline.Stages
{
    public  class CobaltStage
    {
        public List<CobaltStep> Steps { get; }

        // [ctor]
        public CobaltStage()
        {
            Steps = new List<CobaltStep>();
        }
    }
}