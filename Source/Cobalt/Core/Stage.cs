namespace Cobalt
{
    public abstract class Stage 
    {

        public abstract ChangeSet<Stage> PrepareInteractionPlan(Unit unit);

        public Stage()
        {
        }



    }
}