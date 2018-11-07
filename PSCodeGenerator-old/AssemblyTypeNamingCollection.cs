using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace PSCodeGenerator
{
    /// <summary>
    /// A collection of <seealso cref="ITypeNamingIdentifier"/> elements that represent types and namespaces contained within an <seealso cref="AssemblyInfo"/>.
    /// </summary>
    public class AssemblyTypeNamingCollection : ICompatibleList<ITypeNamingIdentifier>, ICompatibleDictionary<string, ITypeNamingIdentifier>
    {
        private readonly List<ITypeNamingIdentifier> _innerList = new List<ITypeNamingIdentifier>();

        ///	<summary>
        ///	Gets the element with the full name matching the specified key.
        ///	</summary>
        ///	<param name="key">The full name of the element to get or set.</param>
        ///	<returns>The element matching the specified full name or <c>null</c> if an element with a matching full name is not found.</returns>
        public ITypeNamingIdentifier this[string key] { get => throw new NotImplementedException(); }

        ITypeNamingIdentifier IDictionary<string, ITypeNamingIdentifier>.this[string key] { get => throw new NotImplementedException(); set => throw new NotSupportedException(); }

        object IDictionary.this[object key] { get => throw new NotImplementedException(); set => throw new NotSupportedException(); }

        ///	<summary>
        ///	Gets or sets the element at the specified index.
        ///	</summary>
        ///	<param name="index">The zero-based index of the element to get or set.</param>
        ///	<returns>The element at the specified index.</returns>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.
        ///	<para>-or- <paramref name="index"/> is equal to or greater than <see cref="Count"/>.</para></exception>
        public ITypeNamingIdentifier this[int index] { get => _innerList[index]; set => _innerList[index] = value; }

        object IList.this[int index] { get => _innerList[index]; set => throw new NotImplementedException(); }

        /// <summary>
        /// Owning <seealso cref="AssemblyInfo"/> for this collection.
        /// </summary>
        public AssemblyInfo Assembly { get; private set; }

        /// <summary>
        /// Gets the <seealso cref="IEqualityComparer{string}"/> that is used to determine equality of keys for the dictionary.
        /// </summary>
        public IEqualityComparer<string> Comparer => throw new NotImplementedException();

        ///	<summary>
        ///	Gets an <seealso cref="ICollection{string}"/> containing the keys (full names) of the <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<returns>An <seealso cref="ICollection{string}"/> containing the keys of the object that implements <see cref="AssemblyTypeNamingCollection"/>.</returns>
        public ICollection<string> Keys => throw new NotImplementedException();

        ICollection IDictionary.Keys => throw new NotImplementedException();

        ICollection<ITypeNamingIdentifier> IDictionary<string, ITypeNamingIdentifier>.Values => this;

        ICollection IDictionary.Values => this;

        ///	<summary>
        ///	Gets the number of elements contained in the <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<returns>The number of elements contained in the <see cref="AssemblyTypeNamingCollection"/>.</returns>
        public int Count => _innerList.Count;

        bool ICollection<KeyValuePair<string, ITypeNamingIdentifier>>.IsReadOnly => true;

        bool IDictionary.IsReadOnly => true;

        bool IList.IsReadOnly => false;

        bool ICollection<ITypeNamingIdentifier>.IsReadOnly => false;

        bool IDictionary.IsFixedSize => false;

        bool IList.IsFixedSize => false;

        bool ICollection.IsSynchronized => true;

        object ICollection.SyncRoot => throw new NotImplementedException();

        /// <summary>
        /// Initializes a new <see cref="AssemblyTypeNamingCollection"/> owned by a <seealso cref="AssemblyInfo"/>.
        /// </summary>
        /// <param name="owner"><seealso cref="AssemblyInfo"/> owned by this collection.</param>
        public AssemblyTypeNamingCollection(AssemblyInfo owner)
        {
            if (owner == null || owner.Types != null)
                throw new InvalidOperationException();
            Assembly = owner;
        }

        void ICollection<ITypeNamingIdentifier>.Add(ITypeNamingIdentifier item)
        {
            _innerList.Add(item);
        }

        void IDictionary<string, ITypeNamingIdentifier>.Add(string key, ITypeNamingIdentifier value) => throw new NotSupportedException();

        void ICollection<KeyValuePair<string, ITypeNamingIdentifier>>.Add(KeyValuePair<string, ITypeNamingIdentifier> item) => throw new NotSupportedException();

        void IDictionary.Add(object key, object value) => throw new NotSupportedException();

        int IList.Add(object value)
        {
            throw new NotImplementedException();
        }
        
        void ICompatibleList<ITypeNamingIdentifier>.AddRange(IEnumerable<ITypeNamingIdentifier> collection)
        {
            _innerList.AddRange(collection);
        }

        /// <summary>
        /// Determines whether an item can be added to the collection.
        /// </summary>
        /// <param name="item">Candidate item.</param>
        /// <returns><c>true</c> if <paramref name="item"/> can be added; otherwise, <c>false</c>.</returns>
        protected internal bool CanAdd(ITypeNamingIdentifier item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines if a type can be imported into the <see cref="AssemblyTypeNamingCollection"/>.
        /// </summary>
        /// <param name="type">Candidate <seealso cref="Type"/> object.</param>
        /// <returns><c>true</c> if <paramref name="type"/> can be imported; otherwise, <c>false</c></returns>
        public bool CanImport(Type type)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Removes all elements from the <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        public void Clear()
        {
            _innerList.Clear();
        }

        ///	<summary>
        ///	Determines whether an element is in the <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyTypeNamingCollection"/></param>
        ///	<returns><c>true</c> if <paramref name="item"/> is found in the <see cref="AssemblyTypeNamingCollection"/>; otherwise, <c>false</c>.</returns>
        public bool Contains(ITypeNamingIdentifier item)
        {
            return _innerList.Contains(item);
        }

        bool ICollection<KeyValuePair<string, ITypeNamingIdentifier>>.Contains(KeyValuePair<string, ITypeNamingIdentifier> item)
        {
            throw new NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Determines whether the <see cref="AssemblyTypeNamingCollection"/> contains an element with the specified key (full name).
        ///	</summary>
        ///	<param name="key">The full name to locate in the <see cref="AssemblyTypeNamingCollection"/>.</param>
        ///	<returns><c>true</c> if the <see cref="AssemblyTypeNamingCollection"/> contains an element with a full name that matches the <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        bool IDictionary.Contains(object key)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Copies the entire <see cref="AssemblyTypeNamingCollection"/> to a compatible one-dimensional array, starting at the specified index of the target array.
        ///	</summary>
        ///	<param name="array">The one-dimensional <seealso cref="Array"/> that is the destination of the elements copied from <see cref="AssemblyTypeNamingCollection"/>.
        ///	The <seealso cref="Array"/> must have zero-based indexing.</param>
        ///	<param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        ///	<exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        ///	<exception cref="ArgumentException">The number of elements in the source <see cref="AssemblyTypeNamingCollection"/> is greater than the available space from
        ///	<paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo(ITypeNamingIdentifier[] array, int arrayIndex)
        {
            _innerList.CopyTo(array, arrayIndex);
        }

        void ICollection<KeyValuePair<string, ITypeNamingIdentifier>>.CopyTo(KeyValuePair<string, ITypeNamingIdentifier>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Determines whether the <see cref="AssemblyTypeNamingCollection"/> contains elements that match the conditions defined by the specified predicate.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{ITypeNamingIdentifier}"/> delegate that defines the conditions of the elements to search for.</param>
        ///	<returns><c>true</c> if the <see cref="AssemblyTypeNamingCollection"/> contains one or more elements that match the conditions defined by the specified predicate; otherwise, <c>false</c>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public bool Exists(Predicate<ITypeNamingIdentifier> match)
        {
            return _innerList.Exists(match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{ITypeNamingIdentifier}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <seealso cref="ITypeNamingIdentifier"/>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public ITypeNamingIdentifier Find(Predicate<ITypeNamingIdentifier> match)
        {
            return _innerList.Find(match);
        }

        ///	<summary>
        ///	Retrieves all the elements that match the conditions defined by the specified predicate.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{ITypeNamingIdentifier}"/> delegate that defines the conditions of the elements to search for.</param>
        ///	<returns>A <see cref="AssemblyTypeNamingCollection"/> containing all the elements that match the conditions defined by the specified predicate, if found;
        ///	otherwise, an empty <see cref="AssemblyTypeNamingCollection"/>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public Collection<ITypeNamingIdentifier> FindAll(Predicate<ITypeNamingIdentifier> match)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elementsin the
        ///	<see cref="AssemblyTypeNamingCollection"/> that starts at the specified index and contains the specified number of elements.
        ///	</summary>
        ///	<param name="startIndex">The zero-based starting index of the search.</param>
        ///	<param name="count">The number of elements in the section to search.</param>
        ///	<param name="match">The <seealso cref="Predicate{ITypeNamingIdentifier}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="AssemblyTypeNamingCollection"/>. 
        ///	<para>-or- <paramref name="count"/> is less than 0.</para>
        ///	<para>-or- <paramref name="startIndex"/> and <paramref name="count"/> do not specify a valid section in the <see cref="AssemblyTypeNamingCollection"/>.</para></exception>
        public int FindIndex(int startIndex, int count, Predicate<ITypeNamingIdentifier> match)
        {
            return _innerList.FindIndex(startIndex, count, match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the
        ///	<see cref="AssemblyTypeNamingCollection"/> that extends from the specified index to the last element.
        ///	</summary>
        ///	<param name="startIndex">The zero-based starting index of the search.</param>
        ///	<param name="match">The <seealso cref="Predicate{ITypeNamingIdentifier}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="AssemblyTypeNamingCollection"/>.</exception>
        public int FindIndex(int startIndex, Predicate<ITypeNamingIdentifier> match)
        {
            return _innerList.FindIndex(startIndex, match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire
        ///	<see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{ITypeNamingIdentifier}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public int FindIndex(Predicate<ITypeNamingIdentifier> match)
        {
            return _innerList.FindIndex(match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{ITypeNamingIdentifier}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <seealso cref="ITypeNamingIdentifier"/>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public ITypeNamingIdentifier FindLast(Predicate<ITypeNamingIdentifier> match)
        {
            return _innerList.FindLast(match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the
        ///	<see cref="AssemblyTypeNamingCollection"/> that contains the specified number of elements and ends at the specified index.
        ///	</summary>
        ///	<param name="startIndex">The zero-based starting index of the backward search.</param>
        ///	<param name="count">The number of elements in the section to search.</param>
        ///	<param name="match">The <seealso cref="Predicate{ITypeNamingIdentifier}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="AssemblyTypeNamingCollection"/>.
        ///	<para>-or- <paramref name="count"/> is less than 0.</para>
        ///	<para>-or- <paramref name="startIndex"/> and <paramref name="count"/> do not specify a valid section in the <see cref="AssemblyTypeNamingCollection"/>.</para></exception>
        public int FindLastIndex(int startIndex, int count, Predicate<ITypeNamingIdentifier> match)
        {
            return _innerList.FindLastIndex(startIndex, count, match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the
        ///	<see cref="AssemblyTypeNamingCollection"/> that extends from the first element to the specified index.
        ///	</summary>
        ///	<param name="startIndex">The zero-based starting index of the backward search.</param>
        ///	<param name="match">The <seealso cref="Predicate{ITypeNamingIdentifier}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="AssemblyTypeNamingCollection"/>.</exception>
        public int FindLastIndex(int startIndex, Predicate<ITypeNamingIdentifier> match)
        {
            return _innerList.FindLastIndex(startIndex, match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the entire
        ///	<see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{ITypeNamingIdentifier}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public int FindLastIndex(Predicate<ITypeNamingIdentifier> match)
        {
            return _innerList.FindLastIndex(match);
        }

        ///	<summary>
        ///	Performs the specified action on each element of the <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="action">The System.Action`1 delegate to perform on each element of the <see cref="AssemblyTypeNamingCollection"/>.</param>
        ///	<exception cref="ArgumentNullException">action is <c>null</c>.</exception>
        ///	<exception cref="InvalidOperationException">An element in the collection has been modified.</exception>
        public void ForEach(Action<ITypeNamingIdentifier> action)
        {
            _innerList.ForEach(action);
        }

        ///	<summary>
        ///	Returns an enumerator that iterates through the <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<returns>A <see cref="AssemblyTypeNamingCollection"/>.Enumerator for the <see cref="AssemblyTypeNamingCollection"/>.</returns>
        public IEnumerator<ITypeNamingIdentifier> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, ITypeNamingIdentifier>> IEnumerable<KeyValuePair<string, ITypeNamingIdentifier>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Creates a shallow copy of a range of elements in the source <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="index">The zero-based <see cref="AssemblyTypeNamingCollection"/> index at which the range starts.</param>
        ///	<param name="count">The number of elements in the range.</param>
        ///	<returns>A shallow copy of a range of elements in the source <see cref="AssemblyTypeNamingCollection"/>.</returns>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.
        ///	<para>-or- <paramref name="count"/> is less than 0.</para></exception>
        ///	<exception cref="ArgumentException"><paramref name="index"/> and <paramref name="count"/> do not denote a valid range of elements in the <see cref="AssemblyTypeNamingCollection"/>.</exception>
        public Collection<ITypeNamingIdentifier> GetRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Imports an <seealso cref="Type"/> object to the end of the <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="item">The <seealso cref="Type"/> object to be added to the end of the <see cref="AssemblyTypeNamingCollection"/> as an <seealso cref="AssemblyInfo"/> object.</param>
        ///	<exception cref="ArgumentNullException"><paramref name="item"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentException">An element with the same <seealso cref="Type.FullName"/> already exists in the <see cref="AssemblyTypeNamingCollection"/>.
        ///	<para>-or- the full name of the Type's <seealso cref="Type.Assembly"/> does not match the <seealso cref="AssemblyInfo.FullName"/> of the <seealso cref="Assembly"/> property.</para></exception>
        public void Import(Type item)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Imports the elements of the specified collection to the end of the <see cref="AssemblyTypeNamingCollection"/> as <seealso cref="AssemblyInfo"/> objects.
        ///	</summary>
        ///	<param name="collection">The collection whose elements should be imported to the end of the <see cref="AssemblyTypeNamingCollection"/>.</param>
        ///	<exception cref="ArgumentNullException">collection is <c>null</c>.</exception>
        ///	<exception cref="ArgumentException">An item in <paramref name="collection"/> is <c>null</c>.
        ///	<para>-or- An element with the same <seealso cref="Type.FullName"/> already exists in the <see cref="AssemblyTypeNamingCollection"/>.</para>
        ///	<para>-or- the full name of the Type's <seealso cref="Type.Assembly"/> does not match the <seealso cref="AssemblyInfo.FullName"/> of the <seealso cref="Assembly"/> property.</para></exception>
        public void ImportRange(IEnumerable<Type> collection)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="AssemblyTypeNamingCollection"/>
        ///	that starts at the specified index and contains the specified number of elements.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyTypeNamingCollection"/>.</param>
        ///	<param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        ///	<param name="count">The number of elements in the section to search.</param>
        ///	<returns>The zero-based index of the first occurrence of <paramref name="item"/> within the range of elements in the <see cref="AssemblyTypeNamingCollection"/>
        ///	that starts at index and contains <paramref name="count"/> number of elements, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indexes for the <see cref="AssemblyTypeNamingCollection"/>. 
        ///	<para>-or- <paramref name="count"/> is less than 0.</para>
        ///	<para>-or- <paramref name="index"/> and <paramref name="count"/> do not specify a valid section in the <see cref="AssemblyTypeNamingCollection"/>.</para></exception>
        public int IndexOf(ITypeNamingIdentifier item, int index, int count)
        {
            return _innerList.IndexOf(item, index, count);
        }

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="AssemblyTypeNamingCollection"/>
        ///	that extends from the specified index to the last element.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyTypeNamingCollection"/>.</param>
        ///	<param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        ///	<returns>The zero-based index of the first occurrence of <paramref name="item"/> within the range of elements in the <see cref="AssemblyTypeNamingCollection"/>
        ///	that extends from index to the last element, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indexes for the <see cref="AssemblyTypeNamingCollection"/>.</exception>
        public int IndexOf(ITypeNamingIdentifier item, int index)
        {
            return _innerList.IndexOf(item, index);
        }

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyTypeNamingCollection"/>.</param>
        ///	<returns>The zero-based index of the first occurrence of <paramref name="item"/> within the entire <see cref="AssemblyTypeNamingCollection"/>, if found; otherwise, –1.</returns>
        public int IndexOf(ITypeNamingIdentifier item)
        {
            return _innerList.IndexOf(item);
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Inserts an element into the <see cref="AssemblyTypeNamingCollection"/> at the specified index.
        ///	</summary>
        ///	<param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        ///	<param name="item">The object to insert.</param>
        ///	<exception cref="ArgumentNullException"><paramref name="item"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.
        ///	<para>-or- <paramref name="index"/> is greater than <see cref="AssemblyTypeNamingCollection.Count"/>.</para></exception>
        public void Insert(int index, Type item)
        {
            throw new NotImplementedException();
        }

        void IList<ITypeNamingIdentifier>.Insert(int index, ITypeNamingIdentifier item)
        {
            _innerList.Insert(index, item);
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the last occurrence within the entire <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyTypeNamingCollection"/>.</param>
        ///	<returns>The zero-based index of the last occurrence of <paramref name="item"/> within the entire the <see cref="AssemblyTypeNamingCollection"/>, if found; otherwise, –1.</returns>
        public int LastIndexOf(ITypeNamingIdentifier item)
        {
            return _innerList.LastIndexOf(item);
        }

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="AssemblyTypeNamingCollection"/>
        ///	that extends from the first element to the specified index.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyTypeNamingCollection"/>.</param>
        ///	<param name="index">The zero-based starting index of the backward search.</param>
        ///	<returns>The zero-based index of the last occurrence of <paramref name="item"/> within the range of elements in the <see cref="AssemblyTypeNamingCollection"/>
        ///	that extends from the first element to index, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid <paramref name="index"/>es for the <see cref="AssemblyTypeNamingCollection"/>.</exception>
        public int LastIndexOf(ITypeNamingIdentifier item, int index)
        {
            return _innerList.LastIndexOf(item, index);
        }

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="AssemblyTypeNamingCollection"/>
        ///	that contains the specified number of elements and ends at the specified index.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyTypeNamingCollection"/>.</param>
        ///	<param name="index">The zero-based starting index of the backward search.</param>
        ///	<param name="count">The number of elements in the section to search.</param>
        ///	<returns>The zero-based index of the last occurrence of <paramref name="item"/> within the range of elements in the <see cref="AssemblyTypeNamingCollection"/>
        ///	that contains <paramref name="count"/> number of elements and ends at index, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid <paramref name="index"/>es for the <see cref="AssemblyTypeNamingCollection"/>.
        ///	<para>-or- <paramref name="count"/> is less than 0.</para>
        ///	<para>-or- <paramref name="index"/> and <paramref name="count"/> do not specify a valid section in the <see cref="AssemblyTypeNamingCollection"/>.</para></exception>
        public int LastIndexOf(ITypeNamingIdentifier item, int index, int count)
        {
            return _innerList.LastIndexOf(item, index, count);
        }

        ///	<summary>
        ///	Removes the element with the specified key from the <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="key">The key of the element to remove.</param>
        ///	<returns><c>true</c> if the element is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if <paramref name="key"/> was not found in the original
        ///	<see cref="AssemblyTypeNamingCollection"/>.</returns>
        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, ITypeNamingIdentifier>>.Remove(KeyValuePair<string, ITypeNamingIdentifier> item)
        {
            throw new NotImplementedException();
        }

        void IDictionary.Remove(object key)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Removes the first occurrence of a specific object from the <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="item">The object to remove from the <see cref="AssemblyTypeNamingCollection"/>.</param>
        ///	<returns><c>true</c> if <paramref name="item"/> is successfully removed; otherwise, <c>false</c>.
        ///	This method also returns <c>false</c> if <paramref name="item"/> was not found in the <see cref="AssemblyTypeNamingCollection"/>.</returns>
        public bool Remove(ITypeNamingIdentifier item)
        {
            return _innerList.Remove(item);
        }

        void IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Removes all the elements that match the conditions defined by the specified predicate.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{ITypeNamingIdentifier}"/> delegate that defines the conditions of the elements to remove.</param>
        ///	<returns>The number of elements removed from the <see cref="AssemblyTypeNamingCollection"/> .</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public int RemoveAll(Predicate<ITypeNamingIdentifier> match)
        {
            return _innerList.RemoveAll(match);
        }

        ///	<summary>
        ///	Removes the element at the specified index of the <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="index">The zero-based index of the element to remove.</param>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.
        ///	<para>-or- <paramref name="index"/> is equal to or greater than <see cref="AssemblyTypeNamingCollection.Count"/>.</para></exception>
        public void RemoveAt(int index)
        {
            _innerList.RemoveAt(index);
        }

        ///	<summary>
        ///	Removes a range of elements from the <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        ///	<param name="index">The zero-based starting index of the range of elements to remove.</param>
        ///	<param name="count">The number of elements to remove.</param>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0. -or- <paramref name="count"/> is less than 0.</exception>
        ///	<exception cref="ArgumentException"><paramref name="index"/> and <paramref name="count"/> do not denote a valid range of elements in the <see cref="AssemblyTypeNamingCollection"/>.</exception>
        public void RemoveRange(int index, int count)
        {
            _innerList.RemoveRange(index, count);
        }

        ///	<summary>
        ///	Reverses the order of the elements in the specified range.
        ///	</summary>
        ///	<param name="index">The zero-based starting index of the range to reverse.</param>
        ///	<param name="count">The number of elements in the range to reverse.</param>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0. -or- <paramref name="count"/> is less than 0.</exception>
        ///	<exception cref="ArgumentException"><paramref name="index"/> and <paramref name="count"/> do not denote a valid range of elements in the <see cref="AssemblyTypeNamingCollection"/>.</exception>
        public void Reverse(int index, int count)
        {
            _innerList.Reverse(index, count);
        }

        ///	<summary>
        ///	Reverses the order of the elements in the entire <see cref="AssemblyTypeNamingCollection"/>.
        ///	</summary>
        public void Reverse()
        {
            _innerList.Reverse();
        }

        ///	<summary>
        ///	Sorts the elements in the entire <see cref="AssemblyTypeNamingCollection"/> using the specified System.Comparison`1.
        ///	</summary>
        ///	<param name="comparison">The System.Comparison`1 to use when comparing elements.</param>
        ///	<exception cref="ArgumentNullException">comparison is <c>null</c>.</exception>
        public void Sort(Comparison<ITypeNamingIdentifier> comparison)
        {
            _innerList.Sort(comparison);
        }

        ///	<summary>
        ///	Sorts the elements in a range of elements in <see cref="AssemblyTypeNamingCollection"/> using the specified comparer.
        ///	</summary>
        ///	<param name="index">The zero-based starting index of the range to sort.</param>
        ///	<param name="count">The length of the range to sort.</param>
        ///	<param name="comparer">The <seealso cref="IComparer{ITypeNamingIdentifier}"/> implementation to use when comparing elements, or <c>null</c> to use the default comparer
        ///	<seealso cref="Comparer{ITypeNamingIdentifier}.Default"/>.</param>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.
        ///	<para>-or- <paramref name="count"/> is less than 0.</para></exception>
        ///	<exception cref="ArgumentException"><paramref name="index"/> and <paramref name="count"/> do not specify a valid range in the <see cref="AssemblyTypeNamingCollection"/>.</exception>
        public void Sort(int index, int count, IComparer<ITypeNamingIdentifier> comparer)
        {
            _innerList.Sort(index, count, comparer);
        }

        ///	<summary>
        ///	Sorts the elements in the entire <see cref="AssemblyTypeNamingCollection"/> using the default comparer.
        ///	</summary>
        public void Sort()
        {
            _innerList.Sort();
        }

        ///	<summary>
        ///	Sorts the elements in the entire <see cref="AssemblyTypeNamingCollection"/> using the specified comparer.
        ///	</summary>
        ///	<param name="comparer">The <seealso cref="IComparer{ITypeNamingIdentifier}"/> implementation to use when comparing elements, or <c>null</c> to use the default comparer
        ///	<seealso cref="Comparer{ITypeNamingIdentifier}.Default"/>.</param>
        public void Sort(IComparer<ITypeNamingIdentifier> comparer)
        {
            _innerList.Sort(comparer);
        }

        ///	<summary>
        ///	Copies the elements of the <see cref="AssemblyTypeNamingCollection"/> to a new array.
        ///	</summary>
        ///	<returns>An array containing copies of the elements of the <see cref="AssemblyTypeNamingCollection"/>.</returns>
        public ITypeNamingIdentifier[] ToArray()
        {
            return _innerList.ToArray();
        }

        ///	<summary>
        ///	Determines whether every element in the <see cref="AssemblyTypeNamingCollection"/> matches the conditions defined by the specified predicate.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{ITypeNamingIdentifier}"/> delegate that defines the conditions to check against the elements.</param>
        ///	<returns><c>true</c> if every element in the <see cref="AssemblyTypeNamingCollection"/> matches the conditions defined by the specified predicate; otherwise, <c>false</c>.
        ///	If the list has no elements, the return value is <c>true</c>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public bool TrueForAll(Predicate<ITypeNamingIdentifier> match)
        {
            return _innerList.TrueForAll(match);
        }

        ///	<summary>
        ///	Gets the value associated with the specified key.
        ///	</summary>
        ///	<param name="key">The key whose value to get.</param>
        ///	<param name="value">When this method returns, the value associated with the specified <paramref name="key"/>, if the <paramref name="key"/> is found;
        ///	otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        ///	<returns><c>true</c> if the object that implements <see cref="AssemblyTypeNamingCollection"/> contains an element with the specified <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        public bool TryGetValue(string key, out ITypeNamingIdentifier value)
        {
            throw new NotImplementedException();
        }
    }
}