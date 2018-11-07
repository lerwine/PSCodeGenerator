using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PSCodeGenerator
{
    /// <summary>
    /// Represents a collection of objects that can be individually accessed by index, compatible with PowerShell operations.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public interface ICompatibleList<T> : IList<T>, ICollection<T>, IList, ICompatibleCollection<T>
        where T : IEquatable<T>
    {
        ///	<summary>
        ///	Adds the elements of the specified collection to the end of the <see cref="ICompatibleList{T}"/>.
        ///	</summary>
        ///	<param name="collection">The collection whose elements should be added to the end of the <see cref="ICompatibleList{T}"/>.</param>
        ///	<exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
        void AddRange(IEnumerable<T> collection);

        ///	<summary>
        ///	Determines whether the <see cref="ICompatibleList{T}"/> contains elements that match the conditions defined by the specified predicate.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{T}"/> delegate that defines the conditions of the elements to search for.</param>
        ///	<returns><c>true</c> if the <see cref="ICompatibleList{T}"/> contains one or more elements that match the conditions defined by the specified predicate;
        ///	otherwise, <c>false</c>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        bool Exists(Predicate<T> match);

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire <see cref="ICompatibleList{T}"/>.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <typeparamref name="T"/>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        T Find(Predicate<T> match);

        ///	<summary>
        ///	Retrieves all the elements that match the conditions defined by the specified predicate.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{T}"/> delegate that defines the conditions of the elements to search for.</param>
        ///	<returns>A <seealso cref="Collection{T}"/> containing all the elements that match the conditions defined by the specified predicate, if found;
        ///	otherwise, an empty <seealso cref="Collection{T}"/>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        Collection<T> FindAll(Predicate<T> match);

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the
        ///	<see cref="ICompatibleList{T}"/> that starts at the specified index and contains the specified number of elements.
        ///	</summary>
        ///	<param name="startIndex">The zero-based starting index of the search.</param>
        ///	<param name="count">The number of elements in the section to search.</param>
        ///	<param name="match">The <seealso cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="ICompatibleList{T}"/>.
        ///	<para>-or- <paramref name="count"/> is less than 0.</para>
        ///	<para>-or- <paramref name="startIndex"/> and <paramref name="count"/> do not specify a valid section in the <see cref="ICompatibleList{T}"/>.</para></exception>
        int FindIndex(int startIndex, int count, Predicate<T> match);

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the
        ///	<see cref="ICompatibleList{T}"/> that extends from the specified index to the last element.
        ///	</summary>
        ///	<param name="startIndex">The zero-based starting index of the search.</param>
        ///	<param name="match">The <seealso cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="ICompatibleList{T}"/>.</exception>
        int FindIndex(int startIndex, Predicate<T> match);

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire
        ///	<see cref="ICompatibleList{T}"/>.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        int FindIndex(Predicate<T> match);

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire <see cref="ICompatibleList{T}"/>.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <typeparamref name="T"/>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        T FindLast(Predicate<T> match);

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the
        ///	<see cref="ICompatibleList{T}"/> that contains the specified number of elements and ends at the specified index.
        ///	</summary>
        ///	<param name="startIndex">The zero-based starting index of the backward search.</param>
        ///	<param name="count">The number of elements in the section to search.</param>
        ///	<param name="match">The <seealso cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="ICompatibleList{T}"/>. 
        ///	<para>-or- <paramref name="count"/> is less than 0.</para>
        ///	<para>-or- <paramref name="startIndex"/> and <paramref name="count"/> do not specify a valid section in the <see cref="ICompatibleList{T}"/>.</para></exception>
        int FindLastIndex(int startIndex, int count, Predicate<T> match);

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the
        ///	<see cref="ICompatibleList{T}"/> that extends from the first element to the specified index.
        ///	</summary>
        ///	<param name="startIndex">The zero-based starting index of the backward search.</param>
        ///	<param name="match">The <seealso cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        ///	<exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="ICompatibleList{T}"/>.</exception>
        int FindLastIndex(int startIndex, Predicate<T> match);

        ///	<summary>
        ///	Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the entire
        ///	<see cref="ICompatibleList{T}"/>.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        ///	<returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match"/>, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        int FindLastIndex(Predicate<T> match);

        ///	<summary>
        ///	Creates a shallow copy of a range of elements in the source <see cref="ICompatibleList{T}"/>.
        ///	</summary>
        ///	<param name="index">The zero-based <see cref="ICompatibleList{T}"/> index at which the range starts.</param>
        ///	<param name="count">The number of elements in the range.</param>
        ///	<returns>A shallow copy of a range of elements in the source <see cref="ICompatibleList{T}"/>.</returns>
        ///	<exception cref="ArgumentOutOfRangeException">index is less than 0. -or- <paramref name="count"/> is less than 0.</exception>
        ///	<exception cref="ArgumentException">index and <paramref name="count"/> do not denote a valid range of elements in the <see cref="ICompatibleList{T}"/>.</exception>
        Collection<T> GetRange(int index, int count);

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="ICompatibleList{T}"/> that starts at the
        ///	specified index and contains the specified number of elements.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="ICompatibleList{T}"/>. The value can be null for reference types.</param>
        ///	<param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        ///	<param name="count">The number of elements in the section to search.</param>
        ///	<returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="ICompatibleList{T}"/> that starts at index and contains
        ///	<paramref name="count"/> number of elements, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentOutOfRangeException">index is outside the range of valid indexes for the <see cref="ICompatibleList{T}"/>.
        ///	<para>-or- <paramref name="count"/> is less than 0.</para>
        ///	<para> -or- index and <paramref name="count"/> do not specify a valid section in the <see cref="ICompatibleList{T}"/>.</para></exception>
        int IndexOf(T item, int index, int count);

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="ICompatibleList{T}"/> that extends from the
        ///	specified index to the last element.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="ICompatibleList{T}"/>. The value can be null for reference types.</param>
        ///	<param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        ///	<returns>The zero-based index of the first occurrence of item within the range of elements in the <see cref="ICompatibleList{T}"/> that extends from index to the last element, if found;
        ///	otherwise, –1.</returns>
        ///	<exception cref="ArgumentOutOfRangeException">index is outside the range of valid indexes for the <see cref="ICompatibleList{T}"/>.</exception>
        int IndexOf(T item, int index);
        
        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the last occurrence within the entire <see cref="ICompatibleList{T}"/>.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="ICompatibleList{T}"/>. The value can be null for reference types.</param>
        ///	<returns>The zero-based index of the last occurrence of item within the entire the <see cref="ICompatibleList{T}"/>, if found; otherwise, –1.</returns>
        int LastIndexOf(T item);

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="ICompatibleList{T}"/> that extends from the
        ///	first element to the specified index.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="ICompatibleList{T}"/>. The value can be null for reference types.</param>
        ///	<param name="index">The zero-based starting index of the backward search.</param>
        ///	<returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="ICompatibleList{T}"/> that extends from the first element to index, if found;
        ///	otherwise, –1.</returns>
        ///	<exception cref="ArgumentOutOfRangeException">index is outside the range of valid indexes for the <see cref="ICompatibleList{T}"/>.</exception>
        int LastIndexOf(T item, int index);

        ///	<summary>
        ///	Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="ICompatibleList{T}"/> that contains the
        ///	specified number of elements and ends at the specified index.
        ///	</summary>
        ///	<param name="item">The object to locate in the <see cref="ICompatibleList{T}"/>. The value can be null for reference types.</param>
        ///	<param name="index">The zero-based starting index of the backward search.</param>
        ///	<param name="count">The number of elements in the section to search.</param>
        ///	<returns>The zero-based index of the last occurrence of item within the range of elements in the <see cref="ICompatibleList{T}"/> that contains <paramref name="count"/> number of
        ///	elements and ends at index, if found; otherwise, –1.</returns>
        ///	<exception cref="ArgumentOutOfRangeException">index is outside the range of valid indexes for the <see cref="ICompatibleList{T}"/>.
        ///	<para>-or- <paramref name="count"/> is less than 0.</para>
        ///	<para>-or- index and <paramref name="count"/> do not specify a valid section in the <see cref="ICompatibleList{T}"/>.</para></exception>
        int LastIndexOf(T item, int index, int count);
        
        ///	<summary>
        ///	Removes all the elements that match the conditions defined by the specified predicate.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{T}"/> delegate that defines the conditions of the elements to remove.</param>
        ///	<returns>The number of elements removed from the <see cref="ICompatibleList{T}"/> .</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        int RemoveAll(Predicate<T> match);
        
        ///	<summary>
        ///	Removes a range of elements from the <see cref="ICompatibleList{T}"/>.
        ///	</summary>
        ///	<param name="index">The zero-based starting index of the range of elements to remove.</param>
        ///	<param name="count">The number of elements to remove.</param>
        ///	<exception cref="ArgumentOutOfRangeException">index is less than 0. -or- <paramref name="count"/> is less than 0.</exception>
        ///	<exception cref="ArgumentException">index and <paramref name="count"/> do not denote a valid range of elements in the <see cref="ICompatibleList{T}"/>.</exception>
        void RemoveRange(int index, int count);

        ///	<summary>
        ///	Reverses the order of the elements in the specified range.
        ///	</summary>
        ///	<param name="index">The zero-based starting index of the range to reverse.</param>
        ///	<param name="count">The number of elements in the range to reverse.</param>
        ///	<exception cref="ArgumentOutOfRangeException">index is less than 0. -or- <paramref name="count"/> is less than 0.</exception>
        ///	<exception cref="ArgumentException">index and <paramref name="count"/> do not denote a valid range of elements in the <see cref="ICompatibleList{T}"/>.</exception>
        void Reverse(int index, int count);

        ///	<summary>
        ///	Reverses the order of the elements in the entire <see cref="ICompatibleList{T}"/>.
        ///	</summary>
        void Reverse();

        ///	<summary>
        ///	Sorts the elements in the entire <see cref="ICompatibleList{T}"/> using the specified <seealso cref="Comparison{T}"/>.
        ///	</summary>
        ///	<param name="comparison">The <seealso cref="Comparison{T}"/> to use when comparing elements.</param>
        ///	<exception cref="ArgumentNullException">comparison is <c>null</c>.</exception>
        ///	<exception cref="ArgumentException">The implementation of comparison caused an error during the sort.</exception>
        void Sort(Comparison<T> comparison);

        ///	<summary>
        ///	Sorts the elements in a range of elements in <see cref="ICompatibleList{T}"/> using the specified comparer.
        ///	</summary>
        ///	<param name="index">The zero-based starting index of the range to sort.</param>
        ///	<param name="count">The length of the range to sort.</param>
        ///	<param name="comparer">The <seealso cref="IComparer{T}"/> implementation to use when comparing elements, or null to use the default comparer 
        ///	<seealso cref="Comparer{T}.Default"/>.</param>
        ///	<exception cref="ArgumentOutOfRangeException">index is less than 0. -or- <paramref name="count"/> is less than 0.</exception>
        ///	<exception cref="ArgumentException">index and <paramref name="count"/> do not specify a valid range in the <see cref="ICompatibleList{T}"/>.
        ///	<para> -or- The implementation of comparer caused an error during the sort.</para></exception>
        ///	<exception cref="InvalidOperationException">comparer is <c>null</c>, and the default comparer <seealso cref="Comparer{T}.Default"/> cannot find implementation of the
        ///	<seealso cref="IComparable{T}"/> generic interface or the <seealso cref="IComparable"/> interface for type <typeparamref name="T"/>.</exception>
        void Sort(int index, int count, IComparer<T> comparer);

        ///	<summary>
        ///	Sorts the elements in the entire <see cref="ICompatibleList{T}"/> using the specified comparer.
        ///	</summary>
        ///	<param name="comparer">The <seealso cref="IComparer{T}"/> implementation to use when comparing elements, or null to use the default comparer <seealso cref="Comparer{T}.Default"/>.</param>
        ///	<exception cref="InvalidOperationException">comparer is <c>null</c>, and the default comparer <seealso cref="Comparer{T}.Default"/> cannot find implementation of the
        ///	<seealso cref="IComparable{T}"/> generic interface or the <seealso cref="IComparable"/> interface for type <typeparamref name="T"/>.</exception>
        ///	<exception cref="ArgumentException">The implementation of comparer caused an error during the sort.</exception>
        void Sort(IComparer<T> comparer);

        ///	<summary>
        ///	Sorts the elements in the entire <see cref="ICompatibleList{T}"/> using the default comparer.
        ///	</summary>
        ///	<exception cref="InvalidOperationException">The default comparer <seealso cref="Comparer{T}.Default"/> cannot find an implementation of the <seealso cref="IComparable{T}"/>
        ///	generic interface or the <seealso cref="IComparable"/> interface for type <typeparamref name="T"/>.</exception>
        void Sort();

        ///	<summary>
        ///	Copies the elements of the <see cref="ICompatibleList{T}"/> to a new array.
        ///	</summary>
        ///	<returns>An array containing copies of the elements of the <see cref="ICompatibleList{T}"/>.</returns>
        T[] ToArray();
        
        ///	<summary>
        ///	Determines whether every element in the <see cref="ICompatibleList{T}"/> matches the conditions defined by the specified predicate.
        ///	</summary>
        ///	<param name="match">The <seealso cref="Predicate{T}"/> delegate that defines the conditions to check against the elements.</param>
        ///	<returns><c>true</c> if every element in the <see cref="ICompatibleList{T}"/> matches the conditions defined by the specified predicate; otherwise, <c>false</c>.
        ///	If the list has no elements, the return value is <c>true</c>.</returns>
        ///	<exception cref="ArgumentNullException"><paramref name="match"/> is <c>null</c>.</exception>
        bool TrueForAll(Predicate<T> match);
    }
}