using System;
using System.Collections.Generic;
using Cobalt.Pipeline.Channel;

namespace Cobalt.Pipeline
{
    public class PipelineInput
    {
        public  Dictionary<Type, InputChannel> InputChannels { get; set; }

        public PipelineInput()
        {
            InputChannels = new Dictionary<Type, InputChannel>();
        }
    }
}