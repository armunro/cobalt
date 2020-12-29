using System.Collections.Generic;

namespace Cobalt.Core
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