namespace Cobalt
{
    public abstract class CoStage 
    {

        public abstract CoChangeSet<CoStage> PrepareInteractionPlan(CoUnit coUnit);

        public CoStage()
        {
        }



    }
}