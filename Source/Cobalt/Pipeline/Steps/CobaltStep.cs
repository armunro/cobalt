using Cobalt.Pipeline.Steps.Interaction;

namespace Cobalt.Pipeline.Steps
{
    public abstract class CobaltStep
    {
        private IStepInteraction _interaction;
        private StepTarget _targetSelector;
        public CobaltStep()
        {
            
        }

        public abstract void ExecuteStep();

    }
}