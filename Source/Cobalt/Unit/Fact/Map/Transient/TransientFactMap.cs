using System;
using System.Collections.Generic;
using Cobalt.Unit.Fact.Map.Node;
using Cobalt.Unit.Fact.Map.Persistent;

namespace Cobalt.Unit.Fact.Map.Transient
{
    public sealed class TransientFactMap<V> : FactMap<V>, IEnumerable<KeyValuePair<FactName, V>>
    {
        internal TransientFactMap(FactMapNode<V> rootFactNode, int factNodeCount)
            : base(rootFactNode, factNodeCount, new VersionID())
        {

        }

        public PersistentFactMap<V> AsPersistent()
        {
            _versionID = null;
            if (_factNodeCount == 0) return PersistentFactMap<V>.Empty;

            return new PersistentFactMap<V>(_rootFactNode, _factNodeCount);
        }

        #region Main functionality
        public void Add(FactName key, V value)
        {
            if (_versionID == null)
                throw new NotSupportedException("Transient dictionary cannot be modified after call AsPersistent() method");

            var set = FactNodeChangeState.ChangedItem | FactNodeChangeState.NewItem;

            if (_factNodeCount == 0)
            {
                var idx = key.GetHashCode() & 0x01f;

                _rootFactNode = CreateValueNode(idx, key, value, _versionID);
            }
            else
            {
                _rootFactNode = Adding(0, _rootFactNode, (UInt32)key.GetHashCode(), key, value, ref set);
            }

            if (set.HasFlag(FactNodeChangeState.NewItem)) _factNodeCount++;
        }

        public void Add<T>(IEnumerable<T> e, Func<T, FactName> key, Func<T, V> val)
        {
            foreach (var item in e)
            {
                Add(key(item), val(item));
            }
        }

        public void Remove(FactName key)
        {
            if (_versionID == null)
                throw new NotSupportedException("Transient dictionary cannot be modified after call AsPersistent() method");

            if (_factNodeCount == 0) 
                throw new KeyNotFoundException("The persistent dictionary doesn't contain value associated with specified key");

            var newRoot = Removing(0, _rootFactNode, (UInt32)key.GetHashCode(), key);
            _factNodeCount--;

            if (newRoot.CalculateValueCount() == 1) newRoot = newRoot.MakeRoot(_versionID);
            else if (newRoot.CalculateValueCount() == 0 && newRoot.ReferenceCount == 0) newRoot = null;

            _rootFactNode = newRoot;
        }
        #endregion

        public V this[FactName i]
        {
            get => GetValue(i);
            set => Add(i, value);
        }
    }
}
