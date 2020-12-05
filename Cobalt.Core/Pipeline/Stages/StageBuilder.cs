using System.Collections.Generic;
using Cobalt.Pipeline.Operation;

namespace Cobalt.Pipeline.Stages
{
    public class StageBuilder
    {
        private List<CobaltOperation> Operations { get; set; }

        public StageBuilder()
        {
            Operations = new List<CobaltOperation>();
        }
        
        public StageBuilder Operation(CobaltOperation operation)
        {
            Operations.Add(operation);
            return this;
        }
    }
}