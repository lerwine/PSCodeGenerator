using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSCodeGenerator
{
    public class FunctionParameterDictionary : IDictionary<string, ParameterDefinition>, IList<ParameterDefinition>, IDictionary, IList
    {
        ParameterDefinition IDictionary<string, ParameterDefinition>.this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        ParameterDefinition IList<ParameterDefinition>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        object IDictionary.this[object key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public FunctionDefinition Function
        {
            get => default(FunctionDefinition);
            set
            {
            }
        }

        ICollection<string> IDictionary<string, ParameterDefinition>.Keys => throw new NotImplementedException();

        ICollection IDictionary.Keys => throw new NotImplementedException();

        ICollection<ParameterDefinition> IDictionary<string, ParameterDefinition>.Values => throw new NotImplementedException();

        ICollection IDictionary.Values => throw new NotImplementedException();

        int ICollection<KeyValuePair<string, ParameterDefinition>>.Count => throw new NotImplementedException();

        int ICollection<ParameterDefinition>.Count => throw new NotImplementedException();

        int ICollection.Count => throw new NotImplementedException();

        bool ICollection<KeyValuePair<string, ParameterDefinition>>.IsReadOnly => throw new NotImplementedException();

        bool ICollection<ParameterDefinition>.IsReadOnly => throw new NotImplementedException();

        bool IDictionary.IsReadOnly => throw new NotImplementedException();

        bool IList.IsReadOnly => throw new NotImplementedException();

        bool IDictionary.IsFixedSize => throw new NotImplementedException();

        bool IList.IsFixedSize => throw new NotImplementedException();

        object ICollection.SyncRoot => throw new NotImplementedException();

        bool ICollection.IsSynchronized => throw new NotImplementedException();

        void IDictionary<string, ParameterDefinition>.Add(string key, ParameterDefinition value)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, ParameterDefinition>>.Add(KeyValuePair<string, ParameterDefinition> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<ParameterDefinition>.Add(ParameterDefinition item)
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

        void ICollection<KeyValuePair<string, ParameterDefinition>>.Clear()
        {
            throw new NotImplementedException();
        }

        void ICollection<ParameterDefinition>.Clear()
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

        bool ICollection<KeyValuePair<string, ParameterDefinition>>.Contains(KeyValuePair<string, ParameterDefinition> item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<ParameterDefinition>.Contains(ParameterDefinition item)
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

        bool IDictionary<string, ParameterDefinition>.ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, ParameterDefinition>>.CopyTo(KeyValuePair<string, ParameterDefinition>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        void ICollection<ParameterDefinition>.CopyTo(ParameterDefinition[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<string, ParameterDefinition>> IEnumerable<KeyValuePair<string, ParameterDefinition>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<ParameterDefinition> IEnumerable<ParameterDefinition>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        int IList<ParameterDefinition>.IndexOf(ParameterDefinition item)
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        void IList<ParameterDefinition>.Insert(int index, ParameterDefinition item)
        {
            throw new NotImplementedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, ParameterDefinition>.Remove(string key)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, ParameterDefinition>>.Remove(KeyValuePair<string, ParameterDefinition> item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<ParameterDefinition>.Remove(ParameterDefinition item)
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

        void IList<ParameterDefinition>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, ParameterDefinition>.TryGetValue(string key, out ParameterDefinition value)
        {
            throw new NotImplementedException();
        }
    }
}