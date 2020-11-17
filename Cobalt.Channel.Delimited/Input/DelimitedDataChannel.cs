using Cobalt.Pipeline.Channel;

namespace Cobalt.Channel.Delimited.Input
{
    public abstract class DelimitedDataChannel<TOptions> : InputChannel
    {
        public DelimitedDataChannel(TOptions options)
        {
            Options = options;
        }

        protected TOptions Options { get; set; }
    }
}