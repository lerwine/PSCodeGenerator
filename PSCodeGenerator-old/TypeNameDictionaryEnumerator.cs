using System.Collections;
using System.Collections.Generic;

namespace PSCodeGenerator
{
    internal class TypeNameDictionaryEnumerator : IEnumerator<KeyValuePair<string, ITypeNamingIdentifier>>, IDictionaryEnumerator
    {
        private IEnumerator<ITypeNamingIdentifier> _innerEnumerator;
        private KeyValuePair<string, ITypeNamingIdentifier>? _current;
        private DictionaryEntry? _entry;
        public TypeNameDictionaryEnumerator(IEnumerable<ITypeNamingIdentifier> innerList)
        {
            _innerEnumerator = (innerList ?? new ITypeNamingIdentifier[0]).GetEnumerator();
        }

        public KeyValuePair<string, ITypeNamingIdentifier> Current
        {
            get
            {
                KeyValuePair<string, ITypeNamingIdentifier>? current = _current;
                if (!current.HasValue)
                    current = new KeyValuePair<string, ITypeNamingIdentifier>(_innerEnumerator.Current.GetFullName(), _innerEnumerator.Current);
                return current.Value;
            }
        }

        public DictionaryEntry Entry
        {
            get
            {
                DictionaryEntry? entry = _entry;
                if (!entry.HasValue)
                    entry = new DictionaryEntry(Current.Key, _innerEnumerator.Current);
                return entry.Value;
            }
        }

        public string Key => Current.Key;

        object IDictionaryEnumerator.Key => Current.Key;

        public ITypeNamingIdentifier Value => _innerEnumerator.Current;

        object IDictionaryEnumerator.Value => _innerEnumerator.Current;

        object IEnumerator.Current => Entry;

        public void Dispose()
        {
            _innerEnumerator.Dispose();
            _current = null;
            _entry = null;
        }

        public bool MoveNext()
        {
            _current = null;
            _entry = null;
            return MoveNext();
        }

        public void Reset()
        {
            _current = null;
            _entry = null;
            _innerEnumerator.Reset();
        }
    }
}