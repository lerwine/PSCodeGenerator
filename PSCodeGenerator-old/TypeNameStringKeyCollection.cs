using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PSCodeGenerator
{
    internal class TypeNameStringKeyCollection : ICollection<string>, ICollection
    {
        private ICollection<ITypeNamingIdentifier> _innerCollection;

        public TypeNameStringKeyCollection(ICollection<ITypeNamingIdentifier> collection) { _innerCollection = collection ?? new ITypeNamingIdentifier[0]; }

        public int Count => _innerCollection.Count;

        bool ICollection<string>.IsReadOnly => true;

        bool ICollection.IsSynchronized => _innerCollection is ICollection && ((ICollection)_innerCollection).IsSynchronized;

        object ICollection.SyncRoot => (_innerCollection is ICollection) ? ((ICollection)_innerCollection).SyncRoot : null;

        void ICollection<string>.Add(string item) => throw new NotSupportedException();

        void ICollection<string>.Clear() => throw new NotSupportedException();

        public bool Contains(string item) => item != null && _innerCollection.Any(n => n.GetFullName() == item);

        public void CopyTo(string[] array, int arrayIndex) => _innerCollection.Select(n => n.GetFullName()).ToArray().CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => _innerCollection.Select(n => n.GetFullName()).ToArray().CopyTo(array, index);

        IEnumerator<string> IEnumerable<string>.GetEnumerator() => _innerCollection.Select(n => n.GetFullName()).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _innerCollection.Select(n => n.GetFullName()).GetEnumerator();

        bool ICollection<string>.Remove(string item) => throw new NotSupportedException();
    }
}