using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeGeneratorCommon
{
    public interface ITransformQueue : ICollection
    {
        ICollection Collection { get; }
        bool IsEmpty { get; }
    }

    public interface ITransformQueue<T> : ITransformQueue, IEnumerable<T>
    {
        new IEnumerable<T> Collection { get; }
    }

    public interface ITransformSourceQueue : ITransformQueue
    {
        bool IsAddingCompleted { get; }
    }

    public interface ITransformSourceQueue<T> : ITransformQueue<T>, ITransformSourceQueue
    {
        bool TryTake(out T item);
    }
    
    public interface ITransformTargetQueue<T> : ITransformQueue<T>
    {
        bool TryAdd(T item);
    }

    public abstract class TransformQueue : ITransformQueue
    {
        public ICollection Collection { get; }
        public virtual bool IsEmpty => Collection.Count == 0;
        int ICollection.Count => Collection.Count;
        protected object SyncRoot => Collection.SyncRoot;
        object ICollection.SyncRoot => Collection.SyncRoot;
        bool ICollection.IsSynchronized => Collection.IsSynchronized;
        protected TransformQueue(ICollection collection) { Collection = collection; }
        public void CopyTo(Array array, int index) => Collection.CopyTo(array, index);
        IEnumerator IEnumerable.GetEnumerator() => Collection.GetEnumerator();
    }

    public abstract class TransformQueue<TItem, TSource> : TransformQueue, ITransformQueue<TItem>
        where TSource : class, IEnumerable<TItem>, ICollection
    {
        public new TSource Collection { get; }
        IEnumerable<TItem> ITransformQueue<TItem>.Collection => throw new NotImplementedException();
        protected TransformQueue(TSource collection) : base(collection) { Collection = collection; }
        public IEnumerator<TItem> GetEnumerator() => Collection.GetEnumerator();
    }

    public abstract class TransformSourceQueue<TItem, TSource> : TransformQueue<TItem, TSource>, ITransformSourceQueue<TItem>
        where TSource : class, IEnumerable<TItem>, ICollection
    {
        public abstract bool IsAddingCompleted { get; }
        protected TransformSourceQueue(TSource collection) : base(collection) { }
        public abstract bool TryTake(out TItem item);
    }

    public abstract class TransformTargetQueue<TItem, TSource> : TransformQueue<TItem, TSource>, ITransformTargetQueue<TItem>
        where TSource : class, IEnumerable<TItem>, ICollection
    {
        protected TransformTargetQueue(TSource collection) : base(collection) { }
        public abstract bool TryAdd(TItem item);
    }

    public class ProducerConsumnerTransformSource<TItem, TSource> : TransformSourceQueue<TItem, TSource>
        where TSource : class, IProducerConsumerCollection<TItem>
    {
        public override bool IsAddingCompleted => throw new NotImplementedException();
        public ProducerConsumnerTransformSource(TSource collection, WaitHandle isAddingCompletedEvent) : base(collection) { }
        public override bool TryTake(out TItem item) => Collection.TryTake(out item);

        public static ProducerConsumnerTransformSource<TItem, TSource> Create(TSource source, WaitHandle isAddingCompletedEvent)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (source is ConcurrentQueue<TItem>)
                return (ProducerConsumnerTransformSource<TItem, TSource>)(Activator.CreateInstance(typeof(ConcurrentTransformSource<,>).MakeGenericType(typeof(TItem), typeof(TSource)), source,
                    isAddingCompletedEvent ?? throw new ArgumentNullException("isAddingCompletedEvent")));
            return new ProducerConsumnerTransformSource<TItem, TSource>(source, isAddingCompletedEvent ?? throw new ArgumentNullException("isAddingCompletedEvent"));
        }
    }

    public class ProducerConsumnerTransformTarget<TItem, TSource> : TransformTargetQueue<TItem, TSource>
        where TSource : class, IProducerConsumerCollection<TItem>
    {
        public ProducerConsumnerTransformTarget(TSource collection) : base(collection) { }
        public override bool TryAdd(TItem item) => Collection.TryAdd(item);
        public static implicit operator ProducerConsumnerTransformTarget<TItem, TSource>(TSource source) { return new ProducerConsumnerTransformTarget<TItem, TSource>(source); }

        public static ProducerConsumnerTransformTarget<TItem, TSource> Create(TSource source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (source is ConcurrentQueue<TItem>)
                return (ProducerConsumnerTransformTarget<TItem, TSource>)(Activator.CreateInstance(typeof(ConcurrentTransformTarget<,>).MakeGenericType(typeof(TItem), typeof(TSource)), source));
            return new ProducerConsumnerTransformTarget<TItem, TSource>(source);
        }
    }

    public class ConcurrentTransformSource<TItem, TSource> : ProducerConsumnerTransformSource<TItem, TSource>
        where TSource : ConcurrentQueue<TItem>
    {
        public override bool IsEmpty => Collection.IsEmpty;
        public ConcurrentTransformSource(TSource source, WaitHandle isAddingCompletedEvent) : base(source, isAddingCompletedEvent) { }
    }

    public class ConcurrentTransformTarget<TItem, TSource> : ProducerConsumnerTransformTarget<TItem, TSource>
        where TSource : ConcurrentQueue<TItem>
    {
        public override bool IsEmpty => Collection.IsEmpty;
        public ConcurrentTransformTarget(TSource source) : base(source) { }
        public static implicit operator ConcurrentTransformTarget<TItem, TSource>(TSource source) { return new ConcurrentTransformTarget<TItem, TSource>(source); }
    }

    public class BlockingCollectionTransformSource<TItem, TSource> : TransformSourceQueue<TItem, TSource>
        where TSource : BlockingCollection<TItem>
    {
        public override bool IsAddingCompleted => Collection.IsAddingCompleted;
        public BlockingCollectionTransformSource(TSource collection) : base(collection) { }
        public override bool TryTake(out TItem item) => Collection.TryTake(out item);
        public static implicit operator BlockingCollectionTransformSource<TItem, TSource>(TSource source) { return new BlockingCollectionTransformSource<TItem, TSource>(source); }
    }

    public class BlockingCollectionTransformTarget<TItem, TSource> : TransformTargetQueue<TItem, TSource>
        where TSource : BlockingCollection<TItem>
    {
        public BlockingCollectionTransformTarget(TSource collection) : base(collection) { }
        public override bool TryAdd(TItem item) => Collection.TryAdd(item);
        public static implicit operator BlockingCollectionTransformTarget<TItem, TSource>(TSource source) { return new BlockingCollectionTransformTarget<TItem, TSource>(source); }
    }

    public abstract class QueueTransformer<TInputItem, TOutputItem> : IDisposable
    {
        private readonly object _syncRoot;
        private BlockingCollection<Worker> _workers = new BlockingCollection<Worker>();
        protected ITransformSourceQueue<TInputItem> InputQueue { get; }
        protected ITransformTargetQueue<TOutputItem> OutputQueue { get; }
        public BlockingCollection<FailedItem> FailedItems { get; }
        protected QueueTransformer(ITransformSourceQueue<TInputItem> inputQueue, ITransformTargetQueue<TOutputItem> outputQueue)
        {
            object syncRoot;
            _syncRoot = (((ICollection)_workers).IsSynchronized && (syncRoot = ((ICollection)_workers).SyncRoot) != null) ? syncRoot : new object();
        }

        #region IDisposable Support

        private bool _isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                if (_isDisposed)
                    return;
                _isDisposed = true;
            }
            finally { Monitor.Exit(_syncRoot); }

            if (!disposing)
                return;

            try
            {
                while (_workers.Count > 0)
                {
                    Worker worker;
                    if (_workers.TryTake(out worker, 250))
                        worker.Stop();
                }
            }
            finally { FailedItems.Dispose(); }
        }

        public void Dispose() => Dispose(true);

        #endregion

        public class FailedItem
        {
            public TInputItem Item { get; }
            public int Position { get; }
            public Exception Error { get; }
            public bool IsCancelled { get; }

            public FailedItem(TInputItem item, int position, Exception error, bool isCancelled)
            {
                Item = item;
                Position = position;
                Error = error;
                IsCancelled = isCancelled;
            }
        }

        public abstract class Worker
        {
            private object _syncRoot = new object();
            private TransformQueue<TInputItem, TInputCollection, TOutputItem, TOutputCollection> _queue;

            public CancellationToken Token { get; private set; }

            protected virtual void BeforeStartNewQueue() { }

            protected virtual void AfterLastItemProcessed() { }

            protected abstract bool ProcessInput(TInputItem current, int position);

            class WorkerCollection : BlockingCollection<Worker>
            {

            }
        }
    }
    
    public class QueueTransformer<TInputItem, TInputCollection, TOutputItem, TOutputCollection> : QueueTransformer<TInputItem, TOutputItem>
        where TInputCollection : class, IEnumerable<TInputItem>, ICollection
        where TOutputCollection : class, IEnumerable<TOutputItem>, ICollection
    {
        protected TransformSourceQueue<TInputItem, TInputCollection> InputQueue { get; }
        protected TransformTargetQueue<TInputItem, TInputCollection> OutputQueue { get; }

        public TransformQueue(TransformSourceQueue<TInputItem, TInputCollection> inputQueue, TransformTargetQueue<TInputItem, TInputCollection> outputQueue)
        {
            InputQueue = inputQueue ?? throw new ArgumentNullException("inputQueue");
            OutputQueue = outputQueue ?? throw new ArgumentNullException("outputQueue");
        }
    }

    public class ProducerConsumerTransformQueue<TInputItem, TInputCollection, TOutputItem, TOutputCollection> : QueueTransformer<TInputItem, TInputCollection, TOutputItem, TOutputCollection>
        where TInputCollection : class, IProducerConsumerCollection<TInputItem>
        where TOutputCollection : class, IEnumerable<TOutputItem>, ICollection
    {
        protected ProducerConsumerTransformQueue(TInputCollection source, WaitHandle isAddingCompletedEvent, TransformTargetQueue<TInputItem, TInputCollection> outputQueue)
            : base(ProducerConsumnerTransformSource<TInputItem, TInputCollection>.Create(source, isAddingCompletedEvent), outputQueue) { }

    }

    public class BlockingCollectionTransformQueue<TInputItem, TInputCollection, TOutputItem, TOutputCollection> : QueueTransformer<TInputItem, TInputCollection, TOutputItem, TOutputCollection>
        where TInputCollection : BlockingCollection<TInputItem>
        where TOutputCollection : class, IEnumerable<TOutputItem>, ICollection
    {
        protected BlockingCollectionTransformQueue(TInputCollection source, WaitHandle isAddingCompletedEvent, TransformTargetQueue<TInputItem, TInputCollection> outputQueue)
            : base(new BlockingCollectionTransformSource<TInputItem, TInputCollection>(source ?? throw new ArgumentNullException("source")), outputQueue) { }

    }
}
