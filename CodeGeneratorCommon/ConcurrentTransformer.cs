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
    public class ConcurrentTransformer<TIn, TOut>
    {
        public IProducerConsumerCollection<TIn> Source { get; }

        public IProducerConsumerCollection<TIn> Target { get; }

        public static ConcurrentTransformer<TIn, TOut> StartNew(BlockingCollection<TIn> source, BlockingCollection<TOut> target, params ConcurrentTransformWorker[] workers)
        {
            throw new NotImplementedException();
        }

        public static ConcurrentTransformer<TIn, TOut> StartNew<TSource, TTarget>(TSource source, WaitHandle isAddingCompletedEvent, TTarget target, params ConcurrentTransformWorker[] workers)
            where TSource : IProducerConsumerCollection<TIn>
            where TTarget : IProducerConsumerCollection<TIn>
        {
            throw new NotImplementedException();
        }

        public static ConcurrentTransformer<TIn, TOut> StartNew<TSource>(TSource source, WaitHandle isAddingCompletedEvent, BlockingCollection<TOut> target, params ConcurrentTransformWorker[] workers)
            where TSource : IProducerConsumerCollection<TIn>
        {
            throw new NotImplementedException();
        }

        public static ConcurrentTransformer<TIn, TOut> StartNew<TTarget>(BlockingCollection<TIn> source,TTarget target, params ConcurrentTransformWorker[] workers)
            where TTarget : IProducerConsumerCollection<TIn>
        {
            throw new NotImplementedException();
        }

        public static ConcurrentTransformer<TIn, TOut> StartNew(BlockingCollection<TIn> source, out BlockingCollection<TOut> target, params ConcurrentTransformWorker[] workers)
        {
            throw new NotImplementedException();
        }

        public static ConcurrentTransformer<TIn, TOut> StartNew<TSource>(TSource source, WaitHandle isAddingCompletedEvent, out BlockingCollection<TOut> target, params ConcurrentTransformWorker[] workers)
            where TSource : IProducerConsumerCollection<TIn>
        {
            throw new NotImplementedException();
        }
    }

    public class BlockingCollectionTransformer<TIn, TSource, TOut> : ConcurrentTransformer<TIn, TOut>
        where TSource : BlockingCollection<TIn>
    {

    }

    public class ProducerConsumerTransformer<TIn, TSource, TOut> : ConcurrentTransformer<TIn, TOut>
        where TSource : IProducerConsumerCollection<TOut>
    {

    }
}
