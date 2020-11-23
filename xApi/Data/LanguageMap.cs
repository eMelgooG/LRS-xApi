using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data
{
    public class LanguageMap : IDictionary<string,string>
    {
        private IDictionary<string, string> _values = new Dictionary<string, string>();
        public LanguageMap() { }
        public LanguageMap(IEnumerable<KeyValuePair<string, string>> values)
        {
            foreach (var item in values)
            {
                Add(item);
            }
        }

        #region Implementation
        public string this[string key] { get => _values[key]; set => _values[key] = value; }

        public int Count => _values.Count;

        public bool IsReadOnly => throw new System.NotImplementedException();

        public ICollection<string> Keys => _values.Keys;

        public ICollection<string> Values => _values.Values;



        public void Add(string key, string value)
        {
            _values.Add(key, value);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            _values.Add(item);
        }

        public void Clear()
        {
            _values.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return _values.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _values.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _values.Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return _values.Remove(item);
        }

        public bool TryGetValue(string key, out string value)
        {
            return _values.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }
        #endregion
    }
}