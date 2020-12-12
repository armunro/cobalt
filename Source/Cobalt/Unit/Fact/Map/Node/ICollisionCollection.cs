using System.Collections.Generic;

namespace Cobalt.Unit.Fact.Map.Node
{
    internal interface ICollisionCollection : IEnumerable<KeyValuePair<string, object>>
    {
        int HashCode { get; }

        ICollisionCollection Add(string name, object item, FactMapVersionRef factMapVersionRef);

        ICollisionCollection Remove(string name, FactMapVersionRef factMapVersionRef);

        ICollisionCollection Change(string name, object value, FactMapVersionRef factMapVersionRef);

        KeyValuePair<string, object> GetItem(string name);

        bool HasItemWithKey(string name);

        int Count { get; }

        bool ContentEqual(ICollisionCollection c2);

        KeyValuePair<string, object> GetRemainingValue(string removedKey);
    }
}
