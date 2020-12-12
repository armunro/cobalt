using System.Collections.Generic;
using System.Threading.Tasks;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Channel
{
    public abstract class UnitInput
    {
        public abstract Task<IEnumerable<CobaltUnit>> InputUnitsAsync();
    }
}