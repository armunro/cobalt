using System.Collections.Generic;
using System.IO;
using Cobalt.BuiltIn.Interaction.Unit;

namespace Cobalt.BuiltIn.Stage
{
    public class LoadFileCoStage : CoStage
    {
        public string FilePath { get; set; }
        public string ToFact { get; set; }


        public override CoChangeSet<CoStage> PrepareInteractionPlan(CoUnit coUnit)
        {
            return new CoChangeSet<CoStage>()
            {
                Interactions = new List<CoChange>()
                {
                    new NewFactCoChange(ToFact, File.ReadAllText(FilePath))
                }
            };
        }
        
    }
}