using System.Collections.Generic;
using System.Threading.Tasks;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Channel
{
    public abstract class InputChannel
    {
        public abstract Task<IEnumerable<CobaltUnit>> GetDataAsync();
    }
}