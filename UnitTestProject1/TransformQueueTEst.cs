using System;
using System.Threading;
using CodeGeneratorCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class TransformQueueTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (Queueable<int> inputQueue = new Queueable<int>())
            {
            }
        }
    }

    public class TestQueue<T> : Queueable<T>
    {
        protected override void OnQueueLengthChange(int length)
        {
            base.OnQueueLengthChange(length);
        }
    }

    public class TestTransformQueue : TransformQueue<int, double>
    {
        public TestTransformQueue(Queueable<int> inputQueue, Queueable<double> outputQueue) : base(inputQueue, outputQueue)
        {

        }
    }

    public class TestTransformWorker : TransformQueue<int, double>.Worker
    {
        ManualResetEvent _mre1;
        ManualResetEvent _mre2;
        public TestTransformWorker(ManualResetEvent mre1, ManualResetEvent mre2)
        {
            _mre1 = mre1;
            _mre2 = mre2;
        }
        protected override bool ProcessInput(int current, int position)
        {
            throw new NotImplementedException();
        }

        protected override void BeforeStartNewQueue()
        {
            base.BeforeStartNewQueue();
        }

        protected override void AfterLastItemProcessed()
        {
            base.AfterLastItemProcessed();
        }
    }
}
