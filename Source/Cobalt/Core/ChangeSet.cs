using System.Collections.Generic;

namespace Cobalt
{
    public class ChangeSet<TWith>
    {
        public List<Change> Changes { get; set; } = new List<Change>();

        public ChangeSet<TWith> ProvideChangeset()
        {
            return new ChangeSet<TWith>();
        }
    }
}