using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cobalt.Unit.Fact.Map.Node;

namespace Cobalt.Unit.Fact.Map
{
    [Serializable]
    public abstract  class FactMap<V> : IEnumerable<KeyValuePair<FactName, V>>
    {
        
       
        
        internal FactMapNode<V> _rootFactNode;
        protected int _factNodeCount;

        [NonSerialized]
        internal VersionID _versionID;
        public bool IsEmpty => _factNodeCount == 0;
        public int FactNodeCount => _factNodeCount;
        internal FactMapNode<V> CreateValueNode(int idx, FactName key, V value, VersionID versionID = null)
        {
            return new FactMapNode<V>(idx, key, value, versionID);
        }

        internal FactMap(FactMapNode<V> rootFactNode, int factNodeCount, VersionID versionID = null)
        {
            _rootFactNode = rootFactNode;
            _factNodeCount = factNodeCount;
            _versionID = versionID;
        }

        internal FactMapNode<V> CreateCommonPath(uint h1, uint h2, int i, int shift, FactMapNode<V> node, FactName key, V value)
        {
            var i1 = (int)(h1 >> shift) & 0x01f;
            var i2 = (int)(h2 >> shift) & 0x01f;

            if (i1 != i2) return node.CreateNewNodeFrom(i, key, value, i1, i2, _versionID);

            // Creating longer path
            var s = new Stack<int>();

            do
            {
                s.Push(i1);

                shift += 5;
                i1 = (int)(h1 >> shift) & 0x01f;
                i2 = (int)(h2 >> shift) & 0x01f;
            }
            while (i1 == i2);

            var newNode = node.CreateNewNodeFrom(i, key, value, i1, i2, _versionID);

            // Creating path
            foreach (var idx in s)
            {
                newNode = node.CreateReferenceNode(idx, newNode, _versionID);
            }

            return newNode;
        }

        internal FactMapNode<V> Adding(int shift, FactMapNode<V> node, uint hash, FactName key, V value, ref FactNodeChangeState set)
        {
            var idx = (int)((hash >> shift) & 0x01f);
            var state = node.GetNodeStateAt(idx);

            switch (state)
            {
                case FactNodeType.Reference:
                {
                    // On position is reference node
                    var referencedNode = node.GetReferenceAt(idx);
                    var newNode = Adding(shift + 5, referencedNode, hash, key, value, ref set);

                    return (newNode == referencedNode || set.HasFlag(FactNodeChangeState.ChangedItem))
                        ? node.ChangeReference(idx, newNode, _versionID): node;
                }
                case FactNodeType.Nil: return node.AddValueItemAt(idx, key, value, _versionID);
            }

            // On position is value node or collision collection
            var relation = node.RelationWithNodeAt(key, idx, state);

            if (relation == FactNodeHashRelation.Equal)
            {
                // Value with the same key already exists
                var n = node.GetValueAt(idx, state, key);

                set &= ~FactNodeChangeState.NewItem;

                if (n.Equals(value))
                {
                    set &= ~FactNodeChangeState.ChangedItem;
                    return node;
                }

                return node.ChangeValue(idx, state, key, value, _versionID);
            }
            if (relation == FactNodeHashRelation.Collide)
            {
                // Hash collision
                return (state == FactNodeType.Value)
                    ? node.CreateCollisionAt(idx, key, value, _versionID)
                    : node.AddToCollisionsAt(idx, key, value, _versionID);
            }

            // Hashes are different, we create longer path
            return node.CreateReference(idx,
                CreateCommonPath((uint)node.GetHashCodeAt(idx, state), hash, idx, shift + 5, node, key, value),
                state,_versionID);
            
        }

        internal FactMapNode<V> Removing(int shift, FactMapNode<V> node, uint hash, FactName key)
        {
            var idx = (int)((hash >> shift) & 0x01f);
            var state = node.GetNodeStateAt(idx);

            switch (state)
            {
                case FactNodeType.Nil:
                    throw new KeyNotFoundException("The persistent dictionary doesn't contain value associated with specified key");
                case FactNodeType.Value:
                case FactNodeType.Collision:
                {
                    var relation = node.RelationWithNodeAt(key, idx, state);
                    if (relation == FactNodeHashRelation.Equal)
                        return node.RemoveValue(idx, state, key, _versionID);

                    throw new KeyNotFoundException("The persistent dictionary doesn't contain value associated with specified key");
                }
            }

            // On position is reference node
            var newNode = Removing(shift + 5, node.GetReferenceAt(idx), hash, key);

            if (newNode.CalculateValueCount() == 1 && newNode.ReferenceCount == 0)
            {
                // In recursion, we carry node to remove
                return (node.ReferenceCount > 1 || node.CalculateValueCount() != 0)
                    ? node.Merge(newNode, idx, _versionID)
                    : newNode;
            }

            return node.ChangeReference(idx, newNode, _versionID);
        }

        protected V GetValue(FactName key)
        {
            var hash = key.GetHashCode();
            var node = _rootFactNode;

            for (int shift = 0; shift < 32; shift += 5)
            {
                var idx = (hash >> shift) & 0x01f;
                var state = node.GetNodeStateAt(idx);

                if (state == FactNodeType.Reference) node = node.GetReferenceAt(idx);
                else if (state == FactNodeType.Value || state == FactNodeType.Collision)
                {
                    return node.GetValueAt(idx, state, key);
                }
                else 
                    throw new KeyNotFoundException("The persistent dictionary doesn't contain value associated with specified key");

            }

            throw new Exception();
        }

        public bool Contains(V item) => this.Any(elem => elem.Value.Equals(item));

        public bool ContainsKey(FactName key)
        {
            if (_rootFactNode == null) return false;
            
            var hash = key.GetHashCode();
            var node = _rootFactNode;

            for (int shift = 0; shift < 32; shift += 5)
            {
                var idx = (hash >> shift) & 0x01f;
                var state = node.GetNodeStateAt(idx);

                switch (state)
                {
                    case FactNodeType.Value:
                    case FactNodeType.Collision:
                        return node.IsKeyAt(idx, state, key);
                    case FactNodeType.Nil:
                        return false;
                    case FactNodeType.Reference:
                        break;
                    default:
                        node = node.GetReferenceAt(idx);
                        break;
                }
            }

            throw new Exception();
        }
        
        // IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // IEnumerable
        public IEnumerator<KeyValuePair<FactName, V>> GetEnumerator() =>
            _rootFactNode == null ?
                Enumerable.Empty<KeyValuePair<FactName, V>>().GetEnumerator() :
                _rootFactNode.GetEnumerator();
    }
}
