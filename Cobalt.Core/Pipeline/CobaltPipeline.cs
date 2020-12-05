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
        private  Dictionary<Type, CobaltUnitSet> DataSets { get; set; }
        private  Dictionary<Type, InputChannel> InputChannels { get; set; }
        private  List<CobaltStage> Stages { get; set; }


        public CobaltPipeline()
        {
            InputChannels = new Dictionary<Type, InputChannel>();
            DataSets = new Dictionary<Type, CobaltUnitSet>();
            Stages = new List<CobaltStage>();
        }


        private void RegisterDataChannel<T>(InputChannel channel)
        {
            InputChannels.Add(typeof(T), channel);
        }
        private void RegisterDataChannel(InputChannel channel)
        {
            InputChannels.Add(channel.GetType(), channel);
        }


        public void FillSet(Type channelType, CobaltUnitSet unitSet)
        {
            DataSets.Add(channelType, unitSet);
        }


        public CobaltPipeline Channel<TChannel, TOptions>(Action<TOptions> options)
            where TChannel : InputChannel where TOptions : new()
        {
            var newOptions = new TOptions();
            options(newOptions);

            var dataChannel = (InputChannel) Activator.CreateInstance(typeof(TChannel), newOptions);

            RegisterDataChannel<TChannel>(dataChannel);
            return this;
        }
        
        public CobaltPipeline Channel(InputChannel inputChannelInstance) 
        {
            RegisterDataChannel(inputChannelInstance);
            return this;
        }

        public async Task ExecuteAsync()
        {
            foreach (var channel in InputChannels)
                FillSet(channel.GetType(),
                    new CobaltUnitSet(channel.Key.Name, await channel.Value.GetDataAsync()));
        }

        public CobaltPipeline Stage(Action<StageBuilder> builder)
        {
            builder.Invoke(new StageBuilder());
            return this;
        }
    }
}