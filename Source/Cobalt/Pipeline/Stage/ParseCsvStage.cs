using Cobalt.Interaction;

namespace Cobalt.Pipeline.Stage
{
    public class ParseCsvStage : Stage
    {
        public string FromFact { get; set; }
        public string ToFact { get; set; }
        public override InteractionPlan<Stage> PrepareInteractionPlan(Unit.Unit unit) => 
            new InteractionPlan<Stage>();

        public override StageDescription Describe() =>
            new StageDescription()
                .Named("Convert CSV string to UnitSet")
                .That("Converts a string containing Comma-separated-values to a Unit")
                .Require(
                    () => FromFact,
                    () => ToFact);
    }
}