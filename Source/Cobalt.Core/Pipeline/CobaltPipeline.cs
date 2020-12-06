using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cobalt.Pipeline.Channel;
using Cobalt.Pipeline.Stages;
using Cobalt.Unit;

namespace Cobalt.Pipeline
{
    public class CobaltPipeline
    {
        public PipelineInput Input { get; set; }
        private List<CobaltStage> Stages { get; set; }
        public PipelineOutput Output { get; set; }
        private Dictionary<Type, CobaltUnitSet> UnitSets { get; set; }


        public CobaltPipeline()
        {
            Input = new PipelineInput();
            Output = new PipelineOutput();

            Input.InputChannels = new Dictionary<Type, InputChannel>();
            UnitSets = new Dictionary<Type, CobaltUnitSet>();
            Stages = new List<CobaltStage>();
        }

        public CobaltPipeline Channel(InputChannel inputChannelInstance)
        {
            Input.InputChannels.Add(inputChannelInstance.GetType(), inputChannelInstance);
            return this;
        }

        public async Task ExecuteAsync()
        {
            //load all sets
            foreach (var channel in Input.InputChannels)
            {
                UnitSets.Add(channel.GetType(), new CobaltUnitSet(channel.Key.Name, await channel.Value.GetDataAsync()));
            }

            foreach (var stage in Stages)
            {
                foreach (var operation in stage.Steps)
                {
                    operation.ExecuteStep();
                }
            }
        }

        public CobaltPipeline Stage(Action<StageBuilder> stageBuilder)
        {
            StageBuilder builder = new StageBuilder();
            stageBuilder.Invoke(builder);

            CobaltStage stage = builder.BuildStage();
            Stages.Add(stage);
            return this;
        }
    }
}