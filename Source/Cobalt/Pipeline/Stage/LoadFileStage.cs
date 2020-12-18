using System.IO;
using System.Net;
using Cobalt.Interaction.Unit;
using Cobalt.Pipeline.Steps;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Stage
{
    public class LoadFileStage : Steps.Stage
    {
        public string FilePath { get; set; }
        public string TargetFact { get; set; }

        public override CobaltUnit RunStage(CobaltUnit unit)
        {
            unit.Interact(interactions => interactions);
            return unit;
        }
    }
}