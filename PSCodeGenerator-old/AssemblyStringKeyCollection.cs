using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PSCodeGenerator
{
    internal class AssemblyStringKeyCollection : ICollection<string>, ICollection
    {
        private ICollection<AssemblyInfo> _innerCollection;

        public AssemblyStringKeyCollection(ICollection<AssemblyInfo> collection) { _innerCollection = collection ?? new AssemblyInfo[0]; }

        public int Count => _innerCollection.Count;
        
        bool ICollection<string>.IsReadOnly => true;

        bool ICollection.IsSynchronized => _innerCollection is ICollection && ((ICollection)_innerCollection).IsSynchronized;

        object ICollection.SyncRoot => (_innerCollection is ICollection) ? ((ICollection)_innerCollection).SyncRoot : null;

        void ICollection<string>.Add(string item) => throw new NotSupportedException();

        void ICollection<string>.Clear() => throw new NotSupportedException();

        public bool Contains(string item) => item != null && _innerCollection.Any(n => n.FullName == item);

        public void CopyTo(string[] array, int arrayIndex) => _innerCollection.Select(n => n.FullName).ToArray().CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => _innerCollection.Select(n => n.FullName).ToArray().CopyTo(array, index);

        IEnumerator<string> IEnumerable<string>.GetEnumerator() => _innerCollection.Select(n => n.FullName).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _innerCollection.Select(n => n.FullName).GetEnumerator();

        bool ICollection<string>.Remove(string item) => throw new NotSupportedException();
    }
}