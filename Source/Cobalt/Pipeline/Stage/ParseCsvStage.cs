using System;
using Cobalt.Pipeline.Steps;
using Cobalt.Unit;

namespace Cobalt.Console
{
    public class ParseCsvStage : Stage
    {
        public string Delimiter { get; set; }
        
        public override CobaltUnit RunStage(CobaltUnit unit)
        {
            unit.Interact();
        }

       
    }
}