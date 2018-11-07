using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeGeneratorCommon
{
    /// <summary>
    /// Interface for a <seealso cref="IProducerConsumerCollection{T}"/> that supports FIFO and LIFO operations as well as bounded capacity.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the collection.</typeparam>
    public interface IProducerConsumerQueue<T> : IProducerConsumerCollection<T>, ICollection<T>
    {
        /// <summary>
        /// Gets the bounded capacity of this <see cref="IProducerConsumerQueue{T}"/> instance.
        /// </summary>
        int BoundedCapacity { get; }

        /// <summary>
        /// Gets whether this <see cref="IProducerConsumerQueue{T}"/> has been marked as complete for adding.
        /// </summary>
        bool IsAddingCompleted { get; }

        /// <summary>
        /// Gets whether this <see cref="IProducerConsumerQueue{T}"/> has been marked as complete for adding and is empty.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="IProducerConsumerQueue{T}"/> is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets a value indicating whether the number of items has reached the <seealso cref="BoundedCapacity"/>.
        /// </summary>
        bool IsFull { get; }

        /// <summary>
        /// Provides a consuming <seealso cref="IEnumerable{T}"/> for items in the collection.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>An <seealso cref="IEnumerable{T}"/> that removes and returns items from the collection.</returns>
        IEnumerable<T> GetConsumingEnumerable(CancellationToken cancellationToken);

        /// <summary>
        /// Provides a consuming System.Collections.Generic.IEnumerator`1 for items in the collection.
        /// </summary>
        /// <returns>An <seealso cref="IEnumerable{T}"/> that removes and returns items from the collection.</returns>
        IEnumerable<T> GetConsumingEnumerable();

        /// <summary>
        /// Tries to add the specified item to the <see cref="IProducerConsumerQueue{T}"/> within the specified time period.
        /// </summary>
        /// <param name="item">The item to be added to the collection.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or System.Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
        /// <returns>true if the item could be added to the collection within the specified time; otherwise, false. If the item is a duplicate, and the underlying collection does not accept duplicate items, then an System.InvalidOperationException is thrown.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is a negative number other than -1, which represents an infinite time-out.</exception>
        bool TryAdd(T item, int millisecondsTimeout);

        /// <summary>
        /// Tries to add the specified item to the <see cref="IProducerConsumerQueue{T}"/> within the specified time period.
        /// </summary>
        /// <param name="item">The item to be added to the collection.</param>
        /// <param name="timeout">A System.TimeSpan that represents the number of milliseconds to wait, or a System.TimeSpan that represents -1 milliseconds to wait indefinitely.</param>
        /// <returns>true if the item could be added to the collection within the specified time span; otherwise, false.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout"/>  is a negative number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater than System.Int32.MaxValue.</exception>
        bool TryAdd(T item, TimeSpan timeout);

        /// <summary>
        /// Tries to add the specified item to the <see cref="IProducerConsumerQueue{T}"/> within the specified time period, while observing a cancellation token.
        /// </summary>
        /// <param name="item" The item to be added to the collection.></param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or System.Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>true if the item could be added to the collection within the specified time; otherwise, false. If the item is a duplicate, and the underlying collection does not accept duplicate items, then an System.InvalidOperationException is thrown.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is a negative number other than -1, which represents an infinite time-out.</exception>
        bool TryAdd(T item, int millisecondsTimeout, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        bool TryPeek(out T result, int millisecondsTimeout);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        bool TryPeek(out T result, TimeSpan timeout);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        bool TryPeek(out T result, int millisecondsTimeout, CancellationToken cancellationToken);

        /// <summary>
        /// Attempts to remove and return the last object of the <see cref="IProducerConsumerQueue{T}"/>.
        /// </summary>
        /// <param name="result">When this method returns, if the operation was successful, result contains the object removed. If no object was available to be removed, the value is unspecified.</param>
        /// <returns>true if an element was removed and returned from the top of the <see cref="IProducerConsumerQueue{T}"/> successfully; otherwise, false.</returns>
        bool TryPop(out T result);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        bool TryPop(out T result, int millisecondsTimeout);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        bool TryPop(out T result, TimeSpan timeout);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        bool TryPop(out T result, int millisecondsTimeout, CancellationToken cancellationToken);

        /// <summary>
        /// Tries to return an object from the beginning of the <see cref="IProducerConsumerQueue{T}"/> without removing it.
        /// </summary>
        /// <param name="result">When this method returns, result contains an object from the beginning of the <see cref="IProducerConsumerQueue{T}"/> or an unspecified value if the operation failed.</param>
        /// <returns>true if an object was returned successfully; otherwise, false.</returns>
        bool TryPeek(out T result);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool TryPush(T item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        bool TryPush(T item, int millisecondsTimeout);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        bool TryPush(T item, TimeSpan timeout);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        bool TryPush(T item, int millisecondsTimeout, CancellationToken cancellationToken);

        /// <summary>
        /// Tries to remove the first item from the <see cref="IProducerConsumerQueue{T}"/> in the specified time period while observing a cancellation token.
        /// </summary>
        /// <param name="item">The item to be removed from the collection.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or System.Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
        /// <param name="cancellationToken">A cancellation token to observe.</param>
        /// <returns>true if an item could be removed from the collection within the specified time; otherwise, false.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is a negative number other than -1, which represents an infinite time-out.</exception>
        bool TryTake(out T item, int millisecondsTimeout, CancellationToken cancellationToken);

        /// <summary>
        /// Tries to remove the first item from the <see cref="IProducerConsumerQueue{T}"/> in the specified time period.
        /// </summary>
        /// <param name="item">The item to be removed from the collection.</param>
        /// <param name="timeout">An object that represents the number of milliseconds to wait, or an object that represents -1 milliseconds to wait indefinitely.</param>
        /// <returns>true if an item could be removed from the collection within the specified time; otherwise, false.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout"/> is a negative number other than -1 milliseconds, which represents an infinite time-out.-or- timeout is greater than System.Int32.MaxValue.</exception>
        bool TryTake(out T item, TimeSpan timeout);

        /// <summary>
        /// Tries to remove the first item from the <see cref="IProducerConsumerQueue{T}"/> in the specified time period.
        /// </summary>
        /// <param name="item">The item to be removed from the collection.</param>
        /// <param name="millisecondsTimeout"> The number of milliseconds to wait, or System.Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
        /// <returns>true if an item could be removed from the collection within the specified time; otherwise, false.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is a negative number other than -1, which represents an infinite time-out.</exception>
        bool TryTake(out T item, int millisecondsTimeout);
    }
}
