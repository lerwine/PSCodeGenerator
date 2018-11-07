using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace CodeGeneratorCommon
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProducerConsumerQueue<T> : IProducerConsumerQueue<T>, INotifyCollectionChanged, IDisposable
    {
        private IEqualityComparer<T> _equalityComparer;
        private Func<T, T, bool> _areEqual;
        private QueueItem _firstElement = null;
        private QueueItem _lastElement = null;
        private ManualResetEvent _notEmptyEvent = new ManualResetEvent(false);
        private ManualResetEvent _notFullEvent = new ManualResetEvent(true);
        private bool _isDisposed = false;

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="ProducerConsumerQueue{T}"/>.
        /// </summary>
        protected object SyncRoot { get; } = new object();

        /// <summary>
        /// Occurs when the items are added or removed from the <see cref="ProducerConsumerQueue{T}"/>.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Maximum number of items that the <see cref="ProducerConsumerQueue{T}"/> can contain.
        /// </summary>
        public int BoundedCapacity { get; }

        /// <summary>
        /// Gets the number of elements contained in the System.Collections.ICollection.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets a value indicating whether any more items can be added to the <see cref="ProducerConsumerQueue{T}"/>.
        /// </summary>
        public bool IsAddingCompleted { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ProducerConsumerQueue{T}"/> is empty and <seealso cref="IsAddingCompleted"/> is true.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ProducerConsumerQueue{T}"/> has no items.
        /// </summary>
        public bool IsEmpty => Count == 0;

        /// <summary>
        /// Gets a value indicating whehter the number of items in the <see cref="ProducerConsumerQueue{T}"/> has reached the <seealso cref="BoundedCapacity"/>.
        /// </summary>
        public bool IsFull => Count == BoundedCapacity;

        object ICollection.SyncRoot => SyncRoot;

        bool ICollection.IsSynchronized => true;

        bool ICollection<T>.IsReadOnly => false;

        /// <summary>
        /// Initializes a new <see cref="ProducerConsumerQueue{T}"/> with a bounded capacity and an item equality comparer.
        /// </summary>
        /// <param name="boundedCapacity">The maximum number if items that the <see cref="ProducerConsumerQueue{T}"/> can contain.</param>
        /// <param name="equalityComparer">An object that compares items for equality.</param>
        public ProducerConsumerQueue(int boundedCapacity, IEqualityComparer<T> equalityComparer)
        {
            _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
            BoundedCapacity = (boundedCapacity > 1) ? boundedCapacity : 1;
            Type t = typeof(T);
            if (t.IsValueType && !(t.IsGenericType && t.GetGenericTypeDefinition().AssemblyQualifiedName == typeof(Nullable<>).AssemblyQualifiedName))
                _areEqual = new Func<T, T, bool>((x, y) => _equalityComparer.Equals(x, y));
            else
                _areEqual = new Func<T, T, bool>((x, y) => ((object)x == null) ? (object)y == null : ((object)y != null && _equalityComparer.Equals(x, y)));
        }

        /// <summary>
        /// Initializes a new <see cref="ProducerConsumerQueue{T}"/>.
        /// </summary>
        /// <param name="boundedCapacity">The maximum number if items that the <see cref="ProducerConsumerQueue{T}"/> can contain.</param>
        public ProducerConsumerQueue(int boundedCapacity) : this(boundedCapacity, null) { }

        /// <summary>
        /// Sets <seealso cref="IsAddingCompleted"/> to true, indicating that no more items will be added to the <see cref="ProducerConsumerQueue{T}"/>.
        /// </summary>
        public void SetAddingCompleted()
        {
            Monitor.Enter(SyncRoot);
            try
            {
                IsAddingCompleted = true;
                if (IsEmpty)
                    IsCompleted = true;
            }
            finally { Monitor.Exit(SyncRoot); }
        }
        
        void ICollection<T>.Add(T item)
        {
            if (!TryAdd(item))
                throw new InvalidOperationException();
        }

        void ICollection<T>.Clear() => QueueItem.Clear(this);

        bool ICollection<T>.Contains(T item) => QueueItem.Contains(this, item);

        /// <summary>
        /// Copies the elements of the <see cref="ProducerConsumerQueue{T}"/> to an array, starting at a specified index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(T[] array, int index) => QueueItem.CopyTo(this, array, index);

        void ICollection.CopyTo(Array array, int index) => QueueItem.CopyTo(this, array, index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IEnumerable<T> GetConsumingEnumerable(CancellationToken cancellationToken) => new ConsumingEnumerable(this, cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetConsumingEnumerable() => new ConsumingEnumerable(this, null);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() => new QueueItem.ConsumingEnumerator(this, null);

        IEnumerator IEnumerable.GetEnumerator() => new QueueItem.ConsumingEnumerator(this, null);

        bool ICollection<T>.Remove(T item) => QueueItem.Remove(this, item);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T[] ToArray() => QueueItem.ToArray(this);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public bool TryAdd(T item, int millisecondsTimeout) => !IsAddingCompleted && _notFullEvent.WaitOne(millisecondsTimeout) && QueueItem.TryAdd(this, item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool TryAdd(T item, TimeSpan timeout) => !IsAddingCompleted && _notFullEvent.WaitOne(timeout) && QueueItem.TryAdd(this, item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public bool TryAdd(T item, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            if (IsAddingCompleted || cancellationToken.IsCancellationRequested)
                return false;
            while (millisecondsTimeout > 125 && !_notFullEvent.WaitOne(125))
            {
                if (cancellationToken.IsCancellationRequested || IsAddingCompleted)
                    return false;
                millisecondsTimeout -= 125;
            }
            return _notFullEvent.WaitOne(millisecondsTimeout) && !cancellationToken.IsCancellationRequested && QueueItem.TryAdd(this, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryAdd(T item) => QueueItem.TryAdd(this, item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public bool TryPeek(out T result, int millisecondsTimeout)
        {
            if (!IsCompleted && _notEmptyEvent.WaitOne(millisecondsTimeout))
                return QueueItem.TryPeek(this, out result);
            result = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool TryPeek(out T result, TimeSpan timeout)
        {
            if (!IsCompleted && _notEmptyEvent.WaitOne(timeout))
                return QueueItem.TryPeek(this, out result);
            result = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public bool TryPeek(out T result, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            if (!(cancellationToken.IsCancellationRequested || IsCompleted))
            {
                while (millisecondsTimeout > 125 && !_notEmptyEvent.WaitOne(125))
                {
                    if (cancellationToken.IsCancellationRequested || IsCompleted)
                    {
                        result = default(T);
                        return false;
                    }
                    millisecondsTimeout -= 125;
                }
                if (_notFullEvent.WaitOne(millisecondsTimeout) && !(cancellationToken.IsCancellationRequested || IsCompleted))
                    return QueueItem.TryPeek(this, out result);
            }
            result = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryPeek(out T result) => QueueItem.TryPeek(this, out result);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryPop(out T result) => QueueItem.TryPop(this, out result);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public bool TryPop(out T result, int millisecondsTimeout)
        {
            if (!IsCompleted && _notEmptyEvent.WaitOne(millisecondsTimeout))
                return QueueItem.TryPop(this, out result);
            result = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool TryPop(out T result, TimeSpan timeout)
        {
            if (!IsCompleted && _notEmptyEvent.WaitOne(timeout))
                return QueueItem.TryPop(this, out result);
            result = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public bool TryPop(out T result, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            if (!(cancellationToken.IsCancellationRequested || IsCompleted))
            {
                while (millisecondsTimeout > 125 && !_notEmptyEvent.WaitOne(125))
                {
                    if (cancellationToken.IsCancellationRequested || IsCompleted)
                    {
                        result = default(T);
                        return false;
                    }
                    millisecondsTimeout -= 125;
                }
                if (_notFullEvent.WaitOne(millisecondsTimeout) && !(cancellationToken.IsCancellationRequested || IsCompleted))
                    return QueueItem.TryPop(this, out result);
            }
            result = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryPush(T item) => QueueItem.TryPush(this, item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public bool TryPush(T item, int millisecondsTimeout) => !IsAddingCompleted && _notFullEvent.WaitOne(millisecondsTimeout) && QueueItem.TryPush(this, item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool TryPush(T item, TimeSpan timeout) => !IsAddingCompleted && _notFullEvent.WaitOne(timeout) && QueueItem.TryPush(this, item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public bool TryPush(T item, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested || IsAddingCompleted)
                return false;
            while (millisecondsTimeout > 125 && !_notFullEvent.WaitOne(125))
            {
                if (cancellationToken.IsCancellationRequested || IsAddingCompleted)
                    return false;
                millisecondsTimeout -= 125;
            }
            return _notFullEvent.WaitOne(millisecondsTimeout) && cancellationToken.IsCancellationRequested && QueueItem.TryPush(this, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public bool TryTake(out T item, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            if (!(cancellationToken.IsCancellationRequested || IsCompleted))
            {
                while (millisecondsTimeout > 125 && !_notEmptyEvent.WaitOne(125))
                {
                    if (cancellationToken.IsCancellationRequested || IsCompleted)
                    {
                        item = default(T);
                        return false;
                    }
                    millisecondsTimeout -= 125;
                }
                if (_notFullEvent.WaitOne(millisecondsTimeout) && !(cancellationToken.IsCancellationRequested || IsCompleted))
                    return QueueItem.TryTake(this, out item);
            }
            item = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool TryTake(out T item, TimeSpan timeout)
        {
            if (!IsCompleted && _notEmptyEvent.WaitOne(timeout))
                return QueueItem.TryTake(this, out item);
            item = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public bool TryTake(out T item, int millisecondsTimeout)
        {
            if (!IsCompleted && _notEmptyEvent.WaitOne(millisecondsTimeout))
                return QueueItem.TryTake(this, out item);
            item = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryTake(out T item) => QueueItem.TryTake(this, out item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args) { }

        private void RaiseItemAdded(int index, T item)
        {
            NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
            try { OnCollectionChanged(args); }
            finally { CollectionChanged?.Invoke(this, args); }
        }

        private void RaiseItemRemoved(int index, T item)
        {
            NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
            try { OnCollectionChanged(args); }
            finally { CollectionChanged?.Invoke(this, args); }
        }

        private void RaiseCollectionReset(params T[] item)
        {
            NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, 0);
            try { OnCollectionChanged(args); }
            finally { CollectionChanged?.Invoke(this, args); }
        }

        class ConsumingEnumerable : IEnumerable<T>
        {
            ProducerConsumerQueue<T> _target;
            private CancellationToken? _cancellationToken;
            internal ConsumingEnumerable(ProducerConsumerQueue<T> target, CancellationToken? cancellationToken)
            {
                _target = target;
                _cancellationToken = cancellationToken;
            }

            public IEnumerator<T> GetEnumerator() => new QueueItem.ConsumingEnumerator(_target, _cancellationToken);

            IEnumerator IEnumerable.GetEnumerator() => new QueueItem.ConsumingEnumerator(_target, _cancellationToken);
        }

        class QueueItem
        {
            private QueueItem(T value) { _value = value; }
            private QueueItem _precedingElement = null;
            private QueueItem _followingElement = null;
            private T _value;
        
            internal static bool TryAdd(ProducerConsumerQueue<T> target, T item)
            {
                int index;
                Monitor.Enter(target.SyncRoot);
                try
                {
                    if (target.IsAddingCompleted || target.Count == target.BoundedCapacity)
                        return false;
                    index = target.Count;
                    target.Count++;
                    if (target._firstElement == null)
                    {
                        target._firstElement = target._lastElement = new QueueItem(item);
                        target._notEmptyEvent.Set();
                    }
                    else
                        target._lastElement = target._lastElement._followingElement = new QueueItem(item) { _precedingElement = target._lastElement };
                    if (target.Count == target.BoundedCapacity)
                        target._notFullEvent.Reset();
                }
                finally { Monitor.Exit(target.SyncRoot); }
                target.RaiseItemAdded(index, item);
                return true;
            }

            internal static bool TryPeek(ProducerConsumerQueue<T> target, out T item)
            {
                Monitor.Enter(target.SyncRoot);
                try
                {
                    if (target._firstElement == null)
                    {
                        item = default(T);
                        return false;
                    }
                    item = target._firstElement._value;
                }
                finally { Monitor.Exit(target.SyncRoot); }
                return true;
            }

            internal static bool TryPop(ProducerConsumerQueue<T> target, out T item)
            {
                int index;
                Monitor.Enter(target.SyncRoot);
                try
                {
                    if (target._lastElement == null)
                    {
                        item = default(T);
                        return false;
                    }
                    item = target._lastElement._value;
                    index = target.Count;
                    bool wasFull = target.Count == target.BoundedCapacity;
                    target.Count--;
                    if (target._lastElement._precedingElement == null)
                    {
                        target._firstElement = target._lastElement = null;
                        target._notEmptyEvent.Reset();
                        if (target.IsAddingCompleted)
                            target.IsCompleted = true;
                    }
                    else
                        (target._lastElement = target._lastElement._precedingElement)._followingElement = null;
                    if (wasFull)
                        target._notFullEvent.Set();
                }
                finally { Monitor.Exit(target.SyncRoot); }
                target.RaiseItemRemoved(index, item);
                return true;
            }

            internal static bool TryPush(ProducerConsumerQueue<T> target, T item)
            {
                Monitor.Enter(target.SyncRoot);
                try
                {
                    if (target.IsAddingCompleted || target.Count == target.BoundedCapacity)
                        return false;
                    target.Count++;
                    if (target._firstElement == null)
                    {
                        target._firstElement = target._lastElement = new QueueItem(item);
                        target._notEmptyEvent.Set();
                    }
                    else
                        target._firstElement = target._firstElement._precedingElement = new QueueItem(item) { _followingElement = target._firstElement };
                    if (target.Count == target.BoundedCapacity)
                        target._notFullEvent.Reset();
                }
                finally { Monitor.Exit(target.SyncRoot); }
                target.RaiseItemAdded(0, item);
                return true;
            }

            internal static bool TryTake(ProducerConsumerQueue<T> target, out T item)
            {
                Monitor.Enter(target.SyncRoot);
                try
                {
                    if (target._firstElement == null)
                    {
                        item = default(T);
                        return false;
                    }
                    item = target._firstElement._value;
                    bool wasFull = target.Count == target.BoundedCapacity;
                    target.Count--;
                    if (target._firstElement._followingElement == null)
                    {
                        target._firstElement = target._lastElement = null;
                        target._notEmptyEvent.Reset();
                        if (target.IsAddingCompleted)
                            target.IsCompleted = true;
                    }
                    else
                        (target._firstElement = target._firstElement._followingElement)._precedingElement = null;
                    if (wasFull)
                        target._notFullEvent.Set();
                }
                finally { Monitor.Exit(target.SyncRoot); }
                target.RaiseItemRemoved(0, item);
                return true;
            }

            internal static void Clear(ProducerConsumerQueue<T> target)
            {
                List<T> removed = new List<T>();
                Monitor.Enter(target.SyncRoot);
                try
                {
                    if (target._firstElement == null)
                        return;
                    for (QueueItem item = target._firstElement; item != null; item = item._followingElement)
                        removed.Add(item._value);
                    bool wasFull = target.Count == target.BoundedCapacity;
                    target.Count = 0;
                    target._firstElement = target._lastElement = null;
                    target._notEmptyEvent.Reset();
                    if (target.IsAddingCompleted)
                        target.IsCompleted = true;
                    if (wasFull)
                        target._notFullEvent.Set();
                }
                finally { Monitor.Exit(target.SyncRoot); }
                if (removed.Count > 0)
                    target.RaiseCollectionReset(removed.ToArray());
            }

            internal static bool Contains(ProducerConsumerQueue<T> target, T item)
            {
                Monitor.Enter(target.SyncRoot);
                try
                {
                    for (QueueItem q = target._firstElement; q != null; q = q._followingElement)
                    {
                        if (target._areEqual(q._value, item))
                            return true;
                    }
                }
                finally { Monitor.Exit(target.SyncRoot); }
                return false;
            }

            internal static void CopyTo(ProducerConsumerQueue<T> target, Array array, int index)
            {
                List<T> list = new List<T>();
                Monitor.Enter(target.SyncRoot);
                try
                {
                    for (QueueItem item = target._firstElement; item != null; item = item._followingElement)
                        list.Add(item._value);
                }
                finally { Monitor.Exit(target.SyncRoot); }
                list.ToArray().CopyTo(array, index);
            }

            internal static bool Remove(ProducerConsumerQueue<T> target, T item)
            {
                bool collectionChanged = false;
                Monitor.Enter(target.SyncRoot);
                try
                {
                    for (QueueItem q = target._firstElement; q != null; q = q._followingElement)
                    {
                        if (target._areEqual(q._value, item))
                        {
                            item = q._value;
                            target.Count--;
                            collectionChanged = true;
                            bool wasFull = target.Count == target.BoundedCapacity;
                            if (q._precedingElement == null)
                            {
                                if (q._followingElement == null)
                                {
                                    target._firstElement = target._lastElement = null;
                                    target._notEmptyEvent.Reset();
                                }
                                else
                                    (target._firstElement = q._followingElement)._precedingElement = null;
                            }
                            else if (q._followingElement == null)
                                (target._lastElement = q._precedingElement)._followingElement = null;
                            else
                            {
                                q._precedingElement._followingElement = q._followingElement;
                                q._followingElement._precedingElement = q._precedingElement;
                            }
                            break;
                        }
                    }
                }
                finally { Monitor.Exit(target.SyncRoot); }
                if (collectionChanged)
                    target.RaiseItemRemoved(0, item);
                return collectionChanged;
            }

            internal static T[] ToArray(ProducerConsumerQueue<T> target)
            {
                List<T> removed = new List<T>();
                Monitor.Enter(target.SyncRoot);
                try
                {
                    if (target._firstElement == null)
                        return new T[0];
                    for (QueueItem item = target._firstElement; item != null; item = item._followingElement)
                        removed.Add(item._value);
                    bool wasFull = target.Count == target.BoundedCapacity;
                    target.Count = 0;
                    target._firstElement = target._lastElement = null;
                    target._notEmptyEvent.Reset();
                    if (target.IsAddingCompleted)
                        target.IsCompleted = true;
                    if (wasFull)
                        target._notFullEvent.Set();
                }
                finally { Monitor.Exit(target.SyncRoot); }
                if (removed.Count > 0)
                    target.RaiseCollectionReset(removed.ToArray());
                return removed.ToArray();
            }

            internal class ConsumingEnumerator : IEnumerator<T>
            {
                private T _current;
                ProducerConsumerQueue<T> _target;
                private bool _collectionUnchanged = true;
                private bool _isEnumerating = false;
                private bool _isTaking = false;
                private CancellationToken? _cancellationToken;
                internal ConsumingEnumerator(ProducerConsumerQueue<T> target, CancellationToken? cancellationToken)
                {
                    _target = target;
                    _cancellationToken = cancellationToken;
                    target.CollectionChanged += Target_CollectionChanged;
                }

                private void Target_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
                {
                    if (_isTaking && e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null && e.OldItems.Count == 1 && _target._areEqual(_current, (T)(e.OldItems[0])))
                        _isTaking = false;
                    else
                        _collectionUnchanged = false;
                }

                public T Current
                {
                    get
                    {
                        Monitor.Enter(_target.SyncRoot);
                        try
                        {
                            if (_collectionUnchanged)
                                return _current;
                        }
                        finally { Monitor.Exit(_target.SyncRoot); }
                        throw new InvalidOperationException("Collection has changed");
                    }
                }

                object IEnumerator.Current => Current;

                public bool MoveNext()
                {
                    Monitor.Enter(_target.SyncRoot);
                    T item;

                    try
                    {
                        if (!_isEnumerating)
                            return false;
                        _isEnumerating = !(_cancellationToken.HasValue && _cancellationToken.Value.IsCancellationRequested);
                        if (_isEnumerating)
                        {
                            if (!_collectionUnchanged || _isTaking)
                                throw new InvalidOperationException("Collection has changed");
                            _isEnumerating = _target._firstElement != null;
                            if (!_isEnumerating)
                                return false;
                        }
                        else
                            return false;

                        _current = item = _target._firstElement._value;
                        _isTaking = true;
                        bool wasFull = _target.Count == _target.BoundedCapacity;
                        _target.Count--;
                        if (_target._firstElement._followingElement == null)
                        {
                            _target._firstElement = _target._lastElement = null;
                            _target._notEmptyEvent.Reset();
                            if (_target.IsAddingCompleted)
                                _target.IsCompleted = true;
                        }
                        else
                            (_target._firstElement = _target._firstElement._followingElement)._precedingElement = null;
                        if (wasFull)
                            _target._notFullEvent.Set();
                    }
                    finally { Monitor.Exit(_target.SyncRoot); }

                    _target.RaiseItemRemoved(0, item);
                    return true;
                }

                public void Reset()
                {
                    Monitor.Enter(_target.SyncRoot);
                    try
                    {
                        if (_cancellationToken.HasValue)
                            _cancellationToken.Value.ThrowIfCancellationRequested();
                        _isEnumerating = true;
                        _isTaking = false;
                        _collectionUnchanged = true;
                        _current = default(T);
                    }
                    finally { Monitor.Exit(_target.SyncRoot); }
                }

                #region IDisposable Support

                private bool disposedValue = false;

                protected virtual void Dispose(bool disposing)
                {
                    if (!disposedValue)
                    {
                        disposedValue = true;
                        if (disposing)
                            _target.CollectionChanged -= Target_CollectionChanged;
                    }
                }

                public void Dispose()
                {
                    Dispose(true);
                }

                #endregion
            }
        }

        #region IDisposable Support
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (disposing)
                {
                    try { _notEmptyEvent.Dispose(); }
                    finally { _notFullEvent.Dispose(); }
                }
            }
        }

        public void Dispose() { Dispose(true); }

        #endregion
    }
}
