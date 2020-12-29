using System.Collections.Generic;
using System.IO;
using Cobalt.BuiltIn.Interaction.Unit;
using Cobalt.Core;

namespace Cobalt.BuiltIn.Stages.Files
{
    public class LoadFileStage : Stage
    {
        public string FilePath { get; set; }
        public string ToFact { get; set; }


        public override ChangeSet<Stage> PrepareInteractionPlan(Unit unit)
        {
            return new ChangeSet<Stage>()
            {
                Changes = new List<Change>()
                {
                    new NewFactChange(ToFact, File.ReadAllText(FilePath))
                }
            };
        }
        
    }
}