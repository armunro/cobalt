using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cobalt.Pipeline.Channel;
using Cobalt.Pipeline.Channel.Local;
using Cobalt.Pipeline.Stages;
using Cobalt.Unit;

namespace Cobalt.Pipeline
{
    public class CobaltPipeline
    {
        public List<UnitInput> Inputs { get; set; }
        private List<CobaltStage> Stages { get; set; }
        public List<UnitOutput> Outputs { get; set; }


        public CobaltPipeline()
        {
            Inputs = new List<UnitInput>();
            Stages = new List<CobaltStage>();
            Outputs = new List<UnitOutput>();
        }


        public async Task ExecuteAsync()
        {
            var returnSet = new HashSet<CobaltUnit>();
            var inputUnitSets = new List<IEnumerable<CobaltUnit>>();
            var outputUnitSets = new List<IEnumerable<CobaltUnit>>();

            Inputs.ForEach(async x => inputUnitSets.Add(await x.InputUnitsAsync()));


            foreach (var inputUnitSet in inputUnitSets)
            {
                var newUnits = new List<CobaltUnit>();
                foreach (var unit in inputUnitSet)
                {
                    foreach (var stage in Stages) 
                        stage.Steps.ForEach(step => step.ExecuteStep(unit));
                    newUnits.Add(unit);
                }
                outputUnitSets.Add(newUnits);
            }

            Outputs.ForEach(unitOutput =>
                outputUnitSets.ForEach(unitSet =>
                    unitOutput.OutputUnitsAsync(unitSet)));
        }

        public CobaltPipeline Stage(string name, Action<StageBuilder> stageBuilder)
        {
            var builder = new StageBuilder();
            stageBuilder.Invoke(builder);

            var stage = builder.BuildStage();
            Stages.Add(stage);
            return this;
        }

        public CobaltPipeline In(UnitInput @in, string name = "default")
        {
            Inputs.Add(@in);
            return this;
        }

        public CobaltPipeline Out(UnitOutput @out, string name = "default")
        {
            Outputs.Add(@out);
            return this;
        }
    }
}