using System.Collections.Generic;
using Cobalt.Pipeline.Operation;

namespace Cobalt.Pipeline.Stages
{
    public abstract class CobaltStage
    {
        public List<CobaltOperation> Operations { get; set; } = new List<CobaltOperation>();
    }
}