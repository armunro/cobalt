using System.Collections.Generic;

namespace Cobalt
{
    public class CoChangeSet<TWith>
    {
        public List<CoChange> Interactions { get; set; } = new List<CoChange>();

        public CoChangeSet<TWith> Interact()
        {
            return new CoChangeSet<TWith>();
        }
    }
}