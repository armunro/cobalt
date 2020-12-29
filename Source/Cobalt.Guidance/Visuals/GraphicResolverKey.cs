using System.Collections.Generic;
using Cobalt.Resolver;

namespace Cobalt.Guidance.Visuals
{
    public class GraphicResolverKey : ResolverKey
    {
        public string GraphicName { get; }

        public GraphicResolverKey(string graphicName)
        {
            GraphicName = graphicName;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return GraphicName;
        }
    }
}