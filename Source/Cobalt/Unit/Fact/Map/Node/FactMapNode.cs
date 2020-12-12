using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cobalt.Unit.Fact.Map.Hash;

namespace Cobalt.Unit.Fact.Map.Node
{
    [Serializable]
    internal class FactMapNode<TValue> : IEnumerable<KeyValuePair<FactName, TValue>>, IEquatable<FactMapNode<TValue>>
    {
        private KeyValuePair<FactName, TValue>[] _values;
        private CollisionArray<TValue>[] _collisions;
        private FactMapNode<TValue>[] _references;
        private uint _valueBitmap;
        private uint _referenceBitmap;
        private uint _collisionBitmap;

        [NonSerialized] private int _hash;
        [NonSerialized] private VersionID _versionId;

        private FactMapNode()
        {
        }

        public FactMapNode(int idx, FactName key, TValue value, VersionID versionID)
        {
            _values = new[] {new KeyValuePair<FactName, TValue>(key, value)};
            _valueBitmap = (uint) (1 << idx);
            _versionId = versionID;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int ReferenceCount => _references?.Length ?? 0;

        private int ValuePosition(int i) => (i == 31) ? 0 : (_valueBitmap >> i + 1).BitCount();
        private int CollisionPosition(int i) => (i == 31) ? 0 : (_collisionBitmap >> i + 1).BitCount();
        private int ReferencePosition(int i) => (i == 31) ? 0 : (_referenceBitmap >> i + 1).BitCount();
        public FactMapNode<TValue> GetReferenceAt(int i) => _references[ReferencePosition(i)];
        public override bool Equals(object obj) => obj is FactMapNode<TValue> mapNode && Equals(mapNode);
        private CollisionArray<TValue> CreateCollisionCollection(
            VersionID versionID, params KeyValuePair<FactName,
                TValue>[] pairs) => new CollisionArray<TValue>(versionID, pairs);

        public FactNodeType GetNodeStateAt(int index)
        {
            var pos = (1 << index);
            if ((_referenceBitmap & pos) != 0) return FactNodeType.Reference;
            if ((_valueBitmap & pos) != 0) return FactNodeType.Value;
            return (_collisionBitmap & pos) != 0 ? FactNodeType.Collision : FactNodeType.Nil;
        }


        public TValue GetValueAt(int i, FactNodeType type, FactName key)
        {
            var pair = (type == FactNodeType.Value)
                ? _values[ValuePosition(i)]
                : _collisions[CollisionPosition(i)].GetItem(key);

            if (!key.Equals(pair.Key))
                throw new KeyNotFoundException(
                    "The persistent dictionary doesn't contain value associated with specified key");

            return pair.Value;
        }

        public bool IsKeyAt(int index, FactNodeType type, FactName factName)
        {
            return (type == FactNodeType.Value)
                ? _values[ValuePosition(index)].Key.Equals(factName)
                : _collisions[CollisionPosition(index)].HasItemWithKey(factName);
        }


        private KeyValuePair<FactName, TValue>[] AddToValues(int vIndex, FactName key, TValue value)
        {
            if (_values == null) return new[] {new KeyValuePair<FactName, TValue>(key, value)};

            var newValues = new KeyValuePair<FactName, TValue>[_values.Length + 1];

            Array.Copy(_values, newValues, vIndex);
            newValues[vIndex] = new KeyValuePair<FactName, TValue>(key, value);
            Array.Copy(_values, vIndex, newValues, vIndex + 1, _values.Length - vIndex);

            return newValues;
        }

        private CollisionArray<TValue>[] AddToCollisions(int cIndex, CollisionArray<TValue> collision)
        {
            if (_collisions == null) return new[] {collision};

            var newCollisions = new CollisionArray<TValue>[_collisions.Length + 1];

            Array.Copy(_collisions, newCollisions, cIndex);
            newCollisions[cIndex] = collision;
            Array.Copy(_collisions, cIndex, newCollisions, cIndex + 1, _collisions.Length - cIndex);

            return newCollisions;
        }

        public FactMapNode<TValue> AddValueItemAt(int i, FactName key, TValue value, VersionID versionID = null)
        {
            var newCollisions = _collisions;
            var newReferences = _references;

            if (versionID != null && versionID != _versionId)
            {
                newCollisions = _collisions.Copy();
                newReferences = _references.Copy();
            }

            var newValues = AddToValues(ValuePosition(i), key, value);

            return new FactMapNode<TValue>()
            {
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _valueBitmap = _valueBitmap | (1u << i),
                _collisionBitmap = _collisionBitmap,
                _referenceBitmap = _referenceBitmap,
                _versionId = versionID
            };
        }


        public FactNodeHashRelation RelationWithNodeAt(FactName key, int idx, FactNodeType type)
        {
            if (type == FactNodeType.Value)
            {
                var pair = _values[ValuePosition(idx)];
                if (pair.Key.GetHashCode() == key.GetHashCode())
                    return (pair.Key.Equals(key)) ? FactNodeHashRelation.Equal : FactNodeHashRelation.Collide;

                return FactNodeHashRelation.Differ;
            }

            var cCollection = _collisions[CollisionPosition(idx)];
            if (cCollection.HashCode == key.GetHashCode())
                return (cCollection.HasItemWithKey(key))
                    ? FactNodeHashRelation.Equal
                    : FactNodeHashRelation.Collide;

            return FactNodeHashRelation.Differ;
        }

        public FactMapNode<TValue> AddToCollisionsAt(int i, FactName key, TValue value, VersionID versionID)
        {
            var cIndex = CollisionPosition(i);

            var newCollisions = _collisions;
            var newValues = _values;
            var newReferences = _references;

            if (versionID == null || _versionId != versionID) newCollisions = _collisions.Copy();

            newCollisions[cIndex] = newCollisions[cIndex].Add(key, value, versionID);

            if (versionID != null && versionID != _versionId)
            {
                newReferences = _references.Copy();
                newValues = _values.Copy();
            }

            return new FactMapNode<TValue>()
            {
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _valueBitmap = _valueBitmap,
                _collisionBitmap = _collisionBitmap,
                _referenceBitmap = _referenceBitmap,
                _versionId = versionID
            };
        }

        public FactMapNode<TValue> CreateCollisionAt(int i, FactName key, TValue value, VersionID versionID)
        {
            var pos = (1u << i);
            var cIndex = CollisionPosition(i);
            var vIndex = ValuePosition(i);
            var newValues = RemoveFromValues(vIndex);
            CollisionArray<TValue>[] newCollisions;
            if (_collisions != null)
            {
                newCollisions = new CollisionArray<TValue>[_collisions.Length + 1];
                Array.Copy(_collisions, newCollisions, cIndex);
                Array.Copy(_collisions, cIndex, newCollisions, cIndex + 1, _collisions.Length - cIndex);
            }
            else
                newCollisions = new CollisionArray<TValue>[1];

            newCollisions[cIndex] = CreateCollisionCollection(versionID, _values[vIndex],
                new KeyValuePair<FactName, TValue>(key, value)
            );

            var newReferences = (versionID != null && versionID != _versionId) ? _references.Copy() : _references;

            return new FactMapNode<TValue>()
            {
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _valueBitmap = _valueBitmap & ~pos,
                _collisionBitmap = _collisionBitmap | pos,
                _referenceBitmap = _referenceBitmap,
                _versionId = versionID
            };
        }

        private KeyValuePair<FactName, TValue>[] RemoveFromValues(int vIndex)
        {
            if (_values.Length == 1) return null;

            var newValues = new KeyValuePair<FactName, TValue>[_values.Length - 1];

            Array.Copy(_values, newValues, vIndex);
            Array.Copy(_values, vIndex + 1, newValues, vIndex, _values.Length - vIndex - 1);

            return newValues;
        }

        private CollisionArray<TValue>[] RemoveFromCollisions(int cIndex)
        {
            if (_collisions.Length == 1) return null;

            var newCollisions = new CollisionArray<TValue>[_collisions.Length - 1];

            Array.Copy(_collisions, newCollisions, cIndex);
            Array.Copy(_collisions, cIndex + 1, newCollisions, cIndex, _collisions.Length - cIndex - 1);

            return newCollisions;
        }

        private FactMapNode<TValue>[] RemoveFromReferences(int rIndex)
        {
            if (_references.Length == 1) return null;

            var newReferences = new FactMapNode<TValue>[_references.Length - 1];

            Array.Copy(_references, newReferences, rIndex);
            Array.Copy(_references, rIndex + 1, newReferences, rIndex, _references.Length - rIndex - 1);

            return newReferences;
        }

        public FactMapNode<TValue> CreateReference(int i, FactMapNode<TValue> mapNode, FactNodeType type,
            VersionID versionID)
        {
            var pos = 1u << i;
            var rIndex = ReferencePosition(i);
            FactMapNode<TValue>[] newReferences;

            if (_references == null)
                newReferences = new[] {mapNode};
            else
            {
                newReferences = new FactMapNode<TValue>[_references.Length + 1];
                Array.Copy(_references, newReferences, rIndex);
                newReferences[rIndex] = mapNode;
                Array.Copy(_references, rIndex, newReferences, rIndex + 1, _references.Length - rIndex);
            }
            var newCBitmap = _collisionBitmap;
            var newVBitmap = _valueBitmap;
            var newValues = _values;
            var newCollisions = _collisions;

            if (type == FactNodeType.Value)
            {
                newVBitmap &= ~pos;
                newValues = RemoveFromValues(ValuePosition(i));
            }
            else
            {
                newCBitmap &= ~pos;
                newCollisions = RemoveFromCollisions(CollisionPosition(i));
            }

            if (versionID != null)
            {
                if (_versionId == versionID)
                {
                    _values = newValues;
                    _collisions = newCollisions;
                    _references = newReferences;
                    _collisionBitmap = newCBitmap;
                    _valueBitmap = newVBitmap;
                    _referenceBitmap |= pos;

                    return this;
                }

                if (newCollisions == _collisions) newCollisions = _collisions.Copy();
                if (newValues == _values) newValues = _values.Copy();
            }

            return new FactMapNode<TValue>()
            {
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _collisionBitmap = newCBitmap,
                _valueBitmap = newVBitmap,
                _referenceBitmap = _referenceBitmap | pos,
                _versionId = versionID
            };
        }

        public FactMapNode<TValue> ChangeReference(int i, FactMapNode<TValue> mapNode, VersionID versionID)
        {
            var newValues = _values;
            var newCollisions = _collisions;
            var rIndex = ReferencePosition(i);

            if (versionID != null)
            {
                if (_versionId == versionID)
                {
                    _references[rIndex] = mapNode;
                    return this;
                }

                newValues = _values.Copy();
                newCollisions = _collisions.Copy();
            }

            var newReferences = _references.Copy();
            newReferences[rIndex] = mapNode;

            return new FactMapNode<TValue>()
            {
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _valueBitmap = _valueBitmap,
                _collisionBitmap = _collisionBitmap,
                _referenceBitmap = _referenceBitmap,
                _versionId = versionID
            };
        }

        public int CalculateValueCount()
        {
            var count = 0;
            if (_values != null) count += _values.Length;
            if (_collisions != null) count += _collisions.Length;
            return count;
        }

        private CollisionArray<TValue>[] ChangeCollisions(int cIndex, FactName key, TValue value,VersionID versionID)
        {
            if (versionID != null && versionID == _versionId)
            {
                _collisions[cIndex] = _collisions[cIndex].Change(key, value, versionID);
                return _collisions;
            }

            var newCollisions = _collisions.Copy();
            var collision = newCollisions[cIndex];

            newCollisions[cIndex] = collision.Change(key, value, versionID);

            return newCollisions;
        }

        private KeyValuePair<FactName, TValue>[] ChangeValues(int vIndex, FactName key, TValue value,
            VersionID versionID)
        {
            var newPair = new KeyValuePair<FactName, TValue>(key, value);

            if (versionID != null && versionID == _versionId)
            {
                _values[vIndex] = newPair;
                return _values;
            }

            var newValues = _values.Copy();
            newValues[vIndex] = newPair;

            return newValues;
        }

        public FactMapNode<TValue> ChangeValue(int idx, FactNodeType type, FactName key, TValue value,
            VersionID versionID)
        {
            var newValues = _values;
            var newCollisions = _collisions;
            var newReferences = _references;

            switch (type)
            {
                case FactNodeType.Value:
                    newValues = ChangeValues(ValuePosition(idx), key, value, versionID);
                    break;
                case FactNodeType.Collision:
                    newCollisions = ChangeCollisions(CollisionPosition(idx), key, value, versionID);
                    break;
            }

            if (versionID != null && _versionId != versionID)
            {
                newReferences = _references.Copy();
                if (_values == newValues) newValues = _values.Copy();
                if (_collisions == newCollisions) newCollisions = _collisions.Copy();
            }

            return new FactMapNode<TValue>()
            {
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _valueBitmap = _valueBitmap,
                _collisionBitmap = _collisionBitmap,
                _referenceBitmap = _referenceBitmap,
                _versionId = versionID
            };
        }

        public FactMapNode<TValue> CreateNewNodeFrom(int oldIdx, FactName key, TValue value, int idx1, int idx2,
            VersionID versionID)
        {
            var otherValue = new KeyValuePair<FactName, TValue>(key, value);

            if ((_valueBitmap & (1u << oldIdx)) != 0)
            {
                // On index oldIdx is value
                var vIndex = ValuePosition(oldIdx);
                var thisValue = _values[vIndex];

                return new FactMapNode<TValue>()
                {
                    _valueBitmap = (1u << idx1) | (1u << idx2),
                    _values = (idx1 < idx2)
                        ? new[] {otherValue, thisValue}
                        : new[] {thisValue, otherValue},
                    _versionId = versionID
                };
            }
            else
            {
                // On index oldIdx is collision collection
                var cIndex = CollisionPosition(oldIdx);
                var thisValue = _collisions[cIndex];

                return new FactMapNode<TValue>()
                {
                    _collisionBitmap = (1u << idx1),
                    _valueBitmap = (1u << idx2),
                    _values = new[] {otherValue},
                    _collisions = new[] {thisValue},
                    _versionId = versionID
                };
            }
        }

        public int GetHashCodeAt(int idx, FactNodeType type)
        {
            return (type == FactNodeType.Value)
                ? _values[ValuePosition(idx)].Key.GetHashCode()
                : _collisions[CollisionPosition(idx)].HashCode;
        }

        public FactMapNode<TValue> CreateReferenceNode(int idx, FactMapNode<TValue> node, VersionID versionID)
        {
            return new FactMapNode<TValue>()
            {
                _referenceBitmap = 1u << idx,
                _references = new[] {node},
                _versionId = versionID
            };
        }

        public FactMapNode<TValue> RemoveValue(int idx, FactNodeType type, FactName key, VersionID versionID)
        {
            var pos = 1u << idx;

            var newCollisions = _collisions;
            var newValues = _values;
            var newReferences = _references;
            var newCBitmap = _collisionBitmap;
            var newVBitmap = _valueBitmap;

            if (type == FactNodeType.Collision)
            {
                var cIndex = CollisionPosition(idx);

                var collision = _collisions[cIndex];
                if (collision.Count == 2)
                {
                    var vIndex = ValuePosition(idx);

                    newCollisions = RemoveFromCollisions(cIndex);
                    var pair = collision.GetRemainingValue(key);

                    newValues = AddToValues(vIndex, pair.Key, pair.Value);
                    newCBitmap &= ~pos;
                    newVBitmap |= pos;
                }
                else
                {
                    if (versionID == null || versionID != _versionId) newCollisions = _collisions.Copy();
                    newCollisions[cIndex] = collision.Remove(key, versionID);
                }
            }
            else // if (state == NodeState.Value)
            {
                newValues = RemoveFromValues(ValuePosition(idx));
                newVBitmap &= ~pos;
            }

            if (versionID != null)
            {
                if (_versionId == versionID)
                {
                    _valueBitmap = newVBitmap;
                    _collisionBitmap = newCBitmap;
                    _values = newValues;
                    _collisions = newCollisions;

                    return this;
                }

                if (newValues == _values) newValues = _values.Copy();
                if (newCollisions == _collisions) newCollisions = _collisions.Copy();
                newReferences = _references.Copy();
            }

            return new FactMapNode<TValue>()
            {
                _valueBitmap = newVBitmap,
                _collisionBitmap = newCBitmap,
                _referenceBitmap = _referenceBitmap,
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _versionId = versionID
            };
        }

        public FactMapNode<TValue> Merge(FactMapNode<TValue> newNode, int index, VersionID versionID)
        {
            var pos = 1u << index;

            var mapNode = newNode;
            if (mapNode == null) throw new NotSupportedException();

            var newCollisions = _collisions;
            var newValues = _values;
            var newReferences = RemoveFromReferences(ReferencePosition(index));

            var newCBitmap = _collisionBitmap;
            var newVBitmap = _valueBitmap;
            var newRBitmap = _referenceBitmap & ~pos;

            if (mapNode._values != null)
            {
                var valuePair = mapNode._values[0];
                newValues = AddToValues(ValuePosition(index), valuePair.Key, valuePair.Value);
                newVBitmap |= pos;
            }
            else // if (mapNode.collisions != null)
            {
                var collision = mapNode._collisions[0];
                newCollisions = AddToCollisions(CollisionPosition(index), collision);
                newCBitmap |= pos;
            }

            if (versionID != null)
            {
                if (_versionId == versionID)
                {
                    _valueBitmap = newVBitmap;
                    _collisionBitmap = newCBitmap;
                    _referenceBitmap = newRBitmap;

                    _values = newValues;
                    _collisions = newCollisions;
                    _references = newReferences;
                    return this;
                }

                if (newValues == _values) newValues = _values.Copy();
                if (newCollisions == _collisions) newCollisions = _collisions.Copy();
            }

            return new FactMapNode<TValue>()
            {
                _referenceBitmap = newRBitmap,
                _valueBitmap = newVBitmap,
                _collisionBitmap = newCBitmap,
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _versionId = versionID
            };
        }

        public FactMapNode<TValue> MakeRoot(VersionID versionID = null)
        {
            var newCBitmap = _collisionBitmap;
            var newVBitmap = _valueBitmap;

            if (_values != null)
            {
                var idx = _values[0].Key.GetHashCode() & 0x01f;
                newVBitmap = 1u << idx;
            }
            else
            {
                var idx = _collisions[0].HashCode & 0x01f;
                newCBitmap = 1u << idx;
            }

            var newValues = _values;
            var newCollisions = _collisions;

            if (versionID != null)
            {
                if (_versionId == versionID)
                {
                    _valueBitmap = newVBitmap;
                    _collisionBitmap = newCBitmap;
                    return this;
                }

                newValues = _values.Copy();
                newCollisions = _collisions.Copy();
            }

            return new FactMapNode<TValue>()
            {
                _valueBitmap = newVBitmap,
                _collisionBitmap = newCBitmap,
                _values = newValues,
                _collisions = newCollisions,
                _versionId = versionID
            };
        }


        public IEnumerator<KeyValuePair<FactName, TValue>> GetEnumerator()
        {
            var res = Enumerable.Empty<KeyValuePair<FactName, TValue>>();
            if (_values != null) res = res.Concat(_values);
            if (_collisions != null) res = res.Concat(_collisions.SelectMany(x => x));
            if (_references != null) res = res.Concat(_references.SelectMany(x => x));
            return res.GetEnumerator();
        }


        public override int GetHashCode()
        {
            if (_hash != 0) return _hash;
            if (_values != null)
                foreach (var value in _values)
                    _hash ^= value.Key.GetHashCode() ^ value.Value.GetHashCode();
            if (_collisions != null)
                foreach (var value in _collisions.SelectMany(x => x))
                    _hash ^= value.Key.GetHashCode() ^ value.Value.GetHashCode();
            if (_references != null)
                foreach (var node in _references)
                    _hash ^= node.GetHashCode();
            if (_hash == 0) _hash = 1;
            return _hash;
        }


        public bool Equals(FactMapNode<TValue> other)
        {
            // Compare values
            if ((_values != null) == (other._values != null))
            {
                if (_values != null && !ReferenceEquals(_values, other._values))
                {
                    if (_values.Length != other._values.Length) return false;
                    for (int i = 0; i < _values.Length; i++)
                        if (!_values[i].Equals(other._values[i]))
                            return false;
                }
            }
            else return false;

            // Compare collision collections
            if ((_collisions != null) == (other._collisions != null))
            {
                if (_collisions != null && !ReferenceEquals(_collisions, other._collisions))
                {
                    if (_collisions.Length != other._collisions.Length) return false;
                    for (int i = 0; i < _collisions.Length; i++)
                    {
                        var c1 = _collisions[i];
                        var c2 = other._collisions[i];
                        if (!ReferenceEquals(c1, c2) && !c1.ContentEqual(c2)) return false;
                    }
                }
            }
            else return false;

            // Compare references
            if ((_references != null) != (other._references != null)) return true;
            {
                if (_references == null) return true;
                if (_references.Length != other._references.Length) return false;
                for (int i = 0; i < _references.Length; i++)
                {
                    var r1 = _references[i];
                    var r2 = other._references[i];
                    if (!ReferenceEquals(r1, r2) && !r1.Equals(r2)) return false;
                }
            }

            return true;
        }
    }
}