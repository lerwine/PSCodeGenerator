using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace CodeGeneratorCommon
{
    public class UniqueCollection<T> : IList<T>, IList
    {
        private InnerList<T> _innerList;

        public IEqualityComparer<T> Comparer => _innerList.Comparer;

        public T this[int index] { get => _innerList[index]; set => throw new NotImplementedException(); }

        object IList.this[int index] { get => _innerList[index]; set => throw new NotImplementedException(); }

        public int Count => _innerList.Count;
        
        bool ICollection<T>.IsReadOnly => false;

        bool IList.IsReadOnly => false;

        bool IList.IsFixedSize => false;

        internal object SyncRoot => _innerList.SyncRoot;

        object ICollection.SyncRoot => _innerList.SyncRoot;

        bool ICollection.IsSynchronized => true;

        public UniqueCollection() : this(null, null) { }

        public UniqueCollection(IEnumerable<T> items) : this(null, items) { }

        public UniqueCollection(IEqualityComparer<T> comparer) : this(comparer, new T[0]) { }

        public UniqueCollection(IEqualityComparer<T> comparer, IEnumerable<T> items)
        {
            Type t = typeof(T);
            if (t.IsClass)
                _innerList = (InnerList<T>)(Activator.CreateInstance((((typeof(INotifyPropertyChanged)).IsAssignableFrom(t)) ? (((typeof(INotifyPropertyChanging)).IsAssignableFrom(t)) ? typeof(InnerClassList3<>) : typeof(InnerClassList2<>)) :
                    typeof(InnerClassList<>)).MakeGenericType(t), comparer, items));
            else
                _innerList = new InnerList<T>(comparer, items);
        }

        public void Add(T item) => _innerList.Add(item);

        int IList.Add(object value)
        {
            Monitor.Enter(_innerList.SyncRoot);
            int index;
            try
            {
                index = _innerList.Count;
                _innerList.Add((T)value);
            }
            finally { Monitor.Exit(_innerList.SyncRoot); }
            return index;
        }

        public void Clear() => _innerList.Clear();

        public bool Contains(T item) => _innerList.Contains(item);

        bool IList.Contains(object value) => value != null && value is T && _innerList.Contains((T)value);

        public void CopyTo(T[] array, int arrayIndex) => _innerList.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => _innerList.ToArray().CopyTo(array, index);

        public IEnumerator<T> GetEnumerator() => _innerList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _innerList.GetEnumerator();

        public int IndexOf(T item) => _innerList.IndexOf(item);

        int IList.IndexOf(object value) => (value != null && value is T) ? _innerList.IndexOf((T)value) : -1;

        public void Insert(int index, T item) => _innerList.Insert(index, item);

        void IList.Insert(int index, object value) => _innerList.Insert(index, (T)value);

        public bool Remove(T item) => _innerList.Remove(item);

        void IList.Remove(object value)
        {
            if (value != null && value is T)
                _innerList.Remove((T)value);
        }

        public void RemoveAt(int index) => _innerList.RemoveAt(index);

        class InnerClassList3<TItem> : InnerClassList2<TItem>, INotifyItemPropertyChanging<TItem>
            where TItem : class, T, INotifyPropertyChanged, INotifyPropertyChanging
        {
            internal InnerClassList3(IEqualityComparer<TItem> comparer, IEnumerable<TItem> items) : base(comparer, (items == null) ? null : items.Where(i => i != null)) { }

            public event EventHandler<ItemPropertyChangeEventArgs<TItem>> ItemPropertyChanging;

            protected override void OnItemAdded(ItemEventArgs<TItem> args)
            {
                try { args.Item.PropertyChanging += Item_PropertyChanging; }
                finally { base.OnItemAdded(args); }
            }

            private void Item_PropertyChanging(object sender, PropertyChangingEventArgs e) { RaiseItemPropertyChanging((TItem)sender, e.PropertyName); }

            private void RaiseItemPropertyChanging(TItem item, string propertyName)
            {
                ItemPropertyChangeEventArgs<TItem> args = new ItemPropertyChangeEventArgs<TItem>(item, propertyName);
                try { OnItemPropertyChanging(args); }
                finally { ItemPropertyChanging?.Invoke(this, args); }
            }

            protected virtual void OnItemPropertyChanging(ItemPropertyChangeEventArgs<TItem> args) { }

            protected override void OnItemRemoved(ItemEventArgs<TItem> args)
            {
                try { args.Item.PropertyChanging -= Item_PropertyChanging; }
                finally { base.OnItemRemoved(args); }
            }
        }

        class InnerClassList2<TItem> : InnerClassList<TItem>, INotifyItemPropertyChanged<TItem>
            where TItem : class, T, INotifyPropertyChanged
        {
            internal InnerClassList2(IEqualityComparer<TItem> comparer, IEnumerable<TItem> items) : base(comparer, (items == null) ? null : items.Where(i => i != null)) { }

            public event EventHandler<ItemPropertyChangeEventArgs<TItem>> ItemPropertyChanged;

            private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e) { RaiseItemPropertyChanged((TItem)sender, e.PropertyName); }

            private void RaiseItemPropertyChanged(TItem item, string propertyName)
            {
                ItemPropertyChangeEventArgs<TItem> args = new ItemPropertyChangeEventArgs<TItem>(item, propertyName);
                try { OnItemPropertyChanged(args); }
                finally
                {
                    try { OnItemChanged(new ItemEventArgs<TItem>(args.Item, IndexOf(args.Item))); }
                    finally { ItemPropertyChanged?.Invoke(this, args); }
                }
            }

            protected override void OnItemAdded(ItemEventArgs<TItem> args)
            {
                args.Item.PropertyChanged += Item_PropertyChanged;
                base.OnItemAdded(args);
            }

            protected virtual void OnItemPropertyChanged(ItemPropertyChangeEventArgs<TItem> args) { }

            protected override void OnItemRemoved(ItemEventArgs<TItem> args)
            {
                args.Item.PropertyChanged -= Item_PropertyChanged;
                base.OnItemRemoved(args);
            }
        }

        class InnerClassList<TItem> : InnerList<TItem>
            where TItem : class, T
        {
            internal InnerClassList(IEqualityComparer<TItem> comparer, IEnumerable<TItem> items) : base(comparer, (items == null) ? null : items.Where(i => i != null), ChangeComparer.Default) { }

            public override void Add(TItem item)
            {
                if (item == null)
                    throw new ArgumentNullException("item");
                base.Add(item);
            }

            public override int IndexOf(TItem item)
            {
                if (item == null)
                    throw new ArgumentNullException("item");
                return base.IndexOf(item);
            }

            public override void Insert(int index, TItem item)
            {
                if (item == null)
                    throw new ArgumentNullException("item");
                base.Insert(index, item);
            }

            protected override bool ItemEquals(TItem x, TItem y) => (x == null) ? y == null : x != null && base.ItemEquals(x, y);

            public override TItem this[int index]
            {
                get => base[index]; set
                {
                    if (value == null)
                        throw new ArgumentNullException();
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (!ReferenceEquals(value, base[index]))
                            base[index] = value;
                    }
                    finally { Monitor.Exit(SyncRoot); }
                }
            }

            class ChangeComparer : IEqualityComparer<TItem>
            {
                public static readonly ChangeComparer Default = new ChangeComparer();
                private IEqualityComparer<TItem> _hashCodeComparer = EqualityComparer<object>.Default;
                public bool Equals(TItem x, TItem y) => (x == null) ? y == null : y != null && ReferenceEquals(x, y);
                public int GetHashCode(TItem obj) => _hashCodeComparer.GetHashCode(obj);
            }
        }
        
        class InnerList<TItem> : IList<TItem>, ICollectionChangeNotify<TItem>, INotifyPropertyChanging, INotifyPropertyChanged
            where TItem : T
        {
            protected internal object SyncRoot { get; }
            private List<TItem> _innerList = new List<TItem>();

            public event PropertyChangingEventHandler PropertyChanging;

            public event PropertyChangedEventHandler PropertyChanged;

            public event NotifyGenericCollectionChangedEventHandler<TItem> CollectionChanged;

            private event NotifyCollectionChangedEventHandler _collectionChanged;

            event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
            {
                add => _collectionChanged += value;
                remove => _collectionChanged -= value;
            }

            public event EventHandler<ItemEventArgs<TItem>> ItemAdding;

            public event EventHandler<ItemEventArgs<TItem>> ItemAdded;

            public event EventHandler<ItemEventArgs<TItem>> ItemRemoving;

            public event EventHandler<ItemEventArgs<TItem>> ItemRemoved;

            public virtual TItem this[int index]
            {
                get => _innerList[index]; set
                {
                    TItem oldItem;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        int i = IndexOf(value);
                        if (!(i < 0 || ItemEquals(value, _innerList[i])))
                            throw new ArgumentOutOfRangeException("item", "Item already exists in the collection");
                        if (ReferenceEquals(Comparer, _changeComparer) || !_changeComparer.Equals(_innerList[index], value))
                        {
                            RaiseRemovingItem(_innerList[index], index);
                            RaiseAddingItem(value, index);
                            oldItem = _innerList[index];
                            _innerList[index] = value;
                        }
                        else
                            return;
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    RaiseItemReplaced(value, oldItem, index);
                }
            }

            private IEqualityComparer<TItem> _changeComparer;

            public IEqualityComparer<TItem> Comparer { get; }

            public int Count => _innerList.Count;

            bool ICollection<TItem>.IsReadOnly => false;

            public InnerList(IEqualityComparer<TItem> comparer, IEnumerable<TItem> items) : this(comparer, items, null) { }

            protected InnerList(IEqualityComparer<TItem> comparer, IEnumerable<TItem> items, IEqualityComparer<TItem> changeComparer)
            {
                SyncRoot = new object();
                Comparer = comparer ?? EqualityComparer<TItem>.Default;
                try { _changeComparer = changeComparer ?? EqualityComparer<TItem>.Default; }
                catch { _changeComparer = comparer; }
            }

            public virtual void Add(TItem item)
            {
                int index;
                Monitor.Enter(SyncRoot);
                try
                {
                    if (Contains(item))
                        throw new ArgumentOutOfRangeException("item", "Item already exists in the collection");
                    RaiseAddingItem(item, _innerList.Count);
                    index = _innerList.Count;
                    _innerList.Add(item);
                }
                finally { Monitor.Exit(SyncRoot); }
                try { RaiseItemAdded(item, index); }
                finally { RaisePropertyChanged("Count"); }
            }

            public void Clear()
            {
                TItem[] itemsRemoved;
                Monitor.Enter(SyncRoot);
                try
                {
                    itemsRemoved = _innerList.ToArray();
                    for (int index = 0; index < itemsRemoved.Length; index++)
                        RaiseRemovingItem(itemsRemoved[index], index);
                    _innerList.Clear();
                }
                finally { Monitor.Exit(SyncRoot); }
                RaiseReset(itemsRemoved, 0);
            }

            public bool Contains(TItem item) => _innerList.Any(i => ItemEquals(i, item));

            protected virtual bool ItemEquals(TItem x, TItem y) => Comparer.Equals(x, y);

            public void CopyTo(TItem[] array, int arrayIndex) => _innerList.CopyTo(array, arrayIndex);

            public IEnumerator<TItem> GetEnumerator() => _innerList.GetEnumerator();

            public virtual int IndexOf(TItem item)
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    for (int i = 0; i < _innerList.Count; i++)
                    {
                        if (ItemEquals(_innerList[i], item))
                            return i;
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
                return -1;
            }

            public virtual void Insert(int index, TItem item)
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (Contains(item))
                        throw new ArgumentOutOfRangeException("item", "Item already exists in the collection");
                    RaiseAddingItem(item, index);
                    _innerList.Insert(index, item);
                }
                finally { Monitor.Exit(SyncRoot); }
                RaiseItemAdded(item, index);
            }

            protected virtual void OnCollectionChanged(NotifyGenericCollectionChangedEventArgs<TItem> args) { }

            protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) { }

            protected virtual void OnAddingItem(ItemEventArgs<TItem> args) { }

            protected virtual void OnItemAdded(ItemEventArgs<TItem> args) { }

            protected virtual void OnItemChanged(ItemEventArgs<TItem> args) { }

            protected virtual void OnRemovingItem(ItemEventArgs<TItem> args) { }

            protected virtual void OnItemRemoved(ItemEventArgs<TItem> args) { }

            private void RaiseCollectionChanged(NotifyGenericCollectionChangedEventArgs<TItem> args)
            {
                try { OnCollectionChanged(args); }
                finally
                {
                    try { CollectionChanged?.Invoke(this, args); }
                    finally { _collectionChanged?.Invoke(this, args); }
                }
            }

            private void RaiseAddingItem(TItem item, int index)
            {
                ItemEventArgs<TItem> args = new ItemEventArgs<TItem>(item, index);
                try { OnAddingItem(args); }
                finally { ItemAdding?.Invoke(this, args); }
            }

            private void RaiseItemAdded(TItem newItem, int index)
            {
                try { OnItemAdded(new ItemEventArgs<TItem>(newItem, index)); }
                finally
                {
                    try { ItemAdded?.Invoke(this, new ItemEventArgs<TItem>(newItem, index)); }
                    finally { RaiseCollectionChanged(new NotifyGenericCollectionChangedEventArgs<TItem>(NotifyCollectionChangedAction.Add, newItem, index)); }
                }
            }
            
            private void RaiseRemovingItem(TItem item, int index)
            {
                ItemEventArgs<TItem> args = new ItemEventArgs<TItem>(item, index);
                try { OnRemovingItem(args); }
                finally { ItemRemoving?.Invoke(this, args); }
            }

            private void RaiseItemRemoved(TItem newItem, int index)
            {
                try { OnItemRemoved(new ItemEventArgs<TItem>(newItem, index)); }
                finally
                {
                    try { ItemRemoved?.Invoke(this, new ItemEventArgs<TItem>(newItem, index)); }
                    finally { RaiseCollectionChanged(new NotifyGenericCollectionChangedEventArgs<TItem>(NotifyCollectionChangedAction.Remove, newItem, index)); }
                }
            }

            protected void RaiseItemReplaced(TItem newItem, TItem oldItem, int index)
            {
                ItemEventArgs<TItem> args = new ItemEventArgs<TItem>(oldItem, index);
                try { OnItemRemoved(args); }
                finally
                {
                    try { ItemRemoved?.Invoke(this, args); }
                    finally
                    {
                        args = new ItemEventArgs<TItem>(newItem, index);
                        try { OnItemAdded(args); }
                        finally
                        {
                            try { ItemAdded?.Invoke(this, args); }
                            finally { RaiseCollectionChanged(new NotifyGenericCollectionChangedEventArgs<TItem>(NotifyCollectionChangedAction.Replace, newItem, oldItem, index)); }
                        }
                    }
                }
            }

            private void RaisePropertyChanged(string propertyName)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
                try { OnPropertyChanged(args); }
                finally { PropertyChanged?.Invoke(this, args); }
            }

            private void RaiseReset(TItem[] itemsRemoved, int startingIndex)
            {
                try
                {
                    for (int index = itemsRemoved.Length - 1; index >= 0; index--)
                        RaiseItemRemoved(itemsRemoved[index], index + startingIndex);
                }
                finally { RaiseCollectionChanged(new NotifyGenericCollectionChangedEventArgs<TItem>(NotifyCollectionChangedAction.Reset, itemsRemoved, startingIndex)); }
            }

            public bool Remove(TItem item)
            {
                int index = -1;
                Monitor.Enter(SyncRoot);
                try
                {
                    for (int i = 0; i < _innerList.Count; i++)
                    {
                        if (ItemEquals(_innerList[i], item))
                        {
                            index = i;
                            item = _innerList[i];
                            RaiseRemovingItem(item, i);
                            _innerList.RemoveAt(i);
                            break;
                        }
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
                if (index < 0)
                    return false;
                RaiseItemRemoved(item, index);
                return true;
            }

            public void RemoveAt(int index)
            {
                TItem item;
                Monitor.Enter(SyncRoot);
                try
                {
                    item = _innerList[index];
                    RaiseRemovingItem(item, index);
                    _innerList.RemoveAt(index);
                }
                finally { Monitor.Exit(SyncRoot); }
                RaiseItemRemoved(item, index);
            }

            IEnumerator IEnumerable.GetEnumerator() => _innerList.GetEnumerator();
        }
    }
}
