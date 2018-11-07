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
    public static class ThreadingExtensions
    {
        public static void RunSynchronized(ICollection list, Action action)
        {
            object syncRoot;
            if (list != null && list.IsSynchronized && (syncRoot = list.SyncRoot) != null)
            {
                Monitor.Enter(syncRoot);
                try { action(); }
                finally { Monitor.Exit(syncRoot); }
            }
            else
                action();
        }

        public static void RunSynchronized(IDictionary dictionary, Action action)
        {
            object syncRoot;
            if (dictionary != null && dictionary.IsSynchronized && (syncRoot = dictionary.SyncRoot) != null)
            {
                Monitor.Enter(syncRoot);
                try { action(); }
                finally { Monitor.Exit(syncRoot); }
            }
            else
                action();
        }

        public static void RunSynchronized(IThreadSync obj, Action action)
        {
            object syncRoot;
            if (obj != null && obj.IsSynchronized && (syncRoot = obj.SyncRoot) != null)
            {
                Monitor.Enter(syncRoot);
                try { action(); }
                finally { Monitor.Exit(syncRoot); }
            }
            else
                action();
        }

        public static T GetSynchronized<T>(ICollection list, Func<T> func)
        {
            object syncRoot;
            if (list != null && list.IsSynchronized && (syncRoot = list.SyncRoot) != null)
            {
                Monitor.Enter(syncRoot);
                try { return func(); }
                finally { Monitor.Exit(syncRoot); }
            }

            return func();
        }

        public static T GetSynchronized<T>(IDictionary dictionary, Func<T> func)
        {
            object syncRoot;
            if (dictionary != null && dictionary.IsSynchronized && (syncRoot = dictionary.SyncRoot) != null)
            {
                Monitor.Enter(syncRoot);
                try { return func(); }
                finally { Monitor.Exit(syncRoot); }
            }

            return func();
        }

        public static T GetSynchronized<T>(IThreadSync obj, Func<T> func)
        {
            object syncRoot;
            if (obj != null && obj.IsSynchronized && (syncRoot = obj.SyncRoot) != null)
            {
                Monitor.Enter(syncRoot);
                try { return func(); }
                finally { Monitor.Exit(syncRoot); }
            }
            
            return func();
        }
    }
}
