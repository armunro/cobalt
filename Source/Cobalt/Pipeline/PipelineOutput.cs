using System;
using System.Collections.Generic;
using Cobalt.Pipeline.Channel;

namespace Cobalt.Pipeline
{
    public class PipelineOutput
    {
        public  Dictionary<Type, OutputChannel> OutputChannels { get; set; }

        public PipelineOutput()
        {
            OutputChannels = new Dictionary<Type, OutputChannel>();
        }
    }
}