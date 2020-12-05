using System.Collections.Generic;
using System.Threading.Tasks;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Channel.Local
{
    public class InMemOutputChannel : OutputChannel
    {
        public override Task WriteDataAsync(IEnumerable<CobaltUnit> units)
        {
            throw new System.NotImplementedException();
        }
    }
}