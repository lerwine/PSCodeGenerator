using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PSCodeGenerator
{
    public partial class ParameterSetDefinition
    {
        private object _syncRoot = new object();
        public ParameterSetParameterDictionary Parameters
        {
            get => default(ParameterSetParameterDictionary);
            set
            {
            }
        }

        public OutputTypeCollection OutputTypes
        {
            get => default(OutputTypeCollection);
            set
            {
            }
        }

        private FunctionDefinition _function = null;

        public FunctionDefinition Function
        {
            get => _function;
            set
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (value == null)
                    {
                        if (_function != null)
                            _function.ParameterSets.Remove(this);
                    }
                    else if (_function == null || !ReferenceEquals(_function, value))
                        value.ParameterSets.Add(this);
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }
        public string Name { get; private set; }

        public abstract class BaseParameterSetDictionary : IDictionary<string, ParameterSetDefinition>, IList<ParameterSetDefinition>, IDictionary, IList
        {
            private List<ParameterSetDefinition> _innerList = new List<ParameterSetDefinition>();

            private object _syncRoot = new object();
            ParameterSetDefinition IDictionary<string, ParameterSetDefinition>.this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            ParameterSetDefinition IList<ParameterSetDefinition>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            object IDictionary.this[object key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public abstract FunctionDefinition Function { get; }

            ICollection<string> IDictionary<string, ParameterSetDefinition>.Keys => throw new NotImplementedException();

            ICollection IDictionary.Keys => throw new NotImplementedException();

            ICollection<ParameterSetDefinition> IDictionary<string, ParameterSetDefinition>.Values => throw new NotImplementedException();

            ICollection IDictionary.Values => throw new NotImplementedException();

            int ICollection<KeyValuePair<string, ParameterSetDefinition>>.Count => throw new NotImplementedException();

            int ICollection<ParameterSetDefinition>.Count => throw new NotImplementedException();

            int ICollection.Count => throw new NotImplementedException();

            bool ICollection<KeyValuePair<string, ParameterSetDefinition>>.IsReadOnly => throw new NotImplementedException();

            bool ICollection<ParameterSetDefinition>.IsReadOnly => throw new NotImplementedException();

            bool IDictionary.IsReadOnly => throw new NotImplementedException();

            bool IList.IsReadOnly => throw new NotImplementedException();

            bool IDictionary.IsFixedSize => throw new NotImplementedException();

            bool IList.IsFixedSize => throw new NotImplementedException();

            object ICollection.SyncRoot => throw new NotImplementedException();

            bool ICollection.IsSynchronized => throw new NotImplementedException();

            void IDictionary<string, ParameterSetDefinition>.Add(string key, ParameterSetDefinition value)
            {
                throw new NotImplementedException();
            }

            void ICollection<KeyValuePair<string, ParameterSetDefinition>>.Add(KeyValuePair<string, ParameterSetDefinition> item)
            {
                throw new NotImplementedException();
            }

            public int IndexOfKey(string key)
            {
                throw new NotImplementedException();
            }

            public void Add(ParameterSetDefinition item)
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    Monitor.Enter(item._syncRoot);
                    try
                    {
                        if (ContainsKey(item.Name))
                            throw new ArgumentOutOfRangeException();
                        if (item._function != null)
                        {
                            if (ReferenceEquals(item._function, Function))
                                return;
                            item._function.ParameterSets._innerList.Remove(item);
                        }
                        else
                            _innerList.Add(item);
                        item._function = Function;
                    }
                    finally { Monitor.Exit(item._syncRoot); }
                }
                finally { Monitor.Exit(_syncRoot); }
            }

            void IDictionary.Add(object key, object value)
            {
                throw new NotImplementedException();
            }

            int IList.Add(object value)
            {
                throw new NotImplementedException();
            }

            void ICollection<KeyValuePair<string, ParameterSetDefinition>>.Clear()
            {
                throw new NotImplementedException();
            }

            void ICollection<ParameterSetDefinition>.Clear()
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

            bool ICollection<KeyValuePair<string, ParameterSetDefinition>>.Contains(KeyValuePair<string, ParameterSetDefinition> item)
            {
                throw new NotImplementedException();
            }

            bool ICollection<ParameterSetDefinition>.Contains(ParameterSetDefinition item)
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

            public bool ContainsKey(string key)
            {
                throw new NotImplementedException();
            }

            void ICollection<KeyValuePair<string, ParameterSetDefinition>>.CopyTo(KeyValuePair<string, ParameterSetDefinition>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            void ICollection<ParameterSetDefinition>.CopyTo(ParameterSetDefinition[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            void ICollection.CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            IEnumerator<KeyValuePair<string, ParameterSetDefinition>> IEnumerable<KeyValuePair<string, ParameterSetDefinition>>.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator<ParameterSetDefinition> IEnumerable<ParameterSetDefinition>.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IDictionaryEnumerator IDictionary.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            int IList<ParameterSetDefinition>.IndexOf(ParameterSetDefinition item)
            {
                throw new NotImplementedException();
            }

            int IList.IndexOf(object value)
            {
                throw new NotImplementedException();
            }

            void IList<ParameterSetDefinition>.Insert(int index, ParameterSetDefinition item)
            {
                throw new NotImplementedException();
            }

            void IList.Insert(int index, object value)
            {
                throw new NotImplementedException();
            }

            bool IDictionary<string, ParameterSetDefinition>.Remove(string key)
            {
                throw new NotImplementedException();
            }

            bool ICollection<KeyValuePair<string, ParameterSetDefinition>>.Remove(KeyValuePair<string, ParameterSetDefinition> item)
            {
                throw new NotImplementedException();
            }

            public bool Remove(ParameterSetDefinition item)
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

            void IList<ParameterSetDefinition>.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            void IList.RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            bool IDictionary<string, ParameterSetDefinition>.TryGetValue(string key, out ParameterSetDefinition value)
            {
                throw new NotImplementedException();
            }
        }
    }
}