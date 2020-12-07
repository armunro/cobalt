using System;
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
        public PipelineInput Input { get; set; }
        private List<CobaltStage> Stages { get; set; }
        public PipelineOutput Output { get; set; }
        private Dictionary<Type, CobaltUnitSet> UnitSets { get; set; }


        public CobaltPipeline()
        {
            Input = new PipelineInput();
            Stages = new List<CobaltStage>();
            Output = new PipelineOutput();
            UnitSets = new Dictionary<Type, CobaltUnitSet>();

        }



        public async Task ExecuteAsync()
        {
            //load all sets
            foreach (var channel in Input.InputChannels)
            {
                UnitSets.Add(channel.GetType(), new CobaltUnitSet(channel.Key.Name, await channel.Value.InputDataAsync()));
            }

            foreach (var stage in Stages)
            {
                foreach (var operation in stage.Steps)
                {
                    operation.ExecuteStep();
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

        public CobaltPipeline In(InputChannel inputChannelInstance)
        {
            Input.InputChannels.Add(inputChannelInstance.GetType(), inputChannelInstance);
            return this;
        }
        
        public CobaltPipeline Out(InMemOutputChannel outputChannel)
        {
            Output.OutputChannels.Add(outputChannel.GetType(), outputChannel);
            return this;
        }
    }
}