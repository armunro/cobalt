using Cobalt.Interaction;

namespace Cobalt.BuiltIn.Stage
{
    public class ParseCsvCoStage : Cobalt.Stage.CoStage
    {
        public string FromFact { get; set; }
        public string ToFact { get; set; }

        public override CoInteractPath<Cobalt.Stage.CoStage> PrepareInteractionPlan(Unit.CoUnit coUnit) =>
            new CoInteractPath<Cobalt.Stage.CoStage>();

        // public override StageDescription Describe() =>
        //     new StageDescription()
        //         .Named("Convert CSV string to UnitSet")
        //         .That("Converts a string containing Comma-separated-values to a Unit")
        //         .Require(() => FromFact, () => ToFact);
    }
}