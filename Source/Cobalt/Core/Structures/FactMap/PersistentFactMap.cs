using System;
using System.Collections.Generic;
using Cobalt.Core.Structures.FactMap.Node;

namespace Cobalt.Core.Structures.FactMap
{
    [Serializable]
    public class PersistentFactMap : FactMap, IEnumerable<KeyValuePair<string, object>>, IEquatable<PersistentFactMap>
    {
        public static PersistentFactMap Empty { get; } = new PersistentFactMap(null, 0);


        internal PersistentFactMap(FactMapNode rootNode, int nodeCount)
            :base(rootNode, nodeCount) { }

        
        public PersistentFactMap Add(string factName, object factValue)
        {
            if (NodeCount == 0)
            {
                var idx = factName.GetHashCode() & 0x01f;
                return new PersistentFactMap(CreateValueNode(idx, factName, factValue), 1);
            }

            var set = FactNodeChangeState.ChangedItem | FactNodeChangeState.NewItem;
            var newRoot = Adding(0, RootNode, (uint)factName.GetHashCode(), factName, factValue, ref set);

            return new PersistentFactMap(newRoot, NodeCount + ((set.HasFlag(FactNodeChangeState.NewItem)) ? 1 : 0));
        }

        public PersistentFactMap Add<T>(IEnumerable<T> e, Func<T, string> key, Func<T,object> val)
        {
            var d = Empty.AsTransient();
            d.Add(e, key, val);

            return d.AsPersistent();
        }

        public PersistentFactMap Remove(string key)
        {
            if (NodeCount == 0) 
                throw new KeyNotFoundException("The persistent dictionary doesn't contain value associated with specified key"); ;

            var newRoot = Removing(0, RootNode, (uint)key.GetHashCode(), key);

            if (NodeCount == 1) return Empty;
            else if (newRoot.ValueCount == 1 && newRoot.ReferenceCount == 0) 
                newRoot = newRoot.MakeRoot(FactMapVersionRef);

            return new PersistentFactMap(newRoot, NodeCount - 1);
        }

        public TransientFactMap AsTransient() => new TransientFactMap(RootNode, NodeCount);

        //base - object
        public override int GetHashCode() => (RootNode != null) ? RootNode.GetHashCode() : 0;

        //base - object
        public override bool Equals(object obj)
        {
            var dict = obj as PersistentFactMap;

            return dict != null && Equals(dict);
        }

        //IEquatable
        public bool Equals(PersistentFactMap other)
        {
            if (NodeCount != other.NodeCount) return false;

            if ((RootNode != null) == (other.RootNode != null))
            {
                if (RootNode != null)
                    return RootNode.Equals(other.RootNode);

                return true;
            }
            return false;
        }
        
        // indexer
        public object this[string i] => GetValue(i);

        // equals operator
        public static bool operator !=(PersistentFactMap a, PersistentFactMap b) => !(a == b);
        
        // equals operator
        public static bool operator ==(PersistentFactMap a, PersistentFactMap b)
        {
            if ((object)a == null == ((object)b == null))
            {
                if ((object)a != null) return a.Equals(b);
            }
            else return false;

            return true;
        }
    }
    
    
}
