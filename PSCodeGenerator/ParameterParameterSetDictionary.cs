using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSCodeGenerator
{
    public class ParameterParameterSetDictionary : IDictionary<string, ParameterSetParameter>, IList<ParameterSetParameter>, IDictionary, IList
    {
        ParameterSetParameter IDictionary<string, ParameterSetParameter>.this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        ParameterSetParameter IList<ParameterSetParameter>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        object IDictionary.this[object key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ParameterDefinition Definition
        {
            get => default(ParameterDefinition);
            set
            {
            }
        }

        ICollection<string> IDictionary<string, ParameterSetParameter>.Keys => throw new NotImplementedException();

        ICollection IDictionary.Keys => throw new NotImplementedException();

        ICollection<ParameterSetParameter> IDictionary<string, ParameterSetParameter>.Values => throw new NotImplementedException();

        ICollection IDictionary.Values => throw new NotImplementedException();

        int ICollection<KeyValuePair<string, ParameterSetParameter>>.Count => throw new NotImplementedException();

        int ICollection<ParameterSetParameter>.Count => throw new NotImplementedException();

        int ICollection.Count => throw new NotImplementedException();

        bool ICollection<KeyValuePair<string, ParameterSetParameter>>.IsReadOnly => throw new NotImplementedException();

        bool ICollection<ParameterSetParameter>.IsReadOnly => throw new NotImplementedException();

        bool IDictionary.IsReadOnly => throw new NotImplementedException();

        bool IList.IsReadOnly => throw new NotImplementedException();

        bool IDictionary.IsFixedSize => throw new NotImplementedException();

        bool IList.IsFixedSize => throw new NotImplementedException();

        object ICollection.SyncRoot => throw new NotImplementedException();

        bool ICollection.IsSynchronized => throw new NotImplementedException();

        void IDictionary<string, ParameterSetParameter>.Add(string key, ParameterSetParameter value)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, ParameterSetParameter>>.Add(KeyValuePair<string, ParameterSetParameter> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<ParameterSetParameter>.Add(ParameterSetParameter item)
        {
            throw new NotImplementedException();
        }

        void IDictionary.Add(object key, object value)
        {
            throw new NotImplementedException();
        }

        int IList.Add(object value)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, ParameterSetParameter>>.Clear()
        {
            throw new NotImplementedException();
        }

        void ICollection<ParameterSetParameter>.Clear()
        {
            throw new NotImplementedException();
        }

        void IDictionary.Clear()
        {
            throw new NotImplementedException();
        }

        void IList.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, ParameterSetParameter>>.Contains(KeyValuePair<string, ParameterSetParameter> item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<ParameterSetParameter>.Contains(ParameterSetParameter item)
        {
            throw new NotImplementedException();
        }

        bool IDictionary.Contains(object key)
        {
            throw new NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, ParameterSetParameter>.ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, ParameterSetParameter>>.CopyTo(KeyValuePair<string, ParameterSetParameter>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        void ICollection<ParameterSetParameter>.CopyTo(ParameterSetParameter[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<string, ParameterSetParameter>> IEnumerable<KeyValuePair<string, ParameterSetParameter>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<ParameterSetParameter> IEnumerable<ParameterSetParameter>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        int IList<ParameterSetParameter>.IndexOf(ParameterSetParameter item)
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        void IList<ParameterSetParameter>.Insert(int index, ParameterSetParameter item)
        {
            throw new NotImplementedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, ParameterSetParameter>.Remove(string key)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, ParameterSetParameter>>.Remove(KeyValuePair<string, ParameterSetParameter> item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<ParameterSetParameter>.Remove(ParameterSetParameter item)
        {
            throw new NotImplementedException();
        }

        void IDictionary.Remove(object key)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        void IList<ParameterSetParameter>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, ParameterSetParameter>.TryGetValue(string key, out ParameterSetParameter value)
        {
            throw new NotImplementedException();
        }
    }
}