using System;
using System.Collections.Generic;
using System.Linq;

namespace Cobalt.Unit.Fact.Map.Hash
{
    /// <summary>
    ///     Simple implementation of collision collection using persistent array
    /// </summary>
    [Serializable]
    internal class FactHashCollisionArray : IEnumerable<KeyValuePair<string, object>>
    {
        private KeyValuePair<string, object>[] _collisions;
        [NonSerialized]
        private FactMapVersionRef _factMapVersionRef;

        public int HashCode { get; }
        public int Count => _collisions.Length;
        

        private FactHashCollisionArray(KeyValuePair<string, object>[] collisions, int hashCode, FactMapVersionRef factMapVersionRef)
        {
            _collisions = collisions;
            HashCode = hashCode;
            _factMapVersionRef = factMapVersionRef;
        }

        public FactHashCollisionArray(FactMapVersionRef factMapVersionRef, params KeyValuePair<string, object>[] pairs)
        {
            if (pairs.Length < 2) throw new NotSupportedException("Collision collection should contain at least 2 items");

            HashCode = pairs.First().Key.GetHashCode();
            _collisions = pairs.ToArray();
            _factMapVersionRef = factMapVersionRef;
        }

        public FactHashCollisionArray Add(string key, object item, FactMapVersionRef factMapVersionRef)
        {
            return new FactHashCollisionArray(
                _collisions.Concat(Helpers.Yield(new KeyValuePair<string, object>(key, item))).ToArray(),
                HashCode, factMapVersionRef);
        }

        public FactHashCollisionArray Remove(string key, FactMapVersionRef factMapVersionRef)
        {
            if (Count < 3) throw new NotSupportedException("Collision collection should contain at least 2 items");
            return new FactHashCollisionArray(_collisions.Where(x => !key.Equals(x.Key)).ToArray(), HashCode, factMapVersionRef);
        }

        public FactHashCollisionArray Change(string key, object value, FactMapVersionRef factMapVersionRef)
        {
            if (factMapVersionRef != null && factMapVersionRef == _factMapVersionRef)
            {
                for (var i = 0; i < _collisions.Length; i++)
                {
                    if (_collisions[i].Key.Equals(key))
                    {
                        _collisions[i] = new KeyValuePair<string, object>(key, value);
                        return this;
                    }
                }

                throw new Exception("Key not found");
            }

            return new FactHashCollisionArray(
                _collisions
                    .Where(x => !key.Equals(x.Key))
                    .Concat(Helpers.Yield(new KeyValuePair<string, object>(key, value)))
                    .ToArray(),
                HashCode, factMapVersionRef);
        }

        public KeyValuePair<string, object> GetItem(string key)
        {
            var pair = _collisions.FirstOrDefault(x => key.Equals(x.Key));

            if (!key.Equals(pair.Key)) 
                throw new KeyNotFoundException("The persistent dictionary doesn't contain value associated with specified key");

            return pair;
        }

        public bool HasItemWithKey(string key) => _collisions.Any(x => key.Equals(x.Key));

        public bool ContentEqual(FactHashCollisionArray c2)
        {
            if (c2.Count != Count) return false;

            var set1 = new HashSet<KeyValuePair<string, object>>(this);
            var set2 = new HashSet<KeyValuePair<string, object>>(c2);
            
            return set1.SetEquals(set2);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _collisions.AsEnumerable().GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        
        public KeyValuePair<string, object> GetRemainingValue(string removedKey) => _collisions.First(x => !removedKey.Equals(x.Key));
    }
}
