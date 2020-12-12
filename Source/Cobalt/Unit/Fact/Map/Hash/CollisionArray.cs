using System;
using System.Collections.Generic;
using System.Linq;
using Cobalt.Unit.Fact.Map.Node;

namespace Cobalt.Unit.Fact.Map.Hash
{
    /// <summary>
    ///     Simple implementation of collision collection using persistent array
    /// </summary>
    [Serializable]
    internal class CollisionArray<TValue> : IEnumerable<KeyValuePair<FactName, TValue>>
    {
        private readonly KeyValuePair<FactName, TValue>[] _collisions;
        [NonSerialized]
        private readonly VersionID _versionId;
        public int HashCode { get; }
        public bool HasItemWithKey(FactName key) => _collisions.Any(x => key.Equals(x.Key));
        public IEnumerator<KeyValuePair<FactName, TValue>> GetEnumerator() => _collisions.AsEnumerable().GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => _collisions.Length;
        public KeyValuePair<FactName, TValue> GetRemainingValue(FactName removedKey) => _collisions.First(x => !removedKey.Equals(x.Key));
        
        
        private CollisionArray(KeyValuePair<FactName, TValue>[] collisions, int hashCode, VersionID versionId)
        {
            _collisions = collisions;
            HashCode = hashCode;
            _versionId = versionId;
        }

        public CollisionArray(VersionID versionId, params KeyValuePair<FactName, TValue>[] pairs)
        {
            if (pairs.Length < 2) throw new NotSupportedException("Collision collection should contain at least 2 items");

            HashCode = pairs.First().Key.GetHashCode();
            _collisions = pairs.ToArray();
            _versionId = versionId;
        }

        public CollisionArray<TValue> Add(FactName key, TValue item, VersionID versionId)
        {
            return new CollisionArray<TValue>(
                _collisions.Concat(new KeyValuePair<FactName, TValue>(key, item).Yield()).ToArray(),
                HashCode,versionId);
        }

        public CollisionArray<TValue> Remove(FactName key, VersionID versionId)
        {
            if (Count < 3) throw new NotSupportedException("Collision collection should contain at least 2 items");

            return new CollisionArray<TValue>(
                _collisions.Where(x => !key.Equals(x.Key)).ToArray(),
                HashCode,
                versionId
                );
        }

        public CollisionArray<TValue> Change(FactName key, TValue value, VersionID versionId)
        {
            if (versionId == null || versionId != _versionId)
                return new CollisionArray<TValue>(_collisions
                        .Where(x => !key.Equals(x.Key))
                        .Concat(new KeyValuePair<FactName, TValue>(key, value).Yield()).ToArray(),HashCode,versionId);
            for (var i = 0; i < _collisions.Length; i++)
                if (_collisions[i].Key.Equals(key))
                {
                    _collisions[i] = new KeyValuePair<FactName, TValue>(key, value);
                    return this;
                }

            throw new Exception("Key not found");
        }

        public KeyValuePair<FactName, TValue> GetItem(FactName key)
        {
            var pair = _collisions.FirstOrDefault(x => key.Equals(x.Key));
            if (!key.Equals(pair.Key)) 
                throw new KeyNotFoundException("The persistent dictionary doesn't contain value associated with specified key");
            return pair;
        }



        public bool ContentEqual(CollisionArray<TValue> c2)
        {
            if (c2.Count != Count) return false;
            var set1 = new HashSet<KeyValuePair<FactName, TValue>>(this);
            var set2 = new HashSet<KeyValuePair<FactName, TValue>>(c2);
            return set1.SetEquals(set2);
        }

      
    }
}
