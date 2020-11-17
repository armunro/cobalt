using System;
using System.Collections.Generic;
using Cobalt.Pipeline.Channel;
using Cobalt.Pipeline.Stages;
using Cobalt.Unit;

namespace Cobalt.Pipeline
{
    public class CobaltPipelineState
    {
        private readonly Dictionary<Type, InputChannel> _inputChannels = new Dictionary<Type, InputChannel>();
        public readonly Dictionary<Type, CobaltUnitSet> DataSets = new Dictionary<Type, CobaltUnitSet>();
        public readonly List<CobaltStage> Stages = new List<CobaltStage>();


        public void RegisterDataChannel<T>(InputChannel channel)
        {
            _inputChannels.Add(typeof(T), channel);
        }

        public Dictionary<Type, InputChannel> CompileChannels()
        {
            return _inputChannels;
        }

        public void FillSet(Type channelType, CobaltUnitSet unitSet)
        {
            DataSets.Add(channelType, unitSet);
        }
    }
}