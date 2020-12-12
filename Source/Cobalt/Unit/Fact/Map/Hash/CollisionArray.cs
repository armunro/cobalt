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
    internal class CollisionArray : ICollisionCollection, IEnumerable<KeyValuePair<string, object>>
    {
        private readonly int _hashCode;
        private KeyValuePair<string, object>[] _collisions;
        [NonSerialized]
        private FactMapVersionRef _factMapVersionRef;

        public int HashCode => _hashCode;

        private CollisionArray(KeyValuePair<string, object>[] collisions, int hashCode, FactMapVersionRef factMapVersionRef)
        {
            _collisions = collisions;
            _hashCode = hashCode;
            _factMapVersionRef = factMapVersionRef;
        }

        public CollisionArray(FactMapVersionRef factMapVersionRef, params KeyValuePair<string, object>[] pairs)
        {
            if (pairs.Length < 2) throw new NotSupportedException("Collision collection should contain at least 2 items");

            _hashCode = pairs.First().Key.GetHashCode();
            _collisions = pairs.ToArray();
            _factMapVersionRef = factMapVersionRef;
        }

        public ICollisionCollection Add(string key, object item, FactMapVersionRef factMapVersionRef)
        {
            return new CollisionArray(
                _collisions.Concat(Extensions.Yield(new KeyValuePair<string, object>(key, item))).ToArray(),
                _hashCode,
                factMapVersionRef
                );
        }

        public ICollisionCollection Remove(string key, FactMapVersionRef factMapVersionRef)
        {
            if (Count < 3) throw new NotSupportedException("Collision collection should contain at least 2 items");

            return new CollisionArray(
                _collisions.Where(x => !key.Equals(x.Key)).ToArray(),
                _hashCode,
                factMapVersionRef
                );
        }

        public ICollisionCollection Change(string key, object value, FactMapVersionRef factMapVersionRef)
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

            return new CollisionArray(
                _collisions
                    .Where(x => !key.Equals(x.Key))
                    .Concat(Extensions.Yield(new KeyValuePair<string, object>(key, value)))
                    .ToArray(),
                _hashCode,
                factMapVersionRef
                );
        }

        public KeyValuePair<string, object> GetItem(string key)
        {
            var pair = _collisions.FirstOrDefault(x => key.Equals(x.Key));

            if (!key.Equals(pair.Key)) 
                throw new KeyNotFoundException("The persistent dictionary doesn't contain value associated with specified key");

            return pair;
        }

        public bool HasItemWithKey(string key) => _collisions.Any(x => key.Equals(x.Key));

        public bool ContentEqual(ICollisionCollection c2)
        {
            if (c2.Count != Count) return false;

            var set1 = new HashSet<KeyValuePair<string, object>>(this);
            var set2 = new HashSet<KeyValuePair<string, object>>(c2);
            
            return set1.SetEquals(set2);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _collisions.AsEnumerable().GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => _collisions.Length;
        public KeyValuePair<string, object> GetRemainingValue(string removedKey) => _collisions.First(x => !removedKey.Equals(x.Key));
    }
}
