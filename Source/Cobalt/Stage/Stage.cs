using Cobalt.Interaction;

namespace Cobalt.Stage
{
    public abstract class Stage 
    {

        public abstract CoInteractPath<Stage> PrepareInteractionPlan(Unit.CoUnit coUnit);

        public Stage()
        {
        }



    }
}