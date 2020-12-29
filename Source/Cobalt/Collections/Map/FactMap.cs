using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cobalt.Collections.Map.Node;

namespace Cobalt.Collections.Map
{
    [Serializable]
    public abstract class FactMap : IEnumerable<KeyValuePair<string, object>>
    {
        protected FactMapNode RootNode { get; set; }
        protected int NodeCount { get; set; }

        [NonSerialized] internal FactMapVersionRef FactMapVersionRef;

        public bool IsEmpty => NodeCount == 0;


        internal FactMapNode CreateValueNode(int idx, string key, object value,
            FactMapVersionRef factMapVersionRef = null)
        {
            return new FactMapNode(idx, key, value, factMapVersionRef);
        }

        internal FactMap(FactMapNode rootNode, int nodeCount, FactMapVersionRef factMapVersionRef = null)
        {
            RootNode = rootNode;
            NodeCount = nodeCount;
            FactMapVersionRef = factMapVersionRef;
        }

        internal FactMapNode CreateCommonPath(uint h1, uint h2, int i, int shift, FactMapNode node,
            string key, object value)
        {
            var i1 = (int) (h1 >> shift) & 0x01f;
            var i2 = (int) (h2 >> shift) & 0x01f;
            if (i1 != i2)
                return node.CreateNewNodeFrom(i, key, value, i1, i2, FactMapVersionRef);

            // Creating longer path
            var s = new Stack<int>();

            do
            {
                s.Push(i1);
                shift += 5;
                i1 = (int) (h1 >> shift) & 0x01f;
                i2 = (int) (h2 >> shift) & 0x01f;
            } while (i1 == i2);

            var newNode = node.CreateNewNodeFrom(i, key, value, i1, i2, FactMapVersionRef);

            // Creating path
            foreach (var idx in s)
                newNode = node.CreateReferenceNode(idx, newNode, FactMapVersionRef);

            return newNode;
        }

        internal FactMapNode Adding(int shift, FactMapNode node, uint hash, string key, object value,
            ref FactNodeChangeState set)
        {
            var idx = (int) ((hash >> shift) & 0x01f);
            var state = node.GetNodeStateAt(idx);

            if (state == FactNodeType.Reference)
            {
                // On position is reference node
                var referencedNode = node.GetReferenceAt(idx);
                var newNode = Adding(shift + 5, referencedNode, hash, key, value, ref set);

                return (newNode == referencedNode || set.HasFlag(FactNodeChangeState.ChangedItem))
                    ? node.ChangeReference(idx, newNode, FactMapVersionRef)
                    : node;
            }

            if (state == FactNodeType.Nil)
                return node.AddValueItemAt(idx, key, value, FactMapVersionRef);

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

                return node.ChangeValue(idx, state, key, value, FactMapVersionRef);
            }

            if (relation == FactNodeHashRelation.Collide)
            {
                // Hash collision
                return (state == FactNodeType.Value)
                    ? node.CreateCollisionAt(idx, key, value, FactMapVersionRef)
                    : node.AddToColisionAt(idx, key, value, FactMapVersionRef);
            }

            // Hashes are different, we create longer path
            return node.CreateReference(
                idx, CreateCommonPath((uint) node.GetHashCodeAt(idx, state), hash, idx, shift + 5, node, key, value),
                state, FactMapVersionRef
            );
        }

        internal FactMapNode Removing(int shift, FactMapNode node, uint hash, string key)
        {
            var idx = (int) ((hash >> shift) & 0x01f);
            var state = node.GetNodeStateAt(idx);

            if (state == FactNodeType.Nil)
                throw new KeyNotFoundException(
                    "The persistent dictionary doesn't contain value associated with specified key");
            if (state == FactNodeType.Value || state == FactNodeType.Collision)
            {
                var relation = node.RelationWithNodeAt(key, idx, state);
                if (relation == FactNodeHashRelation.Equal)
                    return node.RemoveValue(idx, state, key, FactMapVersionRef);
                else
                    throw new KeyNotFoundException(
                        "The persistent dictionary doesn't contain value associated with specified key");
            }

            // On position is reference node
            var newNode = Removing(shift + 5, node.GetReferenceAt(idx), hash, key);

            if (newNode.ValueCount == 1 && newNode.ReferenceCount == 0)
                // In recursion, we carry node to remove
                return (node.ReferenceCount > 1 || node.ValueCount != 0)
                    ? node.Merge(newNode, idx, FactMapVersionRef)
                    : newNode;
            else
                return node.ChangeReference(idx, newNode, FactMapVersionRef);
        }

        public object GetValue(string name)
        {
            var hash = name.GetHashCode();
            var node = RootNode;

            for (var shift = 0; shift < 32; shift += 5)
            {
                var idx = (hash >> shift) & 0x01f;
                var state = node.GetNodeStateAt(idx);

                if (state == FactNodeType.Reference) node = node.GetReferenceAt(idx);
                else if (state == FactNodeType.Value || state == FactNodeType.Collision)
                    return node.GetValueAt(idx, state, name);
                else
                    throw new KeyNotFoundException(
                        "The persistent dictionary doesn't contain value associated with specified key");
            }

            throw new Exception();
        }

        public bool Contains(object item)
        {
            foreach (var elem in this)
                if (elem.Value.Equals(item))
                    return true;

            return false;
        }

        public bool ContainsKey(string key)
        {
            if (RootNode == null) return false;

            var hash = key.GetHashCode();
            var node = RootNode;

            for (var shift = 0; shift < 32; shift += 5)
            {
                var idx = (hash >> shift) & 0x01f;
                var state = node.GetNodeStateAt(idx);

                if (state == FactNodeType.Value || state == FactNodeType.Collision)
                {
                    return node.IsKeyAt(idx, state, key);
                }

                if (state == FactNodeType.Nil) return false;

                node = node.GetReferenceAt(idx);
            }

            throw new Exception();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            if (RootNode == null)
                return Enumerable
                    .Empty<KeyValuePair<string, object>>()
                    .GetEnumerator();

            return RootNode.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}