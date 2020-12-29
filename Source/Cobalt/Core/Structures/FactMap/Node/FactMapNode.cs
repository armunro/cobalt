using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cobalt.Collections.Map.Node
{
    [Serializable]
    public class FactMapNode: 
        IEnumerable<KeyValuePair<string, object>>, 
        IEquatable<FactMapNode>,
        IEnumerable
    {
        private KeyValuePair<string, object>[] _values;
        private FactHashCollisionArray[] _collisions;
        private FactMapNode[] _references;

        [NonSerialized]
        private int _hash;

        private uint _vBitmap;
        private uint _rBitmap;
        private uint _cBitmap;

        [NonSerialized]
        private FactMapVersionRef _factMapVersionRef;

      
        private FactMapNode() { }

        public FactMapNode(int idx, string key, object value, FactMapVersionRef factMapVersionRef)
        {
            _values = new[] { new KeyValuePair<string, object>(key, value) };
            _vBitmap = (uint)(1 << idx);
            _factMapVersionRef = factMapVersionRef;
        }

        public int ValueCount
        {
            get
            {
                var count = 0;
                if (_values != null) count += _values.Length;
                if (_collisions != null) count += _collisions.Length;
                return count;
            }
        }

        public int ReferenceCount => (_references != null) ? _references.Length : 0;
        private int ValuePosition(int i) { return (i == 31) ? 0 : BitCount((_vBitmap >> i + 1)); }
        private int CollisionPosition(int i) { return (i == 31) ? 0 : BitCount((_cBitmap >> i + 1)); }
        private int ReferencePosition(int i) { return (i == 31) ? 0 : BitCount((_rBitmap >> i + 1)); }

        public FactNodeType GetNodeStateAt(int i)
        {
            var pos = (1 << i);
            if ((_rBitmap & pos) != 0) return FactNodeType.Reference;
            if ((_vBitmap & pos) != 0) return FactNodeType.Value;
            if ((_cBitmap & pos) != 0) return FactNodeType.Collision;
            return FactNodeType.Nil;
        }

        private FactHashCollisionArray CreateCollisionCollection(FactMapVersionRef factMapVersionRef, params KeyValuePair<string, object>[] pairs)
        {
            return new FactHashCollisionArray(factMapVersionRef, pairs);
        }

        public object GetValueAt(int i, FactNodeType type, string key)
        {
            var pair = (type == FactNodeType.Value)
                ? _values[ValuePosition(i)]
                : _collisions[CollisionPosition(i)].GetItem(key);

            if (!key.Equals(pair.Key))
                throw new KeyNotFoundException("The persistent dictionary doesn't contain value associated with specified key");

            return pair.Value;
        }

        public bool IsKeyAt(int idx, FactNodeType type, string key)
        {
            return (type == FactNodeType.Value)
                ? _values[ValuePosition(idx)].Key.Equals(key)
                : _collisions[CollisionPosition(idx)].HasItemWithKey(key);
        }

        public FactMapNode GetReferenceAt(int i)
        {
            return _references[ReferencePosition(i)];
        }

        private KeyValuePair<string, object>[] AddToValues(int vIndex, string key, object value)
        {
            if (_values == null) return new[] { new KeyValuePair<string, object>(key, value) };

            var newValues = new KeyValuePair<string, object>[_values.Length + 1];

            Array.Copy(_values, newValues, vIndex);
            newValues[vIndex] = new KeyValuePair<string, object>(key, value);
            Array.Copy(_values, vIndex, newValues, vIndex + 1, _values.Length - vIndex);

            return newValues;
        }

        private FactHashCollisionArray[] AddToCollisions(int cIndex, FactHashCollisionArray factHashCollision)
        {
            if (_collisions == null) return new[] { factHashCollision };

            var newCollisions = new FactHashCollisionArray[_collisions.Length + 1];

            Array.Copy(_collisions, newCollisions, cIndex);
            newCollisions[cIndex] = factHashCollision;
            Array.Copy(_collisions, cIndex, newCollisions, cIndex + 1, _collisions.Length - cIndex);

            return newCollisions;
        }

        public FactMapNode AddValueItemAt(int i, string key, object value, FactMapVersionRef factMapVersionRef = null)
        {
            var newCollisions = _collisions;
            var newReferences = _references;

            if (factMapVersionRef != null && factMapVersionRef != _factMapVersionRef)
            {
                newCollisions = Copy(_collisions);
                newReferences = Copy(_references);
            }

            var newValues = AddToValues(ValuePosition(i), key, value);

            return new FactMapNode()
            {
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _vBitmap = _vBitmap | (1u << i),
                _cBitmap = _cBitmap,
                _rBitmap = _rBitmap,
                _factMapVersionRef = factMapVersionRef
            };
        }


        public FactNodeHashRelation RelationWithNodeAt(string key, int idx, FactNodeType type)
        {
            if (type == FactNodeType.Value)
            {
                var pair = _values[ValuePosition(idx)];

                if (pair.Key.GetHashCode() == key.GetHashCode())
                {
                    return (pair.Key.Equals(key))
                        ? FactNodeHashRelation.Equal
                        : FactNodeHashRelation.Collide;
                }

                return FactNodeHashRelation.Different;
            }

            else //if (state == NodeState.Collision)
            {
                var cCollection = _collisions[CollisionPosition(idx)];
                if (cCollection.HashCode == key.GetHashCode())
                {
                    return (cCollection.HasItemWithKey(key))
                        ? FactNodeHashRelation.Equal
                        : FactNodeHashRelation.Collide;
                }

                return FactNodeHashRelation.Different;
            }
        }

        public FactMapNode AddToColisionAt(int i, string key, object value, FactMapVersionRef factMapVersionRef)
        {
            var cIndex = CollisionPosition(i);

            var newCollisions = _collisions;
            var newValues = _values;
            var newReferences = _references;

            if (factMapVersionRef == null || _factMapVersionRef != factMapVersionRef)
            {
                newCollisions = Copy(_collisions);
            }

            newCollisions[cIndex] = newCollisions[cIndex].Add(key, value, factMapVersionRef);

            if (factMapVersionRef != null && factMapVersionRef != _factMapVersionRef)
            {
                newReferences = Copy(_references);
                newValues = Copy(_values);
            }

            return new FactMapNode()
            {
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _vBitmap = _vBitmap,
                _cBitmap = _cBitmap,
                _rBitmap = _rBitmap,
                _factMapVersionRef = factMapVersionRef
            };
        }

        public FactMapNode CreateCollisionAt(int i, string key, object value, FactMapVersionRef factMapVersionRef)
        {
            var pos = (1u << i);

            var cIndex = CollisionPosition(i);
            var vIndex = ValuePosition(i);

            var newValues = RemoveFromValues(vIndex);
            FactHashCollisionArray[] newCollisions;

            if (_collisions != null)
            {
                newCollisions = new FactHashCollisionArray[_collisions.Length + 1];
                Array.Copy(_collisions, newCollisions, cIndex);
                Array.Copy(_collisions, cIndex, newCollisions, cIndex + 1, _collisions.Length - cIndex);
            }
            else
            {
                newCollisions = new FactHashCollisionArray[1]; ;
            }

            newCollisions[cIndex] = CreateCollisionCollection(
                    factMapVersionRef,
                    _values[vIndex],
                    new KeyValuePair<string, object>(key, value)
                );

            var newReferences = (factMapVersionRef != null && factMapVersionRef != _factMapVersionRef)
                ? Copy(_references)
                : _references;

            return new FactMapNode()
            {
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _vBitmap = _vBitmap & ~pos,
                _cBitmap = _cBitmap | pos,
                _rBitmap = _rBitmap,
                _factMapVersionRef = factMapVersionRef
            };
        }

        private KeyValuePair<string, object>[] RemoveFromValues(int vIndex)
        {
            if (_values.Length == 1) return null;

            var newValues = new KeyValuePair<string, object>[_values.Length - 1];

            Array.Copy(_values, newValues, vIndex);
            Array.Copy(_values, vIndex + 1, newValues, vIndex, _values.Length - vIndex - 1);

            return newValues;
        }

        private FactHashCollisionArray[] RemoveFromCollisions(int cIndex)
        {
            if (_collisions.Length == 1) return null;

            var newCollisions = new FactHashCollisionArray[_collisions.Length - 1];

            Array.Copy(_collisions, newCollisions, cIndex);
            Array.Copy(_collisions, cIndex + 1, newCollisions, cIndex, _collisions.Length - cIndex - 1);

            return newCollisions;
        }

        private FactMapNode[] RemoveFromReferences(int rIndex)
        {
            if (_references.Length == 1) return null;

            var newReferences = new FactMapNode[_references.Length - 1];

            Array.Copy(_references, newReferences, rIndex);
            Array.Copy(_references, rIndex + 1, newReferences, rIndex, _references.Length - rIndex - 1);

            return newReferences;
        }

        public FactMapNode CreateReference(int i, FactMapNode mapNode, FactNodeType type, FactMapVersionRef factMapVersionRef)
        {
            var pos = 1u << i;

            var rIndex = ReferencePosition(i);
            FactMapNode[] newReferences;

            if (_references == null)
            {
                newReferences = new[] { mapNode };
            }
            else
            {
                newReferences = new FactMapNode[_references.Length + 1];
                Array.Copy(_references, newReferences, rIndex);
                newReferences[rIndex] = mapNode;
                Array.Copy(_references, rIndex, newReferences, rIndex + 1, _references.Length - rIndex);
            }

            var newCBitmap = _cBitmap;
            var newVBitmap = _vBitmap;

            var newValues = _values;
            var newColisions = _collisions;

            if (type == FactNodeType.Value)
            {
                newVBitmap &= ~pos;
                newValues = RemoveFromValues(ValuePosition(i));
            }
            else
            {
                newCBitmap &= ~pos;
                newColisions = RemoveFromCollisions(CollisionPosition(i));
            }

            if (factMapVersionRef != null)
            {
                if (_factMapVersionRef == factMapVersionRef)
                {
                    _values = newValues;
                    _collisions = newColisions;
                    _references = newReferences;
                    _cBitmap = newCBitmap;
                    _vBitmap = newVBitmap;
                    _rBitmap = _rBitmap | pos;

                    return this;
                }

                if (newColisions == _collisions) newColisions = Copy(_collisions);
                if (newValues == _values) newValues = Copy(_values);
            }

            return new FactMapNode()
            {
                _values = newValues,
                _collisions = newColisions,
                _references = newReferences,
                _cBitmap = newCBitmap,
                _vBitmap = newVBitmap,
                _rBitmap = _rBitmap | pos,
                _factMapVersionRef = factMapVersionRef
            };
        }

        public FactMapNode ChangeReference(int i, FactMapNode mapNode, FactMapVersionRef factMapVersionRef)
        {
            var newValues = _values;
            var newCollisions = _collisions;

            var rIndex = ReferencePosition(i);

            if (factMapVersionRef != null)
            {
                if (_factMapVersionRef == factMapVersionRef)
                {
                    _references[rIndex] = mapNode;
                    return this;
                }

                newValues = Copy(_values);
                newCollisions = Copy(_collisions);
            }

            var newReferences = Copy(_references);
            newReferences[rIndex] = mapNode;

            return new FactMapNode()
            {
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _vBitmap = _vBitmap,
                _cBitmap = _cBitmap,
                _rBitmap = _rBitmap,
                _factMapVersionRef = factMapVersionRef
            };
        }

        private FactHashCollisionArray[] ChangeCollisions(int cIndex, string key, object value, FactMapVersionRef factMapVersionRef)
        {
            if (factMapVersionRef != null && factMapVersionRef == _factMapVersionRef)
            {
                _collisions[cIndex] = _collisions[cIndex].Change(key, value, factMapVersionRef);
                return _collisions;
            }

            var newCollisions = Copy(_collisions);
            var collision = newCollisions[cIndex];

            newCollisions[cIndex] = collision.Change(key, value, factMapVersionRef);

            return newCollisions;
        }

        private KeyValuePair<string, object>[] ChangeValues(int vIndex, string key, object value, FactMapVersionRef factMapVersionRef)
        {
            var newPair = new KeyValuePair<string, object>(key, value);

            if (factMapVersionRef != null && factMapVersionRef == _factMapVersionRef)
            {
                _values[vIndex] = newPair;
                return _values;
            }

            var newValues = Copy(_values);
            newValues[vIndex] = newPair;

            return newValues;
        }

        public FactMapNode ChangeValue(int idx, FactNodeType type, string key, object value, FactMapVersionRef factMapVersionRef)
        {
            var newValues = _values;
            var newCollisions = _collisions;
            var newReferences = _references;

            if (type == FactNodeType.Value)
            {
                newValues = ChangeValues(ValuePosition(idx), key, value, factMapVersionRef);
            }
            if (type == FactNodeType.Collision)
            {
                newCollisions = ChangeCollisions(CollisionPosition(idx), key, value, factMapVersionRef);
            }

            if (factMapVersionRef != null && _factMapVersionRef != factMapVersionRef)
            {
                newReferences = Copy(_references);
                if (_values == newValues) newValues = Copy(_values);
                if (_collisions == newCollisions) newCollisions = Copy(_collisions);
            }

            return new FactMapNode()
            {
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _vBitmap = _vBitmap,
                _cBitmap = _cBitmap,
                _rBitmap = _rBitmap,
                _factMapVersionRef = factMapVersionRef
            };
        }

        public FactMapNode CreateNewNodeFrom(int oldIdx, string key, object value, int idx1, int idx2, FactMapVersionRef factMapVersionRef)
        {
            var otherValue = new KeyValuePair<string, object>(key, value);

            if ((_vBitmap & (1u << oldIdx)) != 0)
            {
                // On index oldIdx is value
                var vIndex = ValuePosition(oldIdx);
                var thisValue = _values[vIndex];

                return new FactMapNode()
                {
                    _vBitmap = (1u << idx1) | (1u << idx2),
                    _values = (idx1 < idx2)
                        ? new[] { otherValue, thisValue }
                        : new[] { thisValue, otherValue },
                    _factMapVersionRef = factMapVersionRef
                };
            }
            else
            {
                // On index oldIdx is collision collection
                var cIndex = CollisionPosition(oldIdx);
                var thisValue = _collisions[cIndex];

                return new FactMapNode()
                {
                    _cBitmap = (1u << idx1),
                    _vBitmap = (1u << idx2),
                    _values = new[] { otherValue },
                    _collisions = new[] { thisValue },
                    _factMapVersionRef = factMapVersionRef
                };
            }
        }

        public int GetHashCodeAt(int idx, FactNodeType type)
        {
            return (type == FactNodeType.Value)
                ? _values[ValuePosition(idx)].Key.GetHashCode()
                : _collisions[CollisionPosition(idx)].HashCode;
        }

        public FactMapNode CreateReferenceNode(int idx, FactMapNode node, FactMapVersionRef factMapVersionRef)
        {
            return new FactMapNode()
            {
                _rBitmap = 1u << idx,
                _references = new[] { node },
                _factMapVersionRef = factMapVersionRef
            };
        }

        public FactMapNode RemoveValue(int idx, FactNodeType type, string key, FactMapVersionRef factMapVersionRef)
        {
            var pos = 1u << idx;

            var newCollisions = _collisions;
            var newValues = _values;
            var newReferences = _references;
            var newCBitmap = _cBitmap;
            var newVBitmap = _vBitmap;

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
                    if (factMapVersionRef == null || factMapVersionRef != _factMapVersionRef)
                    {
                        newCollisions = Copy(_collisions);
                    }
                    newCollisions[cIndex] = collision.Remove(key, factMapVersionRef);
                }
            }
            else // if (state == NodeState.Value)
            {
                newValues = RemoveFromValues(ValuePosition(idx));
                newVBitmap &= ~pos;
            }

            if (factMapVersionRef != null)
            {
                if (_factMapVersionRef == factMapVersionRef)
                {
                    _vBitmap = newVBitmap;
                    _cBitmap = newCBitmap;

                    _values = newValues;
                    _collisions = newCollisions;

                    return this;
                }

                if (newValues == _values) newValues = Copy(_values);
                if (newCollisions == _collisions) newCollisions = Copy(_collisions);
                newReferences = Copy(_references);
            }

            return new FactMapNode()
            {
                _vBitmap = newVBitmap,
                _cBitmap = newCBitmap,
                _rBitmap = _rBitmap,
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _factMapVersionRef = factMapVersionRef
            };
        }

        public FactMapNode Merge(FactMapNode newNode, int index, FactMapVersionRef factMapVersionRef)
        {
            var pos = 1u << index;

            var mapNode = newNode as FactMapNode;
            if (mapNode == null) throw new NotSupportedException();

            var newCollisions = _collisions;
            var newValues = _values;
            var newReferences = RemoveFromReferences(ReferencePosition(index));

            var newCBitmap = _cBitmap;
            var newVBitmap = _vBitmap;
            var newRBitmap = _rBitmap & ~pos;

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

            if (factMapVersionRef != null)
            {
                if (_factMapVersionRef == factMapVersionRef)
                {
                    _vBitmap = newVBitmap;
                    _cBitmap = newCBitmap;
                    _rBitmap = newRBitmap;

                    _values = newValues;
                    _collisions = newCollisions;
                    _references = newReferences;
                    return this;
                }

                if (newValues == _values) newValues = Copy(_values);
                if (newCollisions == _collisions) newCollisions = Copy(_collisions);
            }

            return new FactMapNode()
            {
                _rBitmap = newRBitmap,
                _vBitmap = newVBitmap,
                _cBitmap = newCBitmap,
                _values = newValues,
                _collisions = newCollisions,
                _references = newReferences,
                _factMapVersionRef = factMapVersionRef
            };
        }

        public FactMapNode MakeRoot(FactMapVersionRef factMapVersionRef = null)
        {
            var newCBitmap = _cBitmap;
            var newVBitmap = _vBitmap;

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

            if (factMapVersionRef != null)
            {
                if (_factMapVersionRef == factMapVersionRef)
                {
                    _vBitmap = newVBitmap;
                    _cBitmap = newCBitmap;
                    return this;
                }
                else
                {
                    newValues = Copy(_values);
                    newCollisions = Copy(_collisions);
                }
            }

            return new FactMapNode()
            {
                _vBitmap = newVBitmap,
                _cBitmap = newCBitmap,
                _values = newValues,
                _collisions = newCollisions,
                _factMapVersionRef = factMapVersionRef
            };
        }
        
        
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            var res = Enumerable.Empty<KeyValuePair<string, object>>();
            if (_values != null) res = res.Concat(_values);
            if (_collisions != null) res = res.Concat(_collisions.SelectMany(x => x));
            if (_references != null) res = res.Concat(_references.SelectMany(x => x));
            return res.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override int GetHashCode()
        {
            if (_hash == 0)
            {
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
            }

            return _hash;
        }

        public override bool Equals(object obj)
        {
            var mapNode = obj as FactMapNode;
            return mapNode != null && Equals(mapNode);
        }

        public bool Equals(FactMapNode other)
        {
            // Compare values
            if ((_values != null) == (other._values != null))
            {
                if (_values != null && !ReferenceEquals(_values, other._values))
                {
                    if (_values.Length != other._values.Length) return false;
                    for (var i = 0; i < _values.Length; i++)
                        if (!_values[i].Equals(other._values[i])) return false;
                }
            }
            else return false;

            // Compare collision collections
            if ((_collisions != null) == (other._collisions != null))
            {
                if (_collisions != null && !ReferenceEquals(_collisions, other._collisions))
                {
                    if (_collisions.Length != other._collisions.Length) return false;
                    for (var i = 0; i < _collisions.Length; i++)
                    {
                        var c1 = _collisions[i];
                        var c2 = other._collisions[i];
                        if (!ReferenceEquals(c1, c2) && !c1.ContentEqual(c2)) return false;
                    }
                }
            }
            else return false;

            // Compare references
            if ((_references != null) == (other._references != null))
            {
                if (_references != null)
                {
                    if (_references.Length != other._references.Length) return false;
                    for (var i = 0; i < _references.Length; i++)
                    {
                        var r1 = _references[i];
                        var r2 = other._references[i];

                        if (!ReferenceEquals(r1, r2) && !r1.Equals(r2)) return false;
                    }
                }
            }

            return true;
        }

        public static T[] Copy<T>(T[] arr) => arr?.ToArray();

        public static byte BitCount(uint i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (byte)((((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24);
        }
    }
}
