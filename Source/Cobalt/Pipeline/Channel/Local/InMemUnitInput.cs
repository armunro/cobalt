using System.Collections.Generic;
using System.Threading.Tasks;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Channel.Local
{
    public class InMemUnitInput : UnitInput
    {
        private readonly List<CobaltUnit> _units;

        public InMemUnitInput()
        {
            _units = new List<CobaltUnit>();
        }
        
        public InMemUnitInput(params CobaltUnit[] units)
        {
            _units = new List<CobaltUnit>(units);
        }

        public List<CobaltUnit> Units => _units;


        public void AddUnit(CobaltUnit unit)
        {
            _units.Add(unit);
        }

        public void AddUnits(params CobaltUnit[] units)
        {
            _units.AddRange(units);
        }


        public override Task<IEnumerable<CobaltUnit>> InputUnitsAsync()
        {
            return Task.FromResult((IEnumerable<CobaltUnit>) _units);
        }
    }
}