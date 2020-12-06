using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Cobalt.Unit;

namespace Cobalt.Pipeline.Channel.Local
{
    public class InMemInputChannel : InputChannel
    {
        private readonly List<CobaltUnit> _items;
        public InMemInputChannel()
        {
            _items = new List<CobaltUnit>();
        }

        public void AddUnit(CobaltUnit unit)
        {
            _items.Add(unit);
        }

        public void AddUnits(params CobaltUnit[] units)
        {
            _items.AddRange(units);
        }
        
       

  
        
        public override Task<IEnumerable<CobaltUnit>> GetDataAsync()
        {
            return Task.FromResult((IEnumerable<CobaltUnit>) _items);
        }
    }
}