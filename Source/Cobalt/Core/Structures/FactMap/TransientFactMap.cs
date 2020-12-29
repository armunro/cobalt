using System;
using System.Collections.Generic;
using Cobalt.Core.Structures.FactMap.Node;

namespace Cobalt.Core.Structures.FactMap
{
    public sealed class TransientFactMap: FactMap, IEnumerable<KeyValuePair<string, object>>
    {
        internal TransientFactMap(FactMapNode rootNode, int nodeCount)
            : base(rootNode, nodeCount, new FactMapVersionRef())
        {

        }

        public PersistentFactMap AsPersistent()
        {
            FactMapVersionRef = null;
            if (NodeCount == 0) return PersistentFactMap.Empty;
            return new PersistentFactMap(RootNode, NodeCount);
        }

        #region Main functionality
        public void Add(string factName, object factValue)
        {
            if (FactMapVersionRef == null)
                throw new NotSupportedException("Transient dictionary cannot be modified after call AsPersistent() method");

            var set = FactNodeChangeState.ChangedItem | FactNodeChangeState.NewItem;

            if (IsEmpty)
            {
                var idx = factName.GetHashCode() & 0x01f;
                RootNode = CreateValueNode(idx, factName, factValue, FactMapVersionRef);
            }
            else
                RootNode = Adding(0, RootNode, (uint) factName.GetHashCode(), factName, factValue, ref set);

            if (set.HasFlag(FactNodeChangeState.NewItem)) 
                NodeCount++;
        }

        public void Add<T>(IEnumerable<T> e, Func<T, string> key, Func<T, object> val)
        {
            foreach (var item in e) 
                Add(key(item), val(item));
        }

        public void Remove(string key)
        {
            if (FactMapVersionRef == null)
                throw new NotSupportedException("Transient dictionary cannot be modified after call AsPersistent() method");

            if (NodeCount == 0) 
                throw new KeyNotFoundException("The persistent dictionary doesn't contain value associated with specified key");

            var newRoot = Removing(0, RootNode, (uint)key.GetHashCode(), key);
            NodeCount--;

            if (newRoot.ValueCount == 1) newRoot = newRoot.MakeRoot(FactMapVersionRef);
            else if (newRoot.ValueCount == 0 && newRoot.ReferenceCount == 0) newRoot = null;

            RootNode = newRoot;
        }
        #endregion

        public object this[string i]
        {
            get => GetValue(i);
            set => Add(i, value);
        }
    }
}
