using System.Collections.Generic;
using System.IO;
using Cobalt.Interaction;
using Cobalt.Interaction.Unit;

namespace Cobalt.Pipeline.Stage
{
    public class LoadFileStage : Stage
    {
        public string FilePath { get; set; }
        public string ToFact { get; set; }


        public override InteractionPlan<Stage> PrepareInteractionPlan(Unit.Unit unit)
        {
            return new InteractionPlan<Stage>()
            {
                Interactions = new List<Interaction.Interaction>()
                {
                    new NewFactInteraction(ToFact, File.ReadAllText(FilePath))
                }
            };
        }

        public override StageDescription Describe() => new StageDescription()
            .Named("Load file into unit")
            .That("Reads all of the content of a local file into a unit fact.")
            .Require(() => ToFact);
    }
}