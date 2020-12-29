using Cobalt.Interaction;

namespace Cobalt.Stage
{
    public abstract class CoStage 
    {

        public abstract CoInteractPath<CoStage> PrepareInteractionPlan(Unit.CoUnit coUnit);

        public CoStage()
        {
        }



    }
}