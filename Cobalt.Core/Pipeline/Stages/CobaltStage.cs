using System.Collections.Generic;
using Cobalt.Pipeline.Work;

namespace Cobalt.Pipeline.Stages
{
    public abstract class CobaltStage
    {
        public List<CobaltWork> Work { get; set; } = new List<CobaltWork>();
        protected abstract void ProcessStage();
    }
}