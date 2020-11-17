using System.Collections.Generic;

namespace Cobalt.Unit
{
    public class CobaltUnitSet
    {
        public CobaltUnitSet(string globalName, IEnumerable<CobaltUnit> models)
        {
            GlobalName = globalName;
            Data.AddRange(models);
        }

        public string GlobalName { get; }
        public List<CobaltUnit> Data { get; } = new List<CobaltUnit>();
    }
}