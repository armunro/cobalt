namespace Cobalt.BuiltIn.Stage
{
    public class ParseCsvStage : Cobalt.Stage
    {
        public string FromFact { get; set; }
        public string ToFact { get; set; }

        public override ChangeSet<Cobalt.Stage> PrepareInteractionPlan(Unit unit) =>
            new ChangeSet<Cobalt.Stage>();

        // public override StageDescription Describe() =>
        //     new StageDescription()
        //         .Named("Convert CSV string to UnitSet")
        //         .That("Converts a string containing Comma-separated-values to a Unit")
        //         .Require(() => FromFact, () => ToFact);
    }
}