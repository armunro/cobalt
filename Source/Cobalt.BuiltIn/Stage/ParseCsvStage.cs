using Cobalt.Interaction;

namespace Cobalt.BuiltIn.Stage
{
    public class ParseCsvStage : Cobalt.Stage.Stage
    {
        public string FromFact { get; set; }
        public string ToFact { get; set; }

        public override CoInteractPath<Cobalt.Stage.Stage> PrepareInteractionPlan(Unit.CoUnit coUnit) =>
            new CoInteractPath<Cobalt.Stage.Stage>();

        // public override StageDescription Describe() =>
        //     new StageDescription()
        //         .Named("Convert CSV string to UnitSet")
        //         .That("Converts a string containing Comma-separated-values to a Unit")
        //         .Require(() => FromFact, () => ToFact);
    }
}