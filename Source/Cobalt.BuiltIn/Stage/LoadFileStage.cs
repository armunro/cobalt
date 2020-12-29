﻿using System.Collections.Generic;
using System.IO;
using Cobalt.BuiltIn.Interaction.Unit;
using Cobalt.Interaction;

namespace Cobalt.BuiltIn.Stage
{
    public class LoadFileStage : Cobalt.Stage.Stage
    {
        public string FilePath { get; set; }
        public string ToFact { get; set; }


        public override CoInteractPath<Cobalt.Stage.Stage> PrepareInteractionPlan(Unit.CoUnit coUnit)
        {
            return new CoInteractPath<Cobalt.Stage.Stage>()
            {
                Interactions = new List<Cobalt.Interaction.CoInteract>()
                {
                    new NewFactCoInteract(ToFact, File.ReadAllText(FilePath))
                }
            };
        }
        
    }
}