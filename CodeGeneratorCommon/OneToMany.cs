using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeGeneratorCommon
{
    public abstract class OneToMany<TPrimary, TItem, TCollection>
        where TPrimary : OneToMany<TPrimary, TItem, TCollection>
        where TItem : OneToMany<TPrimary, TItem, TCollection>.ItemNode
        where TCollection : OneToMany<TPrimary, TItem, TCollection>.ItemNode.ItemCollection
    {
        /// <summary>
        /// Collection of <typeparamref name="TItem"/> objects.
        /// </summary>
        protected TCollection BaseItems { get; }

        private void __OnItemsChanged(CollectionChangedEventArgs args) => BaseItemsChanged(args);

        private void __OnItemsAdded(TItem[] items, int index, bool isInsert) => BaseItemsAdded(items, index, isInsert);

        private void __OnItemReplaced(TItem newItem, TItem oldItem, int index) => BaseItemReplaced(newItem, oldItem, index);

        private void __OnItemsRemoved(TItem[] items, int index) => BaseItemsRemoved(items, index);

        /// <summary>
        /// This gets called whenever the <seealso cref="BaseItems"/> collection changes.
        /// </summary>
        /// <param name="args">Describes the change to <seealso cref="BaseItems"/>.</param>
        protected virtual void BaseItemsChanged(CollectionChangedEventArgs args) { }

        /// <summary>
        /// This gets called whenever one or more <typeparamref name="TItem"/> objects are appended to or inserted into <seealso cref="BaseItems"/>.
        /// </summary>
        /// <param name="items"><typeparamref name="TItem"/> object(s) that were addedor inserted.</param>
        /// <param name="index">Starting index at which <paramref name="items"/> where added or inserted.</param>
        /// <param name="isInsert"><c>true</c> if <paramref name="items"/> were added through an insert operation; otherwise <c>false</c> if <paramref name="items"/> where appended to the end of <seealso cref="BaseItems"/>.</param>
        protected virtual void BaseItemsAdded(TItem[] items, int index, bool isInsert) { }

        /// <summary>
        /// This gets called whenever one <typeparamref name="TItem"/> is replaced with another in the <seealso cref="BaseItems"/> collection.
        /// </summary>
        /// <param name="newItem">Item that has replaced the previous item.</param>
        /// <param name="oldItem">Item that has been replaced by the new item.</param>
        /// <param name="index">Index at which the replacement occurred.</param>
        protected virtual void BaseItemReplaced(TItem newItem, TItem oldItem, int index) { }

        /// <summary>
        /// This gets called whenever one or more <typeparamref name="TItem"/> objects are removed from teh <seealso cref="BaseItems"/> collection.
        /// </summary>
        /// <param name="items"><typeparamref name="TItem"/> object(s) that were removed/</param>
        /// <param name="index">Starting index at which the <paramref name="items"/> were removed.</param>
        protected virtual void BaseItemsRemoved(TItem[] items, int index) { }

        /// <summary>
        /// Provides data for the System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged event.
        /// </summary>
        public class CollectionChangedEventArgs : NotifyCollectionChangedEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CollectionChangedEventArgs"/> class that describes a <seealso cref="NotifyCollectionChangedAction.Reset"/> change.
            /// </summary>
            /// <param name="action">The action that caused the event. This must be set to <seealso cref="NotifyCollectionChangedAction.Reset"/>.</param>
            public CollectionChangedEventArgs(NotifyCollectionChangedAction action) : base(action) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="CollectionChangedEventArgs"/> class that describes a one-item change.
            /// </summary>
            /// <param name="action">The action that caused the event. This can be set to <seealso cref="NotifyCollectionChangedAction.Reset"/>, <seealso cref="NotifyCollectionChangedAction.Add"/>,
            /// or <seealso cref="NotifyCollectionChangedAction.Remove"/>.</param>
            /// <param name="changedItem">The item that is affected by the change.</param>
            /// <exception cref="ArgumentException">If <paramref name="action"/> is not <seealso cref="NotifyCollectionChangedAction.Reset"/>, <seealso cref="NotifyCollectionChangedAction.Add"/>,
            /// or <seealso cref="NotifyCollectionChangedAction.Remove"/>, or if <paramref name="action"/> is <seealso cref="NotifyCollectionChangedAction.Reset"/>, and <paramref name="changedItem"/> is not <c>null</c>.</exception>
            public CollectionChangedEventArgs(NotifyCollectionChangedAction action, TItem changedItem) : base(action, changedItem) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="CollectionChangedEventArgs"/> class that describes a multi-item change.
            /// </summary>
            /// <param name="action">The action that caused the event. This can be set to <seealso cref="NotifyCollectionChangedAction.Reset"/>, <seealso cref="NotifyCollectionChangedAction.Add"/>,
            /// or <seealso cref="NotifyCollectionChangedAction.Remove"/>.</param>
            /// <param name="changedItems">The items that are affected by the change.</param>
            public CollectionChangedEventArgs(NotifyCollectionChangedAction action, TItem[] changedItems) : base(action, changedItems) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="CollectionChangedEventArgs"/> class that describes a one-item change.
            /// </summary>
            /// <param name="action">The action that caused the event. This can be set to <seealso cref="NotifyCollectionChangedAction.Reset"/>, <seealso cref="NotifyCollectionChangedAction.Add"/>,
            /// or <seealso cref="NotifyCollectionChangedAction.Remove"/>.</param>
            /// <param name="changedItem">The item that is affected by the change.</param>
            /// <param name="index">The index where the change occurred.</param>
            /// <exception cref="ArgumentException">If <paramref name="action"/> is not <seealso cref="NotifyCollectionChangedAction.Reset"/>, <seealso cref="NotifyCollectionChangedAction.Add"/>,
            /// or <seealso cref="NotifyCollectionChangedAction.Remove"/>, or if <paramref name="action"/> is <seealso cref="NotifyCollectionChangedAction.Reset"/> and either <paramref name="changedItem"/> is not <c>null</c>
            /// or <paramref name="index"/> is not <c>-1</c>.</exception>
            public CollectionChangedEventArgs(NotifyCollectionChangedAction action, TItem changedItem, int index) : base(action, changedItem, index) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="CollectionChangedEventArgs"/> class that describes a multi-item change or a <seealso cref="NotifyCollectionChangedAction.Reset"/> change.
            /// </summary>
            /// <param name="action">The action that caused the event. This can be set to <seealso cref="NotifyCollectionChangedAction.Reset"/>, <seealso cref="NotifyCollectionChangedAction.Add"/>,
            /// or <seealso cref="NotifyCollectionChangedAction.Remove"/>.</param>
            /// <param name="changedItems">The items affected by the change.</param>
            /// <param name="startingIndex">The index where the change occurred.</param>
            /// <exception cref="ArgumentException">If <paramref name="action"/> is not <seealso cref="NotifyCollectionChangedAction.Reset"/>, <seealso cref="NotifyCollectionChangedAction.Add"/>,
            /// or <seealso cref="NotifyCollectionChangedAction.Remove"/>, if <paramref name="action"/> is <seealso cref="NotifyCollectionChangedAction.Reset"/>, and either <paramref name="changedItems"/> is not <c>null</c>
            /// or <paramref name="startingIndex"/> is not <c>-1</c>, or if <paramref name="action"/> is <seealso cref="NotifyCollectionChangedAction.Add"/> or <seealso cref="NotifyCollectionChangedAction.Remove"/>
            /// and <paramref name="startingIndex"/> is less than <c>-1</c>.</exception>
            /// <exception cref="ArgumentNullException">If <paramref name="action"/> is <seealso cref="NotifyCollectionChangedAction.Add"/> or <seealso cref="NotifyCollectionChangedAction.Remove"/>
            /// and <paramref name="changedItems"/> is <c>null</c>.</exception>
            public CollectionChangedEventArgs(NotifyCollectionChangedAction action, TItem[] changedItems, int startingIndex) : base(action, changedItems, startingIndex) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="CollectionChangedEventArgs"/> class that describes a one-item <seealso cref="NotifyCollectionChangedAction.Replace"/> change.
            /// </summary>
            /// <param name="action">The action that caused the event. This can only be set to <seealso cref="NotifyCollectionChangedAction.Replace"/>.</param>
            /// <param name="newItem">The new item that is replacing the original item.</param>
            /// <param name="oldItem">The original item that is replaced.</param>
            /// <exception cref="ArgumentException">If <paramref name="action"/> is not Replace.</exception>
            public CollectionChangedEventArgs(NotifyCollectionChangedAction action, TItem newItem, TItem oldItem) : base(action, newItem, oldItem) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="CollectionChangedEventArgs"/> class that describes a multi-item <seealso cref="NotifyCollectionChangedAction.Replace"/> change.
            /// </summary>
            /// <param name="action">The action that caused the event. This can only be set to <seealso cref="NotifyCollectionChangedAction.Replace"/>.</param>
            /// <param name="newItems">The new items that are replacing the original items.</param>
            /// <param name="oldItems">The original items that are replaced.</param>
            /// <exception cref="ArgumentException">If <paramref name="action"/> is not <seealso cref="NotifyCollectionChangedAction.Replace"/>.</exception>
            /// <exception cref="ArgumentNullException">If <paramref name="oldItems"/> or <paramref name="newItems"/> is <c>null</c>.</exception>
            public CollectionChangedEventArgs(NotifyCollectionChangedAction action, TItem[] newItems, TItem[] oldItems) : base(action, newItems, oldItems) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="CollectionChangedEventArgs"/> class that describes a one-item <seealso cref="NotifyCollectionChangedAction.Replace"/> change.
            /// </summary>
            /// <param name="action">The action that caused the event. This can be set to <seealso cref="NotifyCollectionChangedAction.Replace"/>.</param>
            /// <param name="newItem">The new item that is replacing the original item.</param>
            /// <param name="oldItem">The original item that is replaced.</param>
            /// <param name="index">The index of the item being replaced.</param>
            /// <exception cref="ArgumentException">If <paramref name="action"/> is not <seealso cref="NotifyCollectionChangedAction.Replace"/>.</exception>
            public CollectionChangedEventArgs(NotifyCollectionChangedAction action, TItem newItem, TItem oldItem, int index) : base(action, newItem, oldItem, index) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="CollectionChangedEventArgs"/> class that describes a multi-item <seealso cref="NotifyCollectionChangedAction.Replace"/> change.
            /// </summary>
            /// <param name="action">The action that caused the event. This can only be set to <seealso cref="NotifyCollectionChangedAction.Replace"/>.</param>
            /// <param name="newItems">The new items that are replacing the original items.</param>
            /// <param name="oldItems">The original items that are replaced.</param>
            /// <param name="startingIndex">The index of the first item of the items that are being replaced.</param>
            /// <exception cref="ArgumentException">If <paramref name="action"/> is not <seealso cref="NotifyCollectionChangedAction.Replace"/>.</exception>
            /// <exception cref="ArgumentNullException">If <paramref name="oldItems"/> or <paramref name="newItems"/> is <c>null</c>.</exception>
            public CollectionChangedEventArgs(NotifyCollectionChangedAction action, TItem[] newItems, TItem[] oldItems, int startingIndex) : base(action, newItems, oldItems, startingIndex) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="CollectionChangedEventArgs"/> class that describes a one-item <seealso cref="NotifyCollectionChangedAction.Move"/> change.
            /// </summary>
            /// <param name="action">The action that caused the event. This can only be set to <seealso cref="NotifyCollectionChangedAction.Move"/>.</param>
            /// <param name="changedItem">The item affected by the change.</param>
            /// <param name="index">The new index for the changed item.</param>
            /// <param name="oldIndex">The old index for the changed item.</param>
            /// <exception cref="ArgumentException">If <paramref name="action"/> is not <seealso cref="NotifyCollectionChangedAction.Move"/> or <paramref name="index"/> is less than <c>0</c>.</exception>
            public CollectionChangedEventArgs(NotifyCollectionChangedAction action, TItem changedItem, int index, int oldIndex) : base(action, changedItem, index, oldIndex) { }

            /// <summary>
            /// Initializes a new instance of the <see cref="CollectionChangedEventArgs"/> class that describes a multi-item <seealso cref="NotifyCollectionChangedAction.Move"/> change.
            /// </summary>
            /// <param name="action">The action that caused the event. This can only be set to <seealso cref="NotifyCollectionChangedAction.Move"/>.</param>
            /// <param name="changedItems">The items affected by the change.</param>
            /// <param name="index">The new index for the changed items.</param>
            /// <param name="oldIndex">The old index for the changed items.</param>
            /// <exception cref="ArgumentException">If <paramref name="action"/> is not <seealso cref="NotifyCollectionChangedAction.Move"/> or <paramref name="index"/> is less than <c>0</c>.</exception>
            public CollectionChangedEventArgs(NotifyCollectionChangedAction action, TItem[] changedItems, int index, int oldIndex) : base(action, changedItems, index, oldIndex) { }

            /// <summary>
            /// Gets the list of new items involved in the change.
            /// </summary>
            public new TItem[] NewItems => (TItem[])(base.NewItems);

            /// <summary>
            /// Gets the list of items affected by a <seealso cref="NotifyCollectionChangedAction.Replace"/>, <seealso cref="NotifyCollectionChangedAction.Remove"/>, or <seealso cref="NotifyCollectionChangedAction.Move"/> action.
            /// </summary>
            public new TItem[] OldItems => (TItem[])(base.OldItems);
        }

        public abstract class ItemNode
        {
            private TPrimary _primary = null;

            protected object SyncRoot { get; } = new object();

            /// <summary>
            /// Primary object in the one-to-many relationship.
            /// </summary>
            protected TPrimary BasePrimary
            {
                get => _primary;
                set
                {
                    throw new NotImplementedException();
                }
            }

            private void __OnAdded(TPrimary newPrimary, int index) => OnAdded(newPrimary, index);

            private void __OnRemoved(TPrimary oldPrimary, int index) => OnAdded(oldPrimary, index);

            private void __OnReplaced(TItem oldItem, TPrimary newPrimary, int index) => OnReplaced(oldItem, newPrimary, index);

            private void __OnReplacedBy(TItem newItem, TPrimary oldPrimary, int index) => OnReplacedBy(newItem, oldPrimary, index);

            /// <summary>
            /// This gets called whenever the current <typeparamref name="TItem"/> is associated with a <typeparamref name="TPrimary"/> object.
            /// </summary>
            /// <param name="newPrimary">New primary object in the one-to-many relationship.</param>
            /// <param name="index">Index at which the current <typeparamref name="TItem"/> was added into the collection of objects in the one-to-many relationship.</param>
            protected void OnAdded(TPrimary newPrimary, int index) { }

            /// <summary>
            /// This gets called whenver the current <typeparamref name="TItem"/> is disassociated from a <typeparamref name="TPrimary"/> object.
            /// </summary>
            /// <param name="oldPrimary">Primary object from the old one-to-many relationship.</param>
            /// <param name="index">Index at which the current <typeparamref name="TItem"/> was removed from the collection of objects in the one-to-many relationship.</param>
            protected void OnRemoved(TPrimary oldPrimary, int index) { }

            /// <summary>
            /// This gets called whenever the current <typeparamref name="TItem"/> replaced another in the collection of objects for a one-to-many relationship.
            /// </summary>
            /// <param name="oldItem">The <typeparamref name="TItem"/> that was replaced.</param>
            /// <param name="newPrimary">The primary object for the new one-to-many relationship.</param>
            /// <param name="index">Index at which the current <typeparamref name="TItem"/> replaced the <paramref name="oldItem"/>.</param>
            protected void OnReplaced(TItem oldItem, TPrimary newPrimary, int index) { }

            /// <summary>
            /// This gets called whenever the current <typeparamref name="TItem"/> is replaced by another in the collection of objects for a one-to-many relationship.
            /// </summary>
            /// <param name="newItem">The <typeparamref name="TItem"/> that has relaced the current object.</param>
            /// <param name="oldPrimary">The primary object for the old one-to-many relationship.</param>
            /// <param name="index">Index at which the <paramref name="newItem"/> replaced the current <typeparamref name="TItem"/>.</param>
            protected void OnReplacedBy(TItem newItem, TPrimary oldPrimary, int index) { }

            public class ItemCollection : IList<TItem>, IList, INotifyCollectionChanged
            {
                private TPrimary _owner;
                private readonly List<TItem> _innerList = new List<TItem>();
                private string _lockEvent = null;

                public event EventHandler<CollectionChangedEventArgs> CollectionChanged;

                private event NotifyCollectionChangedEventHandler InnerCollectionChanged;

                event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
                {
                    add => InnerCollectionChanged += value;
                    remove => InnerCollectionChanged -= value;
                }
                
                public TItem this[int index] { get => BaseGet(index); set => BaseSet(index, value); }

                object IList.this[int index] { get => BaseGet(index); set => BaseSet(index, (TItem)value); }

                public int Count => _innerList.Count;

                bool ICollection<TItem>.IsReadOnly => false;

                bool IList.IsReadOnly => false;

                bool IList.IsFixedSize => false;

                protected object SyncRoot { get; } = new object();

                object ICollection.SyncRoot => SyncRoot;

                bool ICollection.IsSynchronized => true;

                internal ItemCollection(TPrimary owner)
                {
                    _owner = owner;
                }

                public void Add(TItem item) => BaseAdd(item);

                int IList.Add(object value)
                {
                    try { return BaseAdd((TItem)value); }
                    catch (ArgumentNullException) { throw new ArgumentNullException("value"); }
                    catch (ArgumentException exception) { throw new ArgumentException(exception.Message, "value", exception); }
                }

                public int BaseAdd(TItem item)
                {
                    if (item == null)
                        throw new ArgumentNullException("item");
                    int index;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        Monitor.Enter(item.SyncRoot);
                        try
                        {
                            if (item._primary != null)
                            {
                                if (ReferenceEquals(item._primary, _owner))
                                    throw new ArgumentException("That item already exists in this collection", "item");
                                throw new ArgumentNullException("That item already exists in another collection", "item");
                            }
                            if (_innerList.Any(i => ReferenceEquals(i, item)))
                                if (_lockEvent != null)
                                    throw new InvalidOperationException("List cannot be modified during the " + _lockEvent + " event");
                            _lockEvent = "AddingItems";
                            index = _innerList.Count;
                            try { RaiseAddingItem(index, false, item); }
                            catch (Exception exception)
                            {
                                Exception exc = RaiseAddFailed(exception, index, false);
                                if (exc == null || ReferenceEquals(exc, exception))
                                    throw;
                                throw exc;
                            }
                            finally { _lockEvent = null; }
                            item._primary = _owner;
                            _innerList.Add(item);
                        }
                        finally { Monitor.Exit(item.SyncRoot); }
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    RaiseItemAdded(index, false, item);
                    return index;
                }

                protected TItem BaseGet(int index) => _innerList[index];

                protected void BaseSet(int index, TItem value)
                {
                    if (value == null)
                        throw new ArgumentNullException("value");
                    TItem oldItem;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        Monitor.Enter(value.SyncRoot);
                        try
                        {
                            oldItem = _innerList[index];
                            if (ReferenceEquals(value, oldItem))
                                return;
                            Monitor.Enter(oldItem.SyncRoot);
                            try
                            {
                                if (value._primary != null)
                                {
                                    if (ReferenceEquals(value._primary, _owner))
                                        throw new ArgumentException("That item already exists in this collection", "item");
                                    throw new ArgumentNullException("That item already exists in another collection", "item");
                                }
                                if (_lockEvent != null)
                                    throw new InvalidOperationException("List cannot be modified during the " + _lockEvent + " event");
                                _lockEvent = "ReplacingItem";
                                index = _innerList.Count;
                                try { OnReplacingItem(value, oldItem, index); }
                                catch (Exception exception)
                                {
                                    Exception exc = OnReplaceFailed(exception, value, oldItem, index);
                                    if (exc == null || ReferenceEquals(exc, exception))
                                        throw;
                                    throw exc;
                                }
                                finally { _lockEvent = null; }
                                value._primary = _owner;
                                oldItem._primary = null;
                                _innerList[index] = value;
                            }
                            finally { Monitor.Exit(oldItem.SyncRoot); }
                        }
                        finally { Monitor.Exit(value.SyncRoot); }
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    RaiseItemReplaced(value, oldItem, index);
                }
                
                public void Clear()
                {
                    TItem[] oldItems;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        if (_innerList.Count == 0)
                            return;
                        if (_lockEvent != null)
                            throw new InvalidOperationException("List cannot be modified during the " + _lockEvent + " event");
                        oldItems = _innerList.ToArray();
                        _lockEvent = "RemovingItems";
                        try { RaiseRemovingItem(0, oldItems); }
                        catch (Exception exception)
                        {
                            Exception exc = OnRemoveFailed(exception, oldItems, 0);
                            if (exc == null || ReferenceEquals(exc, exception))
                                throw;
                            throw exc;
                        }
                        finally { _lockEvent = null; }
                        foreach (TItem item in oldItems)
                            item._primary = null;
                        _innerList.Clear();
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    RaiseItemRemoved(0, oldItems);
                }

                public bool Contains(TItem item)
                {
                    if (item == null)
                        return false;
                    Monitor.Enter(item.SyncRoot);
                    try { return item._primary != null && ReferenceEquals(item._primary, item); }
                    finally { Monitor.Exit(item.SyncRoot); }
                }

                bool IList.Contains(object value) => value != null && value is TItem && Contains((TItem)value);

                public void CopyTo(TItem[] array, int arrayIndex) => _innerList.CopyTo(array, arrayIndex);

                void ICollection.CopyTo(Array array, int index) => _innerList.ToArray().CopyTo(array, index);

                public IEnumerator<TItem> GetEnumerator() => _innerList.GetEnumerator();

                IEnumerator IEnumerable.GetEnumerator() => _innerList.GetEnumerator();

                public int IndexOf(TItem item)
                {
                    if (item != null)
                    {
                        Monitor.Enter(item.SyncRoot);
                        try
                        {
                            if (item._primary != null && ReferenceEquals(item._primary, _owner))
                            {
                                for (int i = 0; i <  _innerList.Count; i++)
                                {
                                    if (ReferenceEquals(item, _innerList[i]))
                                        return i;
                                }
                            }
                        }
                        finally { Monitor.Exit(item.SyncRoot); }
                    }
                    return -1;
                }

                int IList.IndexOf(object value) => (value != null && value is TItem) ? IndexOf((TItem)value) : -1;
                
                public void Insert(int index, TItem item)
                {
                    if (item == null)
                        throw new ArgumentNullException("item");
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        Monitor.Enter(item.SyncRoot);
                        try
                        {
                            if (item._primary != null)
                            {
                                if (ReferenceEquals(item._primary, _owner))
                                    throw new ArgumentException("That item already exists in this collection", "item");
                                throw new ArgumentNullException("That item already exists in another collection", "item");
                            }
                            if (_lockEvent != null)
                                throw new InvalidOperationException("List cannot be modified during the " + _lockEvent + " event");
                            _lockEvent = "AddingItems";
                            try
                            {
                                RaiseAddingItem(index, true, item);
                                item._primary = _owner;
                                try { _innerList.Insert(index, item); }
                                catch
                                {
                                    item._primary = null;
                                    throw;
                                }
                            }
                            catch (Exception exception)
                            {
                                Exception exc = RaiseAddFailed(exception, index, true);
                                if (exc == null || ReferenceEquals(exc, exception))
                                    throw;
                                throw exc;
                            }
                            finally { _lockEvent = null; }
                        }
                        finally { Monitor.Exit(item.SyncRoot); }
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    RaiseItemAdded(index, true, item);
                }

                void IList.Insert(int index, object value)
                {
                    try { Insert(index, (TItem)value); }
                    catch (ArgumentNullException) { throw new ArgumentNullException("value"); }
                    catch (ArgumentException exc)
                    {
                        if (exc.ParamName == "index")
                            throw;
                        throw new ArgumentException(exc.Message, "value", exc);
                    }
                }

                protected virtual void OnAddingItems(TItem[] items, int index) { }

                protected virtual Exception OnAddFailed(Exception exception, TItem[] items, int index, bool isInsert) { return exception; }

                protected virtual void OnItemsAdded(TItem[] items, int index) { }

                protected virtual void OnRemovingItems(TItem[] items, int index) { }

                protected virtual Exception OnRemoveFailed(Exception exception, TItem[] items, int index) { return exception; }

                protected virtual void OnItemsRemoved(TItem[] items, int index) { }

                protected virtual void OnReplacingItem(TItem newItem, TItem oldItem, int index) { }

                protected virtual Exception OnReplaceFailed(Exception exception, TItem newItem, TItem oldItem, int index) { return exception; }

                protected virtual void OnItemReplaced(TItem newItem, TItem oldItem, int index) { }

                protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args) { }

                protected void RaiseAddingItem(int index, bool isInsert, params TItem[] items) => OnAddingItems(items, index);

                protected virtual Exception RaiseAddFailed(Exception exception, int index, bool isInsert, params TItem[] items) => OnAddFailed(exception, items, index, isInsert);

                protected void RaiseItemAdded(int index, bool isInsert, params TItem[] items)
                {
                    if (items == null || items.Length == 0)
                        return;
                    try { OnItemsAdded(items, index); }
                    finally
                    {
                        try { _owner.__OnItemsAdded(items, index, isInsert); }
                        finally
                        {

                            try
                            {
                                int i = 0;
                                try
                                {
                                    while (i < items.Length)
                                    {
                                        items[i].__OnAdded(_owner, index + i);
                                        i++;
                                    }
                                }
                                catch
                                {
                                    i++;
                                    while (i < items.Length)
                                    {
                                        try { items[i].__OnAdded(_owner, index + i); } catch { }
                                        i++;
                                    }
                                    throw;
                                }
                            }
                            finally
                            {
                                if (items.Length == 1)
                                    RaiseCollectionChanged(new CollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items[0], index));
                                else
                                    RaiseCollectionChanged(new CollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, index));
                            }
                        }
                    }
                }

                protected void RaiseRemovingItem(int index, params TItem[] items) => OnAddingItems(items, index);

                protected virtual Exception RaiseRemoveFailed(Exception exception, int index, params TItem[] items) => OnRemoveFailed(exception, items, index);

                protected void RaiseItemRemoved(int index, params TItem[] items)
                {
                    if (items == null || items.Length == 0)
                        return;
                    try { OnItemsRemoved(items, index); }
                    finally
                    {
                        try { _owner.__OnItemsRemoved(items, index); }
                        finally
                        {

                            try
                            {
                                int i = 0;
                                try
                                {
                                    while (i < items.Length)
                                    {
                                        items[i].__OnRemoved(_owner, index + i);
                                        i++;
                                    }
                                }
                                catch
                                {
                                    i++;
                                    while (i < items.Length)
                                    {
                                        try { items[i].__OnRemoved(_owner, index + i); } catch { }
                                        i++;
                                    }
                                    throw;
                                }
                            }
                            finally
                            {
                                if (items.Length == 1)
                                    RaiseCollectionChanged(new CollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items[0], index));
                                else
                                    RaiseCollectionChanged(new CollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items, index));
                            }
                        }
                    }
                }
                
                protected void RaiseItemReplaced(TItem newItem, TItem oldItem, int index)
                {
                    try { OnItemReplaced(newItem, oldItem, index); }
                    finally
                    {
                        try { _owner.__OnItemReplaced(newItem, oldItem, index); }
                        finally
                        {
                            try { newItem.__OnReplaced(oldItem, _owner, index); }
                            finally
                            {
                                try { oldItem.__OnReplacedBy(newItem, _owner, index); }
                                finally { RaiseCollectionChanged(new CollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index)); }
                            }
                        }
                    }
                }

                private void RaiseCollectionChanged(CollectionChangedEventArgs args)
                {
                    try { OnCollectionChanged(args); }
                    finally
                    {
                        try { _owner.__OnItemsChanged(args); }
                        finally { CollectionChanged?.Invoke(this, args); }
                    }
                }

                public bool Remove(TItem item)
                {
                    int index;
                    if (item == null)
                        return false;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        Monitor.Enter(item.SyncRoot);
                        try
                        {
                            index = IndexOf(item);
                            if (index < 0)
                                return false;
                            if (_lockEvent != null)
                                throw new InvalidOperationException("List cannot be modified during the " + _lockEvent + " event");
                            _lockEvent = "RemovingItems";
                            try
                            {
                                RaiseRemovingItem(index, item);
                                _innerList.RemoveAt(index);
                                item._primary = null;
                            }
                            catch (Exception exception)
                            {
                                Exception exc = RaiseRemoveFailed(exception, index, item);
                                if (exc == null || ReferenceEquals(exc, exception))
                                    throw;
                                throw exc;
                            }
                            finally { _lockEvent = null; }
                        }
                        finally { Monitor.Exit(item.SyncRoot); }
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    RaiseItemRemoved(index, item);
                    return true;
                }

                void IList.Remove(object value)
                {
                    if (value != null && value is TItem)
                        Remove((TItem)value);
                }

                public void RemoveAt(int index)
                {
                    TItem item;
                    Monitor.Enter(SyncRoot);
                    try
                    {
                        item = _innerList[index];
                        Monitor.Enter(item.SyncRoot);
                        try
                        {
                            if (_lockEvent != null)
                                throw new InvalidOperationException("List cannot be modified during the " + _lockEvent + " event");
                            _lockEvent = "RemovingItems";
                            try
                            {
                                RaiseRemovingItem(index, item);
                                item._primary = null;
                                try { _innerList.RemoveAt(index); }
                                catch
                                {
                                    item._primary = _owner;
                                    throw;
                                }
                            }
                            catch (Exception exception)
                            {
                                Exception exc = RaiseRemoveFailed(exception, index, item);
                                if (exc == null || ReferenceEquals(exc, exception))
                                    throw;
                                throw exc;
                            }
                            finally { _lockEvent = null; }
                        }
                        finally { Monitor.Exit(item.SyncRoot); }
                    }
                    finally { Monitor.Exit(SyncRoot); }
                    RaiseItemRemoved(index, item);
                }
            }
        }

        public OneToMany()
        {

        }
    }
}
