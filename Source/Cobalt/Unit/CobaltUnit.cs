using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Cobalt.Unit
{
    public class CobaltUnit : DynamicObject
    {
        private readonly bool _emptyStringWhenMissing;
        private readonly Dictionary<string, object> _facts;


        public CobaltUnit(bool emptyStringWhenMissing = false, ExpandoObject root = null)
        {
            _facts = new Dictionary<string, object>();
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
            if (_facts.ContainsKey(key))
            {
                result = _facts[key];
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
            obj._facts
                .Where(pair => !_facts.ContainsKey(pair.Key))
                .ToList()
                .ForEach(pair => UpdateDictionary(pair.Key, pair.Value));
            return this;
        }

        public dynamic Augment(ExpandoObject obj)
        {
            obj
                .Where(pair => !_facts.ContainsKey(pair.Key))
                .ToList()
                .ForEach(pair => UpdateDictionary(pair.Key, pair.Value));
            return this;
        }

        public T ValueOrDefault<T>(string propertyName, T defaultValue)
        {
            return _facts.ContainsKey(propertyName)
                ? (T) _facts[propertyName]
                : defaultValue;
        }


        public bool HasProperty(string name)
        {
            return _facts.ContainsKey(name);
        }


        public override string ToString()
        {
            return string.Join(", ", _facts.Select(pair => pair.Key + " = " + pair.Value ?? "(null)").ToArray());
        }

        private void UpdateDictionary(string name, object value)
        {
            if (_facts.ContainsKey(name))
                _facts[name] = value;
            else
                _facts.Add(name, value);
        }

        public dynamic ToDynamic()
        {
            return this;
        }

        #endregion
    }
}