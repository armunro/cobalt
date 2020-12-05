using System.Collections.Generic;
using Cobalt.Pipeline.Operation;

namespace Cobalt.Pipeline.Stages
{
    public abstract class CobaltStage
    {
        public List<CobaltStep> Operations { get; set; } = new List<CobaltStep>();
    }
}