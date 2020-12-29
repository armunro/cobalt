using System.Collections.Generic;

namespace Cobalt.Interaction
{
    public class CoInteractPath<TWith>
    {
        public List<CoInteract> Interactions { get; set; } = new List<CoInteract>();

        public CoInteractPath<TWith> Interact()
        {
            return new CoInteractPath<TWith>();
        }
    }
}