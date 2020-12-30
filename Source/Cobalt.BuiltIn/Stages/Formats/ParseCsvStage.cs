using Cobalt.Core;

namespace Cobalt.BuiltIn.Stages.Formats
{
    public class ParseCsvStage : Stage
    {
        public string FromFact { get; set; }
        public string ToFact { get; set; }

        public override ChangeSet<Stage> PrepareInteractionPlan(Unit unit) =>
            new ChangeSet<Stage>();

    
    }
}