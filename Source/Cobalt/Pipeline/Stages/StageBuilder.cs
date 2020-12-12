using System;
using System.Collections.Generic;
using Cobalt.Pipeline.Steps;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Stages
{
    public class StageBuilder
    {
        public string Name { get; set; }
        public string Description { get; set; }
        private List<CobaltStep> Steps { get; set; }

        // [ctor]
        public StageBuilder()
        {
            Steps = new List<CobaltStep>();
        }


        public StageBuilder Step(CobaltStep step)
        {
            Steps.Add(step);
            return this;
        }

        public StageBuilder Step(Func<CobaltUnit, CobaltUnit> inlineStep)
        {
            Steps.Add(new InlineStep(inlineStep));
            return this;
        }


        public CobaltStage BuildStage()
        {
            CobaltStage newStage = new CobaltStage(Steps);

            return newStage;
        }
    }
}