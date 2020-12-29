using System.Collections.Generic;
using System.IO;
using Cobalt.BuiltIn.Interaction.Unit;

namespace Cobalt.BuiltIn.Stage
{
    public class LoadFileStage : Cobalt.Stage
    {
        public string FilePath { get; set; }
        public string ToFact { get; set; }


        public override ChangeSet<Cobalt.Stage> PrepareInteractionPlan(Unit unit)
        {
            return new ChangeSet<Cobalt.Stage>()
            {
                Changes = new List<Change>()
                {
                    new NewFactChange(ToFact, File.ReadAllText(FilePath))
                }
            };
        }
        
    }
}