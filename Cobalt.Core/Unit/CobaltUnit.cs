using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Cobalt.Unit
{
    public class CobaltUnit : DynamicObject
    {
        private readonly bool _emptyStringWhenMissing;
        private readonly Dictionary<string, object> _valueStore;


        public CobaltUnit(bool emptyStringWhenMissing, ExpandoObject root)
        {
            _valueStore = new Dictionary<string, object>();
            _emptyStringWhenMissing = emptyStringWhenMissing;
            if (root != null) Augment(root);
        }

        #region -- Dynamics; May go away from these in favor of simple dictionaries

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            UpdateDictionary(binder.Name, value);
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes[0] is string)
            {
                var key = indexes[0] as string;
                UpdateDictionary(key, value);
                return true;
            }

            return base.TrySetIndex(binder, indexes, value);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var key = binder.Name;
            if (_valueStore.ContainsKey(key))
            {
                result = _valueStore[key];
                return true;
            }

            if (_emptyStringWhenMissing)
            {
                result = string.Empty;
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public dynamic Augment(CobaltUnit obj)
        {
            //existing properties are not overwritten
            obj._valueStore
                .Where(pair => !_valueStore.ContainsKey(pair.Key))
                .ToList()
                .ForEach(pair => UpdateDictionary(pair.Key, pair.Value));
            return this;
        }

        public dynamic Augment(ExpandoObject obj)
        {
            obj
                .Where(pair => !_valueStore.ContainsKey(pair.Key))
                .ToList()
                .ForEach(pair => UpdateDictionary(pair.Key, pair.Value));
            return this;
        }

        public T ValueOrDefault<T>(string propertyName, T defaultValue)
        {
            return _valueStore.ContainsKey(propertyName)
                ? (T) _valueStore[propertyName]
                : defaultValue;
        }


        public bool HasProperty(string name)
        {
            return _valueStore.ContainsKey(name);
        }


        public override string ToString()
        {
            return string.Join(", ", _valueStore.Select(pair => pair.Key + " = " + pair.Value ?? "(null)").ToArray());
        }

        private void UpdateDictionary(string name, object value)
        {
            if (_valueStore.ContainsKey(name))
                _valueStore[name] = value;
            else
                _valueStore.Add(name, value);
        }

        public dynamic ToDynamic()
        {
            return this;
        }

        #endregion
    }
}