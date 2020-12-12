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
            HashSet<CobaltUnit> returnSet = new HashSet<CobaltUnit>();
            List<IEnumerable<CobaltUnit>> inputUnitSets = new List<IEnumerable<CobaltUnit>>();
            List<IEnumerable<CobaltUnit>> outputUnitSets = new List<IEnumerable<CobaltUnit>>();

            foreach (var channel in Inputs)
            {
                inputUnitSets.Add(await channel.InputUnitsAsync());
            }

            foreach (var inputUnitSet in inputUnitSets)
            {
                List<CobaltUnit> newUnits = new List<CobaltUnit>();
                foreach (CobaltUnit unit in inputUnitSet)
                {
                    foreach (var stage in Stages)
                    {
                        foreach (var step in stage.Steps)
                        {
                            step.ExecuteStep(unit);
                        }
                    }
                    newUnits.Add(unit);
                }
                outputUnitSets.Add( newUnits);
            }

            foreach (var channel in Outputs)
            {
                foreach (var outputUnitSet in outputUnitSets)
                {
                    channel.OutputUnitsAsync(outputUnitSet);
                }
            }
        }

        public CobaltPipeline Stage(string name, Action<StageBuilder> stageBuilder)
        {
            StageBuilder builder = new StageBuilder();
            stageBuilder.Invoke(builder);

            CobaltStage stage = builder.BuildStage();
            Stages.Add(stage);
            return this;
        }

        public CobaltPipeline In(UnitInput @in, string name = "default")
        {
            Inputs.Add( @in);
            return this;
        }

        public CobaltPipeline Out(InMemUnitOutput @out, string name = "default")
        {
            Outputs.Add(@out);
            return this;
        }
    }
}