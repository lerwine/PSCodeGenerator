using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace PSCodeGenerator
{
    internal class DictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator, IEnumerator<KeyValuePair<TKey, TValue>>
    {
        private IEnumerator<KeyValuePair<TKey, TValue>> _innerEnumerator;

        public DictionaryEnumerator(IDictionary<TKey, TValue> innerDictionary)
        {
            _innerEnumerator = (innerDictionary ?? new Dictionary<TKey, TValue>()).GetEnumerator();
            try { Entry = new DictionaryEntry(_innerEnumerator.Current.Key, _innerEnumerator.Current.Value); } catch { Entry = new DictionaryEntry(); }
        }

        public DictionaryEntry Entry { get; private set; }

        public TKey Key => _innerEnumerator.Current.Key;

        object IDictionaryEnumerator.Key => Key;

        public TValue Value => _innerEnumerator.Current.Value;

        object IDictionaryEnumerator.Value => Value;

        public KeyValuePair<TKey, TValue> Current => throw new System.NotImplementedException();

        object IEnumerator.Current => Current;

        public void Dispose() => _innerEnumerator.Dispose();

        public bool MoveNext()
        {
            if (_innerEnumerator.MoveNext())
            {
                Entry = new DictionaryEntry(_innerEnumerator.Current.Key, _innerEnumerator.Current.Value);
                return true;
            }

            try { Entry = new DictionaryEntry(_innerEnumerator.Current.Key, _innerEnumerator.Current.Value); } catch { Entry = new DictionaryEntry(); }
            return false;
        }

        public void Reset()
        {
            _innerEnumerator.Reset();
            try { Entry = new DictionaryEntry(_innerEnumerator.Current.Key, _innerEnumerator.Current.Value); } catch { Entry = new DictionaryEntry(); }
        }
    }
}