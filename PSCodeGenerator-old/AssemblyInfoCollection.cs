using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PSCodeGenerator
{
    /// <summary>
    /// Represents a collection of objects that can be individually accessed by index, compatible with PowerShell operations.
    /// </summary>
    public class AssemblyInfoCollection : ICompatibleList<AssemblyInfo>
    {
        private List<AssemblyInfo> _innerList = new List<AssemblyInfo>();

        ///	<summary>
        ///	Gets or sets the element at the specified index.
        ///	</summary>
        ///	<param name="index">The zero-based index of the element to get or set.</param>
        ///	<returns>The element at the specified index.</returns>
        ///	<exception cref="ArgumentOutOfRangeException">index is not a valid index in the <see cref="AssemblyInfoCollection"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> already exists in the <see cref="AssemblyInfoCollection"/>.
        /// <para>-or- another <seealso cref="AssemblyInfo"/> item in the <see cref="AssemblyInfoCollection"/> has the same full name.</para></exception>
        public AssemblyInfo this[int index] { get => _innerList[index]; set => _innerList[index] = value; }

        object IList.this[int index] { get => _innerList[index]; set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets the number of elements contained in the  <see cref="AssemblyInfoCollection"/>.
        /// </summary>
        public int Count => _innerList.Count;

        bool IList.IsFixedSize => throw new NotImplementedException();

        bool IList.IsReadOnly => throw new NotImplementedException();

        bool ICollection<AssemblyInfo>.IsReadOnly => throw new NotImplementedException();

        bool ICollection.IsSynchronized => throw new NotImplementedException();

        object ICollection.SyncRoot => throw new NotImplementedException();

        /// <summary>
        /// Adds an object to the end of the <see cref="AssemblyInfoCollection"/>.
        /// </summary>
        /// <param name="item">Value to be added to the end of the <see cref="AssemblyInfoCollection"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="item"/> already exists in the <see cref="AssemblyInfoCollection"/>.
        /// <para>-or- another <seealso cref="AssemblyInfo"/> item in the <see cref="AssemblyInfoCollection"/> has the same full name.</para></exception>
        public void Add(AssemblyInfo item)
        {
            _innerList.Add(item);
        }

        int IList.Add(object value)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Adds the elements of the specified collection to the end of the <see cref="AssemblyInfoCollection"/>.
        ///	</summary>
        ///	<param name="collection">The collection whose elements should be added to the end of the <see cref="AssemblyInfoCollection"/>.</param>
        ///	<exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">An element in the source is null.
        /// <para>-or- An element in the source <paramref name="collection"/> already exists in the <see cref="AssemblyInfoCollection"/>.</para>
        /// <para>-or- another <seealso cref="AssemblyInfo"/> item in the <see cref="AssemblyInfoCollection"/> has the same as an element in the source <paramref name="collection"/>.</para></exception>
        public void AddRange(IEnumerable<AssemblyInfo> collection)
        {
            _innerList.AddRange(collection);
        }

        /// <summary>
        /// Removes all elements from the <see cref="AssemblyInfoCollection"/>.
        /// </summary>
        public void Clear()
        {
            _innerList.Clear();
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="AssemblyInfoCollection"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="AssemblyInfoCollection"/>.</param>
        /// <returns><c>true</c> if item is found in the <see cref="AssemblyInfoCollection"/>; otherwise, <c>false</c>.</returns>
        public bool Contains(AssemblyInfo item)
        {
            return _innerList.Contains(item);
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copies the entire <see cref="AssemblyInfoCollection"/> to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <seealso cref="Array"/> that is the destination of the elements copied from v. The <seealso cref="Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentException">The number of elements in the source <see cref="AssemblyInfoCollection"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo(AssemblyInfo[] array, int arrayIndex)
        {
            _innerList.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Determines whether the <see cref="AssemblyInfoCollection"/> contains elements that match the conditions defined by the specified predicate.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{AssemblyInfo}"/> delegate that defines the conditions of the elements to search for.</param>
        ///	<returns><c>true</c> if the <see cref="AssemblyInfoCollection"/> contains one or more elements that match the conditions defined by the specified predicate;
        ///	otherwise, <c>false</c>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public bool Exists(Predicate<AssemblyInfo> match)
        {
            return _innerList.Exists(match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire <see cref="AssemblyInfoCollection"/>.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{AssemblyInfo}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, null.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public AssemblyInfo Find(Predicate<AssemblyInfo> match)
        {
            return _innerList.Find(match);
        }

        ///	<summary>
        ///	Retrieves all the elements that match the conditions defined by the specified predicate.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{AssemblyInfo}"/> delegate that defines the conditions of the elements to search for.</param>
        ///	<returns>A <seealso cref="Collection{AssemblyInfo}"/> containing all the elements that match the conditions defined by the specified predicate, if found;
        ///	otherwise, an empty <seealso cref="Collection{AssemblyInfo}"/>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public Collection<AssemblyInfo> FindAll(Predicate<AssemblyInfo> match)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the
        ///	<see cref="AssemblyInfoCollection"/> that starts at the specified index and contains the specified number of elements.
        ///	</summary>
        ///	<param name="startIndex">The zero-based starting index of the search.</param>
        ///	<param name="count">The number of elements in the section to search.</param>
        ///	<param name="match">The <seealso cref="Predicate{AssemblyInfo}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="AssemblyInfoCollection"/>.
        ///	<para>-or- <paramref name="count"/> is less than 0.</para>
        ///	<para>-or- <paramref name="startIndex"/> and <paramref name="count"/> do not specify a valid section in the <see cref="AssemblyInfoCollection"/>.</para></exception>
        public int FindIndex(int startIndex, int count, Predicate<AssemblyInfo> match)
        {
            return _innerList.FindIndex(startIndex, count, match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the
        ///	<see cref="AssemblyInfoCollection"/> that extends from the specified index to the last element.
        ///	</summary>
        ///	<param name="startIndex">The zero-based starting index of the search.</param>
        ///	<param name="match">The <seealso cref="Predicate{AssemblyInfo}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="AssemblyInfoCollection"/>.</exception>
        public int FindIndex(int startIndex, Predicate<AssemblyInfo> match)
        {
            return _innerList.FindIndex(startIndex, match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire
        ///	<see cref="AssemblyInfoCollection"/>.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{AssemblyInfo}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public int FindIndex(Predicate<AssemblyInfo> match)
        {
            return _innerList.FindIndex(match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire <see cref="AssemblyInfoCollection"/>.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{AssemblyInfo}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The last element that matches the conditions defined by the specified predicate, if found; otherwise, null.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public AssemblyInfo FindLast(Predicate<AssemblyInfo> match)
        {
            return _innerList.FindLast(match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the
        ///	<see cref="AssemblyInfoCollection"/> that contains the specified number of elements and ends at the specified index.
        ///	</summary>
        ///	<param name="startIndex">The zero-based starting index of the backward search.</param>
        ///	<param name="count">The number of elements in the section to search.</param>
        ///	<param name="match">The <seealso cref="Predicate{AssemblyInfo}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="AssemblyInfoCollection"/>. 
        ///	<para>-or- <paramref name="count"/> is less than 0.</para>
        ///	<para>-or- <paramref name="startIndex"/> and <paramref name="count"/> do not specify a valid section in the <see cref="AssemblyInfoCollection"/>.</para></exception>
        public int FindLastIndex(int startIndex, int count, Predicate<AssemblyInfo> match)
        {
            return _innerList.FindLastIndex(startIndex, count, match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the
        ///	<see cref="AssemblyInfoCollection"/> that extends from the first element to the specified index.
        ///	</summary>
        ///	<param name="startIndex">The zero-based starting index of the backward search.</param>
        ///	<param name="match">The <seealso cref="Predicate{AssemblyInfo}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="AssemblyInfoCollection"/>.</exception>
        public int FindLastIndex(int startIndex, Predicate<AssemblyInfo> match)
        {
            return _innerList.FindLastIndex(startIndex, match);
        }

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the entire
        ///	<see cref="AssemblyInfoCollection"/>.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{AssemblyInfo}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public int FindLastIndex(Predicate<AssemblyInfo> match)
        {
            return _innerList.FindLastIndex(match);
        }

        /// <summary>
        /// Performs the specified action on each element of the <see cref="AssemblyInfoCollection"/>.
        /// </summary>
        /// <param name="action">The <seealso cref="Action{AssemblyInfo}"/> delegate to perform on each element of the <see cref="AssemblyInfoCollection"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">An element in the collection has been modified.</exception>
        public void ForEach(Action<AssemblyInfo> action)
        {
            _innerList.ForEach(action);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="AssemblyInfoCollection"/>.
        /// </summary>
        /// <returns>A <seealso cref="IEnumerable{AssemblyInfo}"/> for the <see cref="AssemblyInfoCollection"/>.</returns>
        public IEnumerator<AssemblyInfo> GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Creates a shallow copy of a range of elements in the source <see cref="AssemblyInfoCollection"/>.
        ///	</summary>
        ///	<param name="index">The zero-based <see cref="AssemblyInfoCollection"/> index at which the range starts.</param>
        ///	<param name="count">The number of elements in the range.</param>
        ///	<returns>A shallow copy of a range of elements in the source <see cref="AssemblyInfoCollection"/>.</returns>
        ///	<exception cref="ArgumentOutOfRangeException">index is less than 0. -or- <paramref name="count"/> is less than 0.</exception>
        ///	<exception cref="ArgumentException">index and <paramref name="count"/> do not denote a valid range of elements in the <see cref="AssemblyInfoCollection"/>.</exception>
        public Collection<AssemblyInfo> GetRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="AssemblyInfoCollection"/> that starts at the
        ///	specified index and contains the specified number of elements.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyInfoCollection"/>. The value can be null for reference types.</param>
        ///	<param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        ///	<param name="count">The number of elements in the section to search.</param>
        ///	<returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="AssemblyInfoCollection"/> that starts at index and contains
        ///	<paramref name="count"/> number of elements, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentOutOfRangeException">index is outside the range of valid indexes for the <see cref="AssemblyInfoCollection"/>.
        ///	<para>-or- <paramref name="count"/> is less than 0.</para>
        ///	<para> -or- index and <paramref name="count"/> do not specify a valid section in the <see cref="AssemblyInfoCollection"/>.</para></exception>
        public int IndexOf(AssemblyInfo item, int index, int count)
        {
            return _innerList.IndexOf(item, index, count);
        }

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="AssemblyInfoCollection"/> that extends from the
        ///	specified index to the last element.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyInfoCollection"/>. The value can be null for reference types.</param>
        ///	<param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        ///	<returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="AssemblyInfoCollection"/> that extends from index to the last element, if found;
        ///	otherwise, –1.</returns>
        ///	<exception cref="ArgumentOutOfRangeException">index is outside the range of valid indexes for the <see cref="AssemblyInfoCollection"/>.</exception>
        public int IndexOf(AssemblyInfo item, int index)
        {
            return _innerList.IndexOf(item, index);
        }

        ///	<summary>
        ///	Determines the index of a specific item in the <see cref="AssemblyInfoCollection"/>.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyInfoCollection"/>.</param>
        ///	<returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(AssemblyInfo item)
        {
            return _innerList.IndexOf(item);
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Inserts an item to the <see cref="AssemblyInfoCollection"/> at the specified index.
        ///	</summary>
        ///	<param name="index">The zero-based index at which item should be inserted.</param>
        ///	<param name="item">The object to insert into the <see cref="AssemblyInfoCollection"/>.</param>
        ///	<exception cref="ArgumentOutOfRangeException">index is not a valid index in the <see cref="AssemblyInfoCollection"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="item"/> already exists in the <see cref="AssemblyInfoCollection"/>.
        /// <para>-or- another <seealso cref="AssemblyInfo"/> item in the <see cref="AssemblyInfoCollection"/> has the same full name.</para></exception>
        public void Insert(int index, AssemblyInfo item)
        {
            _innerList.Insert(index, item);
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the last occurrence within the entire <see cref="AssemblyInfoCollection"/>.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyInfoCollection"/>. The value can be null for reference types.</param>
        ///	<returns>The zero-based index of the last occurrence of item within the entire the <see cref="AssemblyInfoCollection"/>, if found; otherwise, –1.</returns>
        public int LastIndexOf(AssemblyInfo item)
        {
            return _innerList.LastIndexOf(item);
        }

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="AssemblyInfoCollection"/> that extends from the
        ///	first element to the specified index.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyInfoCollection"/>. The value can be null for reference types.</param>
        ///	<param name="index">The zero-based starting index of the backward search.</param>
        ///	<returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="AssemblyInfoCollection"/> that extends from the first element to index, if found;
        ///	otherwise, –1.</returns>
        ///	<exception cref="ArgumentOutOfRangeException">index is outside the range of valid indexes for the <see cref="AssemblyInfoCollection"/>.</exception>
        public int LastIndexOf(AssemblyInfo item, int index)
        {
            return _innerList.LastIndexOf(item, index);
        }

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="AssemblyInfoCollection"/> that contains the
        ///	specified number of elements and ends at the specified index.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="AssemblyInfoCollection"/>. The value can be null for reference types.</param>
        ///	<param name="index">The zero-based starting index of the backward search.</param>
        ///	<param name="count">The number of elements in the section to search.</param>
        ///	<returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="AssemblyInfoCollection"/> that contains <paramref name="count"/> number of
        ///	elements and ends at index, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentOutOfRangeException">index is outside the range of valid indexes for the <see cref="AssemblyInfoCollection"/>.
        ///	<para>-or- <paramref name="count"/> is less than 0.</para>
        ///	<para>-or- index and <paramref name="count"/> do not specify a valid section in the <see cref="AssemblyInfoCollection"/>.</para></exception>
        public int LastIndexOf(AssemblyInfo item, int index, int count)
        {
            return _innerList.LastIndexOf(item, index, count);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="AssemblyInfoCollection"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="AssemblyInfoCollection"/>.</param>
        /// <returns><c>true</c> if <paramref name="item"/> is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if <paramref name="item"/> was not found in the <see cref="AssemblyInfoCollection"/>.</returns>
        public bool Remove(AssemblyInfo item)
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
        ///	<param name="match">The <seealso cref="Predicate{AssemblyInfo}"/> delegate that defines the conditions of the elements to remove.</param>
        ///	<returns>The number of elements removed from the <see cref="AssemblyInfoCollection"/> .</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public int RemoveAll(Predicate<AssemblyInfo> match)
        {
            return _innerList.RemoveAll(match);
        }

        ///	<summary>
        ///	Removes the <see cref="AssemblyInfoCollection"/> item at the specified index.
        ///	</summary>
        ///	<param name="index">The zero-based index of the item to remove.</param>
        ///	<exception cref="ArgumentOutOfRangeException">index is not a valid index in the <see cref="AssemblyInfoCollection"/>.</exception>
        ///	<exception cref="NotSupportedException">The <see cref="AssemblyInfoCollection"/> is read-only.</exception>
        public void RemoveAt(int index)
        {
            _innerList.RemoveAt(index);
        }

        ///	<summary>
        ///	Removes a range of elements from the <see cref="AssemblyInfoCollection"/>.
        ///	</summary>
        ///	<param name="index">The zero-based starting index of the range of elements to remove.</param>
        ///	<param name="count">The number of elements to remove.</param>
        ///	<exception cref="ArgumentOutOfRangeException">index is less than 0. -or- <paramref name="count"/> is less than 0.</exception>
        ///	<exception cref="ArgumentException">index and <paramref name="count"/> do not denote a valid range of elements in the <see cref="AssemblyInfoCollection"/>.</exception>
        public void RemoveRange(int index, int count)
        {
            _innerList.RemoveRange(index, count);
        }

        ///	<summary>
        ///	Reverses the order of the elements in the specified range.
        ///	</summary>
        ///	<param name="index">The zero-based starting index of the range to reverse.</param>
        ///	<param name="count">The number of elements in the range to reverse.</param>
        ///	<exception cref="ArgumentOutOfRangeException">index is less than 0. -or- <paramref name="count"/> is less than 0.</exception>
        ///	<exception cref="ArgumentException">index and <paramref name="count"/> do not denote a valid range of elements in the <see cref="AssemblyInfoCollection"/>.</exception>
        public void Reverse(int index, int count)
        {
            _innerList.Reverse(index, count);
        }

        ///	<summary>
        ///	Reverses the order of the elements in the entire <see cref="AssemblyInfoCollection"/>.
        ///	</summary>
        public void Reverse()
        {
            _innerList.Reverse();
        }

        ///	<summary>
        ///	Sorts the elements in the entire <see cref="AssemblyInfoCollection"/> using the specified <seealso cref="Comparison{AssemblyInfo}"/>.
        ///	</summary>
        ///	<param name="comparison">The <seealso cref="Comparison{AssemblyInfo}"/> to use when comparing elements.</param>
        ///	<exception cref="ArgumentNullException">comparison is <c>null</c>.</exception>
        ///	<exception cref="ArgumentException">The implementation of comparison caused an error during the sort.</exception>
        public void Sort(Comparison<AssemblyInfo> comparison)
        {
            _innerList.Sort(comparison);
        }

        ///	<summary>
        ///	Sorts the elements in a range of elements in <see cref="AssemblyInfoCollection"/> using the specified comparer.
        ///	</summary>
        ///	<param name="index">The zero-based starting index of the range to sort.</param>
        ///	<param name="count">The length of the range to sort.</param>
        ///	<param name="comparer">The <seealso cref="IComparer{AssemblyInfo}"/> implementation to use when comparing elements, or null to use the default comparer 
        ///	<seealso cref="Comparer{AssemblyInfo}.Default"/>.</param>
        ///	<exception cref="ArgumentOutOfRangeException">index is less than 0. -or- <paramref name="count"/> is less than 0.</exception>
        ///	<exception cref="ArgumentException">index and <paramref name="count"/> do not specify a valid range in the <see cref="AssemblyInfoCollection"/>.
        ///	<para> -or- The implementation of comparer caused an error during the sort.</para></exception>
        ///	<exception cref="InvalidOperationException">comparer is <c>null</c>, and the default comparer <seealso cref="Comparer{AssemblyInfo}.Default"/> cannot find implementation of the
        ///	<seealso cref="IComparable{AssemblyInfo}"/> generic interface or the <seealso cref="IComparable"/> interface for type <seealso cref="AssemblyInfo"/>.</exception>
        public void Sort(int index, int count, IComparer<AssemblyInfo> comparer)
        {
            _innerList.Sort(index, count, comparer);
        }

        ///	<summary>
        ///	Sorts the elements in the entire <see cref="AssemblyInfoCollection"/> using the specified comparer.
        ///	</summary>
        ///	<param name="comparer">The <seealso cref="IComparer{AssemblyInfo}"/> implementation to use when comparing elements, or null to use the default comparer <seealso cref="Comparer{AssemblyInfo}.Default"/>.</param>
        ///	<exception cref="InvalidOperationException">comparer is <c>null</c>, and the default comparer <seealso cref="Comparer{AssemblyInfo}.Default"/> cannot find implementation of the
        ///	<seealso cref="IComparable{AssemblyInfo}"/> generic interface or the <seealso cref="IComparable"/> interface for type <seealso cref="AssemblyInfo"/>.</exception>
        ///	<exception cref="ArgumentException">The implementation of comparer caused an error during the sort.</exception>
        public void Sort(IComparer<AssemblyInfo> comparer)
        {
            _innerList.Sort(comparer);
        }

        ///	Sorts the elements in the entire <see cref="AssemblyInfoCollection"/> using the default comparer.
        ///	</summary>
        ///	<exception cref="InvalidOperationException">The default comparer <seealso cref="Comparer{AssemblyInfo}.Default"/> cannot find an implementation of the <seealso cref="IComparable{AssemblyInfo}"/>
        ///	generic interface or the <seealso cref="IComparable"/> interface for type <seealso cref="AssemblyInfo"/>.</exception>
        public void Sort()
        {
            _innerList.Sort();
        }

        ///	<summary>
        ///	Copies the elements of the <see cref="AssemblyInfoCollection"/> to a new array.
        ///	</summary>
        ///	<returns>An array containing copies of the elements of the <see cref="AssemblyInfoCollection"/>.</returns>
        public AssemblyInfo[] ToArray()
        {
            return _innerList.ToArray();
        }

        ///	<summary>
        ///	Determines whether every element in the <see cref="AssemblyInfoCollection"/> matches the conditions defined by the specified predicate.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{AssemblyInfo}"/> delegate that defines the conditions to check against the elements.</param>
        ///	<returns><c>true</c> if every element in the <see cref="AssemblyInfoCollection"/> matches the conditions defined by the specified predicate; otherwise, <c>false</c>.
        ///	If the list has no elements, the return value is <c>true</c>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        public bool TrueForAll(Predicate<AssemblyInfo> match)
        {
            return _innerList.TrueForAll(match);
        }
    }
}