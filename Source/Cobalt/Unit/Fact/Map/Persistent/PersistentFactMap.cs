using System;
using System.Collections.Generic;
using Cobalt.Unit.Fact.Map.Node;
using Cobalt.Unit.Fact.Map.Transient;

namespace Cobalt.Unit.Fact.Map.Persistent
{
    [Serializable]
    public class PersistentFactMap<TValue> : FactMap<TValue>, IEquatable<PersistentFactMap<TValue>>
    {
        public static PersistentFactMap<TValue> Empty { get; } = new PersistentFactMap<TValue>(null, 0);
        public TransientFactMap<TValue> AsTransient() => new TransientFactMap<TValue>(_rootFactNode, _factNodeCount);
        public override int GetHashCode() => (_rootFactNode != null) ? _rootFactNode.GetHashCode() : 0;
        public TValue this[FactName i] => GetValue(i);
        // [ctor]
        internal PersistentFactMap(FactMapNode<TValue> rootFactNode, int factNodeCount):base(rootFactNode, factNodeCount){}
        
        public PersistentFactMap<TValue> Add(FactName key, TValue value)
        {
            if (_factNodeCount == 0)
            {
                var idx = key.GetHashCode() & 0x01f;
                return new PersistentFactMap<TValue>(CreateValueNode(idx, key, value),1);
            }
            var changeState = FactNodeChangeState.ChangedItem | FactNodeChangeState.NewItem;
            var newRoot = Adding(0, _rootFactNode, (UInt32)key.GetHashCode(), key, value, ref changeState);
            return new PersistentFactMap<TValue>(newRoot,_factNodeCount + ((changeState.HasFlag(FactNodeChangeState.NewItem)) ? 1 : 0));
        }

        public PersistentFactMap<TValue> Add<T>(IEnumerable<T> e, Func<T, FactName> key, Func<T, TValue> val)
        {
            var d = Empty.AsTransient();
            d.Add(e, key, val);
            return d.AsPersistent();
        }

        public PersistentFactMap<TValue> Remove(FactName key)
        {
            if (_factNodeCount == 0) 
                throw new KeyNotFoundException("The persistent dictionary doesn't contain value associated with specified key"); ;

            var newRoot = Removing(0, _rootFactNode, (uint)key.GetHashCode(), key);

            if (_factNodeCount == 1) return Empty;
            if (newRoot.CalculateValueCount() == 1 && newRoot.ReferenceCount == 0) 
                newRoot = newRoot.MakeRoot(_versionID);

            return new PersistentFactMap<TValue>(newRoot,_factNodeCount - 1);
        }
        
        public override bool Equals(object obj)
        {
            var dict = obj as PersistentFactMap<TValue>;
            return dict != null && Equals(dict);
        }

        public bool Equals(PersistentFactMap<TValue> other)
        {
            if (_factNodeCount != other._factNodeCount) return false;
            if ((_rootFactNode != null) == (other._rootFactNode != null))
                return _rootFactNode == null || _rootFactNode.Equals(other._rootFactNode);
            return false;
        }

        public static bool operator ==(PersistentFactMap<TValue> a, PersistentFactMap<TValue> b)
        {
            if ((object)a == null == ((object)b == null))
            {
                if ((object)a != null) return a.Equals(b);
            }
            else return false;
            return true;
        }

        public static bool operator !=(PersistentFactMap<TValue> a, PersistentFactMap<TValue> b) => !(a == b);
    }
    
    
}
