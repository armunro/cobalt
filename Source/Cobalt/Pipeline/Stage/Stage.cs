using System;
using Cobalt.Guidance.Diagrams;
using Cobalt.Guidance.Text.Descriptions;
using Cobalt.Interaction;

namespace Cobalt.Pipeline.Stage
{
    public abstract class Stage : IGivesDescription<Stage>, IGivesVisual<Stage>
    {

        public abstract InteractionPlan<Stage> PrepareInteractionPlan(Unit.Unit unit);
        public abstract StageDescription Describe();
        public Visual<Stage> Visualize() => new Visual<Stage>();

       
    }
}