using System.Collections.Generic;
using System.Threading.Tasks;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Channel
{
    public abstract class OutputChannel
    {
        public abstract Task OutputDataAsync(IEnumerable<CobaltUnit> units);
    }
}