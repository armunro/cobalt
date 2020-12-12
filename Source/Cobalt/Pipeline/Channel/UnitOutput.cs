using System.Collections.Generic;
using System.Threading.Tasks;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Channel
{
    public abstract class UnitOutput
    {
        public abstract Task OutputUnitsAsync(IEnumerable<CobaltUnit> units);
    }
}