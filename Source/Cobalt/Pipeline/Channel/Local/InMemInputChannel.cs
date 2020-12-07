using System.Collections.Generic;
using System.Threading.Tasks;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Channel.Local
{
    public class InMemInputChannel : InputChannel
    {
        private readonly List<CobaltUnit> _units;

        public InMemInputChannel()
        {
            _units = new List<CobaltUnit>();
        }
        
        public InMemInputChannel(params CobaltUnit[] units)
        {
            _units = new List<CobaltUnit>(units);
        }


        public void AddUnit(CobaltUnit unit)
        {
            _units.Add(unit);
        }

        public void AddUnits(params CobaltUnit[] units)
        {
            _units.AddRange(units);
        }


        public override Task<IEnumerable<CobaltUnit>> InputDataAsync()
        {
            return Task.FromResult((IEnumerable<CobaltUnit>) _units);
        }
    }
}