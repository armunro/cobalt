namespace Cobalt.BuiltIn.Stage
{
    public class ParseCsvCoStage : CoStage
    {
        public string FromFact { get; set; }
        public string ToFact { get; set; }

        public override CoChangeSet<CoStage> PrepareInteractionPlan(CoUnit coUnit) =>
            new CoChangeSet<CoStage>();

        // public override StageDescription Describe() =>
        //     new StageDescription()
        //         .Named("Convert CSV string to UnitSet")
        //         .That("Converts a string containing Comma-separated-values to a Unit")
        //         .Require(() => FromFact, () => ToFact);
    }
}