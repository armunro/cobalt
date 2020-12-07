using System.Collections.Generic;
using System.Threading.Tasks;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Channel.Local
{
    public class InMemOutputChannel : OutputChannel
    {
        private readonly List<CobaltUnit> _units;

        public InMemOutputChannel()
        {
            _units = new List<CobaltUnit>();
        }

        public List<CobaltUnit> Units
        {
            get => _units;
        }

        public override async Task OutputDataAsync(IEnumerable<CobaltUnit> units)
        {
            foreach (CobaltUnit unit in units)
            {
                _units.Add(unit);
            }
            
        }
    }
}