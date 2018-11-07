using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeGeneratorCommon
{
    public class ItemProductionTask<T> : IAsyncResult, IDisposable
    {
        private Task _innerTask;
        private Action<ObjectGetState> _action;
        public bool IsCompleted => _innerTask.IsCompleted;
        public bool IsCanceled => _innerTask.IsCanceled;
        public TaskStatus Status => _innerTask.Status;
        public AggregateException Exception => _innerTask.Exception;
        public int TaskId => _innerTask.Id;
        public TaskCreationOptions CreationOptions => _innerTask.CreationOptions;
        public bool IsFaulted => _innerTask.IsFaulted;
        WaitHandle IAsyncResult.AsyncWaitHandle => ((IAsyncResult)_innerTask).AsyncWaitHandle;
        public object AsyncState => _innerTask.AsyncState;
        bool IAsyncResult.CompletedSynchronously => ((IAsyncResult)_innerTask).CompletedSynchronously;

        private void TaskAction(object userState)
        {
            ObjectGetState getObject = null;
            do
            {
                getObject = new ObjectGetState(userState);
                Task task = new Task(() =>
                {
                    _action(getObject);
                }).ContinueWith(t =>
                {
                    if (t.IsCanceled)
                        getObject.SetCanceled(true);
                    else if (t.IsFaulted)
                        getObject.SetError(t.Exception, true);
                });
                task.Wait();
                GetObjectResult result = GetObjectResult.Create(getObject);
                while (!_queue.TryEnqueue(result, 125)) ;
            }
            while (!getObject.EndOfEnum);
        }

        private void TaskAction(object userState, CancellationToken cancellationToken)
        {
            ObjectGetState getObject = null;
            do
            {
                cancellationToken.ThrowIfCancellationRequested();
                Task task = new Task(() =>
                {
                    _action(getObject);
                }).ContinueWith(t =>
                {
                    if (t.IsCanceled)
                        getObject.SetCanceled(true);
                    else if (t.IsFaulted)
                        getObject.SetError(t.Exception, true);
                });
                task.Wait();
                GetObjectResult result = GetObjectResult.Create(getObject);
                while (!_queue.TryEnqueue(result, 125))
                    cancellationToken.ThrowIfCancellationRequested();
            }
            while (!getObject.EndOfEnum);
        }

        private ItemProductionTask() { }

        public ItemProductionTask(Action<ObjectGetState> action)
        {
            _action = action;
            _innerTask = new Task(() => TaskAction(null));
        }
        public ItemProductionTask(Action<ObjectGetState> action, CancellationToken cancellationToken)
        {
            _action = action;
            _innerTask = new Task(() => TaskAction(null, cancellationToken), cancellationToken);
        }
        public ItemProductionTask(Action<ObjectGetState> action, TaskCreationOptions creationOptions)
        {
            _action = action;
            _innerTask = new Task(() => TaskAction(null), creationOptions);
        }
        public ItemProductionTask(Action<ObjectGetState> action, object state)
        {
            _action = action;
            _innerTask = new Task(s => TaskAction(s), state);
        }
        public ItemProductionTask(Action<ObjectGetState> action, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
        {
            _action = action;
            _innerTask = new Task(() => TaskAction(null, cancellationToken), cancellationToken, creationOptions);
        }
        public ItemProductionTask(Action<ObjectGetState> action, object state, CancellationToken cancellationToken)
        {
            _action = action;
            _innerTask = new Task(s => TaskAction(s, cancellationToken), state, cancellationToken);
        }
        public ItemProductionTask(Action<ObjectGetState> action, object state, TaskCreationOptions creationOptions)
        {
            _action = action;
            _innerTask = new Task(s => TaskAction(s), state, creationOptions);
        }
        public ItemProductionTask(Action<ObjectGetState> action, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions)
        {
            _action = action;
            _innerTask = new Task(s => TaskAction(s, cancellationToken), state, cancellationToken, creationOptions);
        }
        public static ItemProductionTask<T> StartNew(Action<ObjectGetState> action)
        {
            ItemProductionTask<T> task = new ItemProductionTask<T> { _action = action };
            task._innerTask = Task.Factory.StartNew(() => task.TaskAction(null));
            return task;
        }
        public static ItemProductionTask<T> StartNew(Action<ObjectGetState> action, CancellationToken cancellationToken)
        {
            ItemProductionTask<T> task = new ItemProductionTask<T> { _action = action };
            task._innerTask = Task.Factory.StartNew(() => task.TaskAction(null, cancellationToken), cancellationToken);
            return task;
        }
        public static ItemProductionTask<T> StartNew(Action<ObjectGetState> action, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
        {
            ItemProductionTask<T> task = new ItemProductionTask<T> { _action = action };
            task._innerTask = Task.Factory.StartNew(() => task.TaskAction(null, cancellationToken), cancellationToken, creationOptions, scheduler);
            return task;
        }
        public static ItemProductionTask<T> StartNew(Action<ObjectGetState> action, object state)
        {
            ItemProductionTask<T> task = new ItemProductionTask<T> { _action = action };
            task._innerTask = Task.Factory.StartNew(s => task.TaskAction(s), state);
            return task;
        }
        public static ItemProductionTask<T> StartNew(Action<ObjectGetState> action, object state, CancellationToken cancellationToken)
        {
            ItemProductionTask<T> task = new ItemProductionTask<T> { _action = action };
            task._innerTask = Task.Factory.StartNew(s => task.TaskAction(s, cancellationToken), state, cancellationToken);
            return task;
        }
        public static ItemProductionTask<T> StartNew(Action<ObjectGetState> action, object state, TaskCreationOptions creationOptions)
        {
            ItemProductionTask<T> task = new ItemProductionTask<T> { _action = action };
            task._innerTask = Task.Factory.StartNew(s => task.TaskAction(s), state, creationOptions);
            return task;
        }
        public static ItemProductionTask<T> StartNew(Action<ObjectGetState> action, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler)
        {
            ItemProductionTask<T> task = new ItemProductionTask<T> { _action = action };
            task._innerTask = Task.Factory.StartNew(s => task.TaskAction(s, cancellationToken), state, cancellationToken, creationOptions, scheduler);
            return task;
        }
        public static ItemProductionTask<T> StartNew(Action<ObjectGetState> action, TaskCreationOptions creationOptions)
        {
            ItemProductionTask<T> task = new ItemProductionTask<T> { _action = action };
            task._innerTask = Task.Factory.StartNew(() => task.TaskAction(null), creationOptions);
            return task;
        }

        public static ItemProductionTask<T> FromResult(IEnumerable<T> result)
        {
            ItemProductionTask<T> task = new ItemProductionTask<T>();
            task._innerTask = Task.Factory.StartNew(() =>
            {
                foreach (T value in result)
                    task._queue.Enqueue(new GetObjectResult(value, null, false, null));
            });
            return task;
        }

        private ResultQueueItem.ResultQueue _queue = new ResultQueueItem.ResultQueue(32);

        public static void WaitAll(params ItemProductionTask<T>[] tasks)
        {
            Task[] tArr;
            if (tasks == null || (tArr = tasks.Where(t => t != null).Select(t => t._innerTask).Where(t => t != null).ToArray()).Length == 0)
                return;
            Task.WaitAll(tArr);
        }
        public static bool WaitAll(ItemProductionTask<T>[] tasks, TimeSpan timeout)
        {
            Task[] tArr;
            if (tasks == null || (tArr = tasks.Where(t => t != null).Select(t => t._innerTask).Where(t => t != null).ToArray()).Length == 0)
                return true;
            return Task.WaitAll(tArr, timeout);
        }
        public static bool WaitAll(ItemProductionTask<T>[] tasks, int millisecondsTimeout)
        {
            Task[] tArr;
            if (tasks == null || (tArr = tasks.Where(t => t != null).Select(t => t._innerTask).Where(t => t != null).ToArray()).Length == 0)
                return true;
            return Task.WaitAll(tArr, millisecondsTimeout);
        }
        public static void WaitAll(ItemProductionTask<T>[] tasks, CancellationToken cancellationToken)
        {
            Task[] tArr;
            if (tasks == null || (tArr = tasks.Where(t => t != null).Select(t => t._innerTask).Where(t => t != null).ToArray()).Length == 0)
                return;
            Task.WaitAll(tArr, cancellationToken);
        }
        public static bool WaitAll(ItemProductionTask<T>[] tasks, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            Task[] tArr;
            if (tasks == null || (tArr = tasks.Where(t => t != null).Select(t => t._innerTask).Where(t => t != null).ToArray()).Length == 0)
                return true;
            return Task.WaitAll(tArr, millisecondsTimeout, cancellationToken);
        }
        public static int WaitAny(params ItemProductionTask<T>[] tasks)
        {
            if (tasks == null || tasks.Length == 0)
                return -1;
            var tArr = tasks.Select((t, i) => new { Index = i, Task = (t == null) ? null : t._innerTask }).Where(a => a.Task != null).ToArray();
            int index = Task.WaitAny(tArr.Select(t => t.Task).ToArray());
            if (index >= 0)
                return tArr[index].Index;
            return index;
        }
        public static int WaitAny(ItemProductionTask<T>[] tasks, TimeSpan timeout)
        {
            if (tasks == null || tasks.Length == 0)
                return -1;
            var tArr = tasks.Select((t, i) => new { Index = i, Task = (t == null) ? null : t._innerTask }).Where(a => a.Task != null).ToArray();
            int index = Task.WaitAny(tArr.Select(t => t.Task).ToArray(), timeout);
            if (index >= 0)
                return tArr[index].Index;
            return index;
        }
        public static int WaitAny(ItemProductionTask<T>[] tasks, int millisecondsTimeout)
        {
            if (tasks == null || tasks.Length == 0)
                return -1;
            var tArr = tasks.Select((t, i) => new { Index = i, Task = (t == null) ? null : t._innerTask }).Where(a => a.Task != null).ToArray();
            int index = Task.WaitAny(tArr.Select(t => t.Task).ToArray(), millisecondsTimeout);
            if (index >= 0)
                return tArr[index].Index;
            return index;
        }
        public static int WaitAny(ItemProductionTask<T>[] tasks, CancellationToken cancellationToken)
        {
            if (tasks == null || tasks.Length == 0)
                return -1;
            var tArr = tasks.Select((t, i) => new { Index = i, Task = (t == null) ? null : t._innerTask }).Where(a => a.Task != null).ToArray();
            int index = Task.WaitAny(tArr.Select(t => t.Task).ToArray(), cancellationToken);
            if (index >= 0)
                return tArr[index].Index;
            return index;
        }
        public static int WaitAny(ItemProductionTask<T>[] tasks, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            if (tasks == null || tasks.Length == 0)
                return -1;
            var tArr = tasks.Select((t, i) => new { Index = i, Task = (t == null) ? null : t._innerTask }).Where(a => a.Task != null).ToArray();
            int index = Task.WaitAny(tArr.Select(t => t.Task).ToArray(), millisecondsTimeout, cancellationToken);
            if (index >= 0)
                return tArr[index].Index;
            return index;
        }

        //public static ItemProductionTask<T> WhenAll(IEnumerable<ItemProductionTask<T>> tasks);
        //public static ItemProductionTask<T> WhenAll(params ItemProductionTask<T>[] tasks);
        //public static Task<ItemProductionTask<T>> WhenAny(params ItemProductionTask<T>[] tasks);
        //public static Task<ItemProductionTask<T>> WhenAny(IEnumerable<ItemProductionTask<T>> tasks);

        public TaskAwaiter GetAwaiter() => _innerTask.GetAwaiter();
        public void RunSynchronously() => _innerTask.RunSynchronously();
        public void RunSynchronously(TaskScheduler scheduler) => _innerTask.RunSynchronously(scheduler);
        public void Start() => _innerTask.Start();
        public void Start(TaskScheduler scheduler) => _innerTask.Start(scheduler);
        public bool Wait(int millisecondsTimeout) => _innerTask.Wait(millisecondsTimeout);
        public void Wait(CancellationToken cancellationToken) => _innerTask.Wait(cancellationToken);
        public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken) => _innerTask.Wait(millisecondsTimeout, cancellationToken);
        public void Wait() => _innerTask.Wait();
        public bool Wait(TimeSpan timeout) => _innerTask.Wait(timeout);

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ItemProductionTask() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        public class ResultQueueItem
        {
            private ResultQueueItem _next = null;
            public GetObjectResult Result { get; private set; }
            public ResultQueueItem(GetObjectResult result) { Result = result; }
            public class ResultQueue
            {
                public ResultQueue(int limit) { Limit = (limit < 1) ? 1 : limit; }
                public object SyncRoot = new object();
                private ResultQueueItem _firstItem = null;
                private ResultQueueItem _lastItem = null;
                private ManualResetEvent _notEmptyWaitHandle = new ManualResetEvent(false);
                private ManualResetEvent _notFullWaitHandle = new ManualResetEvent(true);
                public int Count { get; private set; }
                public int Limit { get; private set; }
                public void Enqueue(GetObjectResult item)
                {
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        Count++;
                        if (_firstItem == null)
                        {
                            _firstItem = _lastItem = new ResultQueueItem(item);
                            _notEmptyWaitHandle.Set();
                        }
                        else if (_firstItem._next == null)
                            _firstItem._next = _lastItem = new ResultQueueItem(item);
                        else
                            _lastItem = _lastItem._next = new ResultQueueItem(item);
                        if (Count == Limit)
                            _notFullWaitHandle.Reset();
                    }
                    finally { Monitor.Exit(SyncRoot); }
                }
                public bool TryEnqueue(GetObjectResult item, int millisecondsTimeout)
                {
                    if (_notEmptyWaitHandle.WaitOne(millisecondsTimeout))
                        return false;

                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (Count >= Limit)
                            return false;
                        Count++;
                        if (_firstItem == null)
                        {
                            _firstItem = _lastItem = new ResultQueueItem(item);
                            _notEmptyWaitHandle.Set();
                        }
                        else if (_firstItem._next == null)
                            _firstItem._next = _lastItem = new ResultQueueItem(item);
                        else
                            _lastItem = _lastItem._next = new ResultQueueItem(item);
                        if (Count == Limit)
                            _notFullWaitHandle.Reset();
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    return true;
                }
                public bool WaitOne(int millisecondsTimeout) => _notEmptyWaitHandle.WaitOne(millisecondsTimeout);

                public bool TryDequeue(int millisecondsTimeout, out GetObjectResult item)
                {
                    if (_notEmptyWaitHandle.WaitOne(millisecondsTimeout))
                    {
                        Monitor.Enter(SyncRoot);
                        try
                        {
                            if (_firstItem != null)
                            {
                                if (Count == Limit)
                                {
                                    Count--;
                                    _notFullWaitHandle.Set();
                                }
                                else
                                    Count--;
                                
                                item = _firstItem.Result;
                                if (_firstItem._next == null)
                                {
                                    _firstItem = _lastItem = null;
                                    _notEmptyWaitHandle.Reset();
                                }
                                else if ((_firstItem = _firstItem._next)._next == null)
                                    _lastItem = _firstItem;
                                return true;
                            }
                        }
                        finally { Monitor.Exit(SyncRoot); }
                    }
                    item = null;
                    return false;
                }
            }
        }
        
        public class ObjectGetState
        {
            public bool HasValue { get; private set; }
            public T Value { get; private set; }
            public bool IsCanceled { get; private set; }
            public bool EndOfEnum { get; private set; }
            public bool IsFaulted { get; private set; }
            public Exception Exception { get; private set; }
            public object UserState { get; }

            public ObjectGetState(object state) { UserState = state; }

            public void SetValue(T value, bool isFinalValue = false)
            {
                HasValue = true;
                Value = value;
                IsCanceled = false;
                IsFaulted = false;
                Exception = null;
                EndOfEnum = isFinalValue;
            }

            public void SetEndOfENum() { EndOfEnum = true; }

            public void SetCanceled(bool isEnumEnd = false)
            {
                HasValue = false;
                IsCanceled = true;
                IsFaulted = false;
                Exception = null;
                EndOfEnum = isEnumEnd;
            }

            public void SetError(Exception error, bool isEnumEnd = false)
            {
                HasValue = false;
                IsCanceled = false;
                IsFaulted = error != null;
                Exception = error;
                EndOfEnum = isEnumEnd;
            }
        }

        public class GetObjectResult : AsyncCompletedEventArgs
        {
            public bool HasValue { get; private set; }
            public T Value { get; private set; }
            public bool IsFaulted { get; private set; }

            public static GetObjectResult Create(ObjectGetState getObject) => (getObject == null) ? null : new GetObjectResult(getObject);

            private GetObjectResult(Exception error, bool cancelled, object userState) : base(error, cancelled, userState) { }

            private GetObjectResult(ObjectGetState getObject)
                : base(getObject.Exception, getObject.IsCanceled, getObject.UserState)
            {
                if (getObject == null)
                    return;
                HasValue = getObject.HasValue;
                Value = getObject.Value;
                IsFaulted = getObject.IsFaulted;
            }

            public GetObjectResult(T value, Exception error, bool cancelled, object userState) : base(null, false, null)
            {
                Value = value;
                IsFaulted = !(cancelled || error == null);
                HasValue = !(cancelled || IsFaulted);
            }
        }
    }
}
