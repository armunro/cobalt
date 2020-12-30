using System.Collections.Generic;
using Cobalt.Sourcing;

namespace Cobalt.BuiltIn.Sourcing.Graphics
{
    public class GraphicSourceKey : ConnectKey
    {
        public string GraphicName { get; }

        public GraphicSourceKey(string graphicName)
        {
            GraphicName = graphicName;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return GraphicName;
        }
    }
}