using Cobalt.Core;

namespace Cobalt.BuiltIn.Stages
{
    public class ParseCsvStage : Stage
    {
        public string FromFact { get; set; }
        public string ToFact { get; set; }

        public override ChangeSet<Stage> PrepareInteractionPlan(Unit unit) =>
            new ChangeSet<Stage>();

        // public override StageDescription Describe() =>
        //     new StageDescription()
        //         .Named("Convert CSV string to UnitSet")
        //         .That("Converts a string containing Comma-separated-values to a Unit")
        //         .Require(() => FromFact, () => ToFact);
    }
}