using System;
using System.Collections;
using System.Collections.Generic;

namespace PSCodeGenerator
{
    public interface ICompatibleCollection<T> : ICollection<T>, ICollection
    {
        ///	<summary>
        ///	Performs the specified action on each element of the <see cref="ICompatibleList{T}"/>.
        ///	</summary>
        ///	<param name="action">The <seealso cref="Action{T}"/> delegate to perform on each element of the <see cref="ICompatibleList{T}"/>.</param>
        ///	<exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        ///	<exception cref="InvalidOperationException">An element in the collection has been modified.</exception>
        void ForEach(Action<T> action);
    }
}