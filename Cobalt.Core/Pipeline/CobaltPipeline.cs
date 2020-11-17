using System;
using System.Threading.Tasks;
using Cobalt.Pipeline.Channel;
using Cobalt.Unit;

namespace Cobalt.Pipeline
{
    public class CobaltPipeline
    {
        public CobaltPipelineState State { get; } = new CobaltPipelineState();


        public CobaltPipeline Channel<TChannel, TOptions>(Action<TOptions> options)
            where TChannel : InputChannel where TOptions : new()
        {
            var newOptions = new TOptions();
            options(newOptions);

            var dataChannel = (InputChannel) Activator.CreateInstance(typeof(TChannel), newOptions);

            State.RegisterDataChannel<TChannel>(dataChannel);
            return this;
        }

        public async Task ExecuteAsync()
        {
            foreach (var channel in State.CompileChannels())
                State.FillSet(channel.GetType(),
                    new CobaltUnitSet(channel.Key.Name, await channel.Value.GetDataAsync()));
        }
    }
}