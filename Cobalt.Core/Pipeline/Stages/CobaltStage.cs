using System.Collections.Generic;
using Cobalt.Pipeline.Work;

namespace Cobalt.Pipeline.Stages
{
    public abstract class CobaltStage
    {
        public List<CobaltOperation> Operations { get; set; } = new List<CobaltOperation>();
        protected abstract void ProcessStage();
    }
}