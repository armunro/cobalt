using System.Collections.Generic;
using Cobalt.Pipeline.Operation;

namespace Cobalt.Pipeline.Stages
{
    public  class CobaltStage
    {
        public List<CobaltStep> Steps { get; set; } = new List<CobaltStep>();
    }
}