using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSCodeGenerator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;

namespace PSCodeGeneratorTest
{
    [TestClass]
    public class AssemblyInfoCollectionUnitTest
    {
        private const string AssembyName_MsCorLib = "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        private const string AssembyName_System = "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
        private const string AssembyName_Automation = "System.Management.Automation, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
        private const string AssembyName_NullToken = "NullToken, Version=1.0.0.0, Culture=en-US, PublicKeyToken=null";
        private const string AssembyName_AltNullToken = "NullToken, Version=1.0.0.1, Culture=en-US, PublicKeyToken=null";
        private const string AssembyName_NoCulture = "NoCulture, Version=1.0.0.0, PublicKeyToken=null";
        private const string AssembyName_NoVersion = "NoVersion, Culture=en-US, PublicKeyToken=null";
        private const string AssembyName_NameOnly = "Name.Only";
        private static readonly ReadOnlyCollection<string> NormalAssemblyNameStrings = new ReadOnlyCollection<string>(new string[]
        {
            AssembyName_MsCorLib,
            AssembyName_System,
            AssembyName_Automation,
            AssembyName_NullToken,
            AssembyName_AltNullToken
        });

        private static readonly ReadOnlyCollection<string> PartialAssemblyNameStrings = new ReadOnlyCollection<string>(new string[]
        {
            AssembyName_NoCulture,
            AssembyName_NoVersion,
            AssembyName_NameOnly
        });

        private static List<AssemblyName> GetTestAssemblyNames(IEnumerable<string> collection) => collection.Select(n => new AssemblyName(n)).ToList();

        private static List<AssemblyName> GetTestAssemblyNames(bool includePartial) => GetTestAssemblyNames((includePartial) ? NormalAssemblyNameStrings.Concat(PartialAssemblyNameStrings) : NormalAssemblyNameStrings);

        private static List<AssemblyInfo> GetTestAssemblyInfos(IEnumerable<AssemblyName> collection) => collection.Select(n => new AssemblyInfo(n)).ToList();

        private static List<AssemblyInfo> GetTestAssemblyInfos(bool includePartial) => GetTestAssemblyInfos(GetTestAssemblyNames(includePartial));

        private static AssemblyInfoCollection CreateTestCollection(bool includePartial) => CreateTestCollection(GetTestAssemblyInfos(includePartial));

        private static AssemblyInfoCollection CreateTestCollection(IEnumerable<AssemblyInfo> collection)
        {
            AssemblyInfoCollection target = new AssemblyInfoCollection();
            foreach (AssemblyInfo a in collection)
                target.Add(a);
            return target;
        }

        [TestMethod]
        public void IndexerTest1()
        {
            List<AssemblyInfo> list = GetTestAssemblyInfos(false);
            AssemblyInfo[] expected = list.ToArray();
            AssemblyInfoCollection target = CreateTestCollection(list);
            for (int index = 0; index < expected.Length; index++)
            {
                AssemblyInfo actual = target[index];
                Assert.IsNotNull(actual);
                Assert.AreSame(expected[index], actual);
            }
            list = GetTestAssemblyInfos(GetTestAssemblyNames(PartialAssemblyNameStrings));
            for (int index = 0; index < list.Count; index++)
            {
                expected[index + 1] = list[index];
                target[index + 1] = list[index];
            }
            for (int index = 0; index < expected.Length; index++)
            {
                AssemblyInfo actual = target[index];
                Assert.IsNotNull(actual);
                Assert.AreSame(expected[index], actual);
            }
            for (int index = 0; index < list.Count; index++)
            {
                expected[index] = new AssemblyInfo(list[index].FullName);
                target[index] = expected[index];
            }
            for (int index = 0; index < expected.Length; index++)
            {
                AssemblyInfo actual = target[index];
                Assert.IsNotNull(actual);
                Assert.AreSame(expected[index], actual);
            }

            for (int n = 0; n < expected.Length; n++)
            {
                try { target[n] = null; }
                catch (ArgumentNullException)
                {
                    for (int index = 0; index < expected.Length; index++)
                    {
                        AssemblyInfo actual = target[index];
                        Assert.IsNotNull(actual);
                        Assert.AreSame(expected[index], actual);
                    }
                }
            }

            for (int n = 0; n < expected.Length; n++)
            {
                try { target[n] = target[(n == 0) ? expected.Length - 1 : n - 1]; }
                catch (ArgumentNullException)
                {
                    for (int index = 0; index < expected.Length; index++)
                    {
                        AssemblyInfo actual = target[index];
                        Assert.IsNotNull(actual);
                        Assert.AreSame(expected[index], actual);
                    }
                }
            }

            for (int n = 0; n < expected.Length; n++)
            {
                try { target[n] = new AssemblyInfo(target[(n == 0) ? expected.Length - 1 : n - 1].FullName); }
                catch (ArgumentNullException)
                {
                    for (int index = 0; index < expected.Length; index++)
                    {
                        AssemblyInfo actual = target[index];
                        Assert.IsNotNull(actual);
                        Assert.AreSame(expected[index], actual);
                    }
                }
            }
        }

        [TestMethod]
        public void IndexerTest2()
        {
            List<AssemblyInfo> list = GetTestAssemblyInfos(false);
            object[] expected = list.ToArray();
            IList target = CreateTestCollection(list);
            for (int index = 0; index < expected.Length; index++)
            {
                object actual = target[index];
                Assert.IsNotNull(actual);
                Assert.AreSame(expected[index], actual);
            }
            list = GetTestAssemblyInfos(GetTestAssemblyNames(PartialAssemblyNameStrings));
            for (int index = 0; index < list.Count; index++)
            {
                expected[index + 1] = list[index];
                target[index + 1] = list[index];
            }
            for (int index = 0; index < expected.Length; index++)
            {
                object actual = target[index];
                Assert.IsNotNull(actual);
                Assert.AreSame(expected[index], actual);
            }
            for (int index = 0; index < list.Count; index++)
            {
                expected[index] = new AssemblyInfo(list[index].FullName);
                target[index] = expected[index];
            }
            for (int index = 0; index < expected.Length; index++)
            {
                object actual = target[index];
                Assert.IsNotNull(actual);
                Assert.AreSame(expected[index], actual);
            }

            for (int n = 0; n < expected.Length; n++)
            {
                try { target[n] = null; }
                catch (ArgumentNullException)
                {
                    for (int index = 0; index < expected.Length; index++)
                    {
                        object actual = target[index];
                        Assert.IsNotNull(actual);
                        Assert.AreSame(expected[index], actual);
                    }
                }
            }

            for (int n = 0; n < expected.Length; n++)
            {
                try { target[n] = target[(n == 0) ? expected.Length - 1 : n - 1]; }
                catch (ArgumentNullException)
                {
                    for (int index = 0; index < expected.Length; index++)
                    {
                        object actual = target[index];
                        Assert.IsNotNull(actual);
                        Assert.AreSame(expected[index], actual);
                    }
                }
            }

            for (int n = 0; n < expected.Length; n++)
            {
                try { target[n] = new AssemblyInfo(((AssemblyInfo)(target[(n == 0) ? expected.Length - 1 : n - 1])).FullName); }
                catch (ArgumentNullException)
                {
                    for (int index = 0; index < expected.Length; index++)
                    {
                        object actual = target[index];
                        Assert.IsNotNull(actual);
                        Assert.AreSame(expected[index], actual);
                    }
                }
            }
        }

        [TestMethod]
        public void CountTest()
        {
            AssemblyInfoCollection target = new AssemblyInfoCollection();
            int expected = 0;
            int actual = target.Count;
            Assert.AreEqual(expected, actual);

            List<AssemblyInfo> list = GetTestAssemblyInfos(true);
            expected = list.Count;
            target = CreateTestCollection(list);
            actual = target.Count;
            Assert.AreEqual(expected, actual);

            list = GetTestAssemblyInfos(false);
            expected = list.Count;
            target = CreateTestCollection(list);
            actual = target.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsFixedSizeTest()
        {
            IList target = CreateTestCollection(true);
            bool actual = target.IsFixedSize;
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsReadOnlyTest1()
        {
            IList target = CreateTestCollection(true);
            bool actual = target.IsReadOnly;
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsReadOnlyTest2()
        {
            ICollection<AssemblyInfo> target = CreateTestCollection(true);
            bool actual = target.IsReadOnly;
            bool expected = false;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsSynchronizedTest()
        {
            ICollection target = CreateTestCollection(true);
            bool actual = target.IsSynchronized;
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SyncRootTest()
        {
            ICollection target = CreateTestCollection(true);
            object actual = target.SyncRoot;
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.GetType().IsClass);
            Assert.AreNotSame(target, actual);
        }

        [TestMethod]
        public void AddTest1()
        {
            List<AssemblyInfo> list = GetTestAssemblyInfos(true);
            AssemblyInfoCollection target = new AssemblyInfoCollection();
            int actual;
            for (int count = 0; count < list.Count; count++)
            {
                actual = target.Count;
                Assert.AreEqual(count, actual);
                target.Add(list[count]);
                for (int i = 0; i <= count; i++)
                {
                    AssemblyInfo item = target[i];
                    Assert.IsNotNull(item);
                    Assert.AreSame(list[i], item);
                }
            }
            int expected = list.Count;
            actual = target.Count;
            Assert.AreEqual(expected, actual);

            try { target.Add(null); }
            catch (ArgumentNullException)
            {
                actual = target.Count;
                Assert.AreEqual(expected, actual);
                for (int index = 0; index < expected; index++)
                {
                    AssemblyInfo item = target[index];
                    Assert.IsNotNull(item);
                    Assert.AreSame(list[index], item);
                }
            }

            try { target.Add(list[2]); }
            catch (ArgumentNullException) { throw; }
            catch (ArgumentException)
            {
                actual = target.Count;
                Assert.AreEqual(expected, actual);
                for (int index = 0; index < expected; index++)
                {
                    AssemblyInfo item = target[index];
                    Assert.IsNotNull(item);
                    Assert.AreSame(list[index], item);
                }
            }

            try { target.Add(new AssemblyInfo(list[0].FullName)); }
            catch (ArgumentNullException) { throw; }
            catch (ArgumentException)
            {
                actual = target.Count;
                Assert.AreEqual(expected, actual);
                for (int index = 0; index < expected; index++)
                {
                    AssemblyInfo item = target[index];
                    Assert.IsNotNull(item);
                    Assert.AreSame(list[index], item);
                }
            }
        }

        [TestMethod]
        public void AddTest2()
        {
            List<AssemblyInfo> list = GetTestAssemblyInfos(true);
            IList target = new AssemblyInfoCollection();
            int actual;
            for (int count = 0; count < list.Count; count++)
            {
                actual = target.Count;
                Assert.AreEqual(count, actual);
                object item = list[count];
                target.Add(item);
                for (int i = 0; i <= count; i++)
                {
                    item = target[i];
                    Assert.IsNotNull(item);
                    Assert.AreSame(list[i], item);
                }
            }
            int expected = list.Count;
            actual = target.Count;
            Assert.AreEqual(expected, actual);

            try { target.Add(null); }
            catch (ArgumentNullException)
            {
                actual = target.Count;
                Assert.AreEqual(expected, actual);
                for (int index = 0; index < expected; index++)
                {
                    object item = target[index];
                    Assert.IsNotNull(item);
                    Assert.AreSame(list[index], item);
                }
            }

            try { target.Add(list[2]); }
            catch (ArgumentNullException) { throw; }
            catch (ArgumentException)
            {
                actual = target.Count;
                Assert.AreEqual(expected, actual);
                for (int index = 0; index < expected; index++)
                {
                    object item = target[index];
                    Assert.IsNotNull(item);
                    Assert.AreSame(list[index], item);
                }
            }

            try { target.Add(new AssemblyInfo(list[0].FullName)); }
            catch (ArgumentNullException) { throw; }
            catch (ArgumentException)
            {
                actual = target.Count;
                Assert.AreEqual(expected, actual);
                for (int index = 0; index < expected; index++)
                {
                    object item = target[index];
                    Assert.IsNotNull(item);
                    Assert.AreSame(list[index], item);
                }
            }

            try { target.Add(new AssemblyName(list[0].FullName)); }
            catch (InvalidCastException)
            {
                actual = target.Count;
                Assert.AreEqual(expected, actual);
                for (int index = 0; index < expected; index++)
                {
                    object item = target[index];
                    Assert.IsNotNull(item);
                    Assert.AreSame(list[index], item);
                }
            }

            try { target.Add((typeof(string)).Assembly); }
            catch (InvalidCastException)
            {
                actual = target.Count;
                Assert.AreEqual(expected, actual);
                for (int index = 0; index < expected; index++)
                {
                    object item = target[index];
                    Assert.IsNotNull(item);
                    Assert.AreSame(list[index], item);
                }
            }

            try { target.Add(list[0].FullName); }
            catch (InvalidCastException)
            {
                actual = target.Count;
                Assert.AreEqual(expected, actual);
                for (int index = 0; index < expected; index++)
                {
                    object item = target[index];
                    Assert.IsNotNull(item);
                    Assert.AreSame(list[index], item);
                }
            }
        }

        [TestMethod]
        public void AddRangeTest()
        {
            List<AssemblyInfo> collection = GetTestAssemblyInfos(true);
            AssemblyInfo[] items = collection.ToArray();
            int expected = collection.Count;
            AssemblyInfoCollection target = CreateTestCollection(collection);
            collection = GetTestAssemblyInfos(GetTestAssemblyNames(PartialAssemblyNameStrings));
            expected += collection.Count;
            target.AddRange(collection);
            Assert.AreEqual(expected, target.Count);
            for (int i = 0; i < items.Length; i++)
            {
                AssemblyInfo actual = target[i];
                Assert.IsNotNull(actual);
                Assert.AreSame(items[i], actual);
            }
            for (int i = 0; i < collection.Count; i++)
            {
                AssemblyInfo actual = target[items.Length + i];
                Assert.IsNotNull(actual);
                Assert.AreSame(collection[i], actual);
            }
            collection[1] = null;
            try { target.AddRange(collection); }
            catch (ArgumentNullException) { throw; }
            catch (ArgumentException)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    AssemblyInfo actual = target[i];
                    Assert.IsNotNull(actual);
                    Assert.AreSame(items[i], actual);
                }
                for (int i = 0; i < collection.Count; i++)
                {
                    AssemblyInfo actual = target[items.Length + i];
                    Assert.IsNotNull(actual);
                    Assert.AreSame(collection[i], actual);
                }
            }
            collection[1] = items[4];
            target.Clear();
            try { target.AddRange(collection); }
            catch (ArgumentNullException) { throw; }
            catch (ArgumentException)
            {
                Assert.AreEqual(4, target.Count);
                for (int i = 0; i < 4; i++)
                {
                    AssemblyInfo actual = target[i];
                    Assert.IsNotNull(actual);
                    Assert.AreSame(collection[i], actual);
                }
            }
        }

        [TestMethod]
        public void ClearTest()
        {
            AssemblyInfoCollection target = CreateTestCollection(true);
            target.Clear();
            int expected = 0;
            int actual = target.Count;
            Assert.AreEqual(expected, actual);
            target.Clear();
            actual = target.Count;
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void ContainsTest1()
        {
            List<AssemblyInfo> list = GetTestAssemblyInfos(false);
            AssemblyInfoCollection target = CreateTestCollection(list);
            AssemblyInfo item = null;
            bool expected = false;
            bool actual = target.Contains(item);
            Assert.AreEqual(expected, actual);
            foreach (AssemblyInfo i in GetTestAssemblyInfos(GetTestAssemblyNames(PartialAssemblyNameStrings)))
            {
                actual = target.Contains(i);
                Assert.AreEqual(expected, actual);
            }
            expected = true;
            foreach (AssemblyInfo i in list)
            {
                actual = target.Contains(i);
                Assert.AreEqual(expected, actual);
                actual = target.Contains(new AssemblyInfo(i.FullName));
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void ContainsTest2()
        {
            List<AssemblyInfo> list = GetTestAssemblyInfos(false);
            IList target = CreateTestCollection(true);
            object value = null;
            bool actual = target.Contains(value);
            bool expected = false;
            Assert.AreEqual(expected, actual);
            foreach (AssemblyInfo i in GetTestAssemblyInfos(GetTestAssemblyNames(PartialAssemblyNameStrings)))
            {
                actual = target.Contains(i);
                Assert.AreEqual(expected, actual);
            }
            foreach (AssemblyInfo i in list)
            {
                actual = target.Contains(i.FullName);
                Assert.AreEqual(expected, actual);
                actual = target.Contains(new AssemblyName(i.FullName));
                Assert.AreEqual(expected, actual);
            }
            expected = true;
            foreach (AssemblyInfo i in list)
            {
                actual = target.Contains(i);
                Assert.AreEqual(expected, actual);
                actual = target.Contains(new AssemblyInfo(i.FullName));
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void CopyToTest1()
        {
            List<AssemblyInfo> list = GetTestAssemblyInfos(true);
            AssemblyInfoCollection target = CreateTestCollection(list);
            AssemblyInfo[] array = new AssemblyInfo[list.Count];
            int arrayIndex = 0;
            target.CopyTo(array, arrayIndex);
            for (int i = 0; i < list.Count; i++)
            {
                AssemblyInfo item = array[i];
                Assert.IsNotNull(item);
                Assert.AreSame(target[i], item);
            }
        }

        [TestMethod]
        public void CopyToTest2()
        {
            List<AssemblyInfo> list = GetTestAssemblyInfos(true);
            ICollection target = CreateTestCollection(list);
            Array array = new object[list.Count];
            int index = 0;
            target.CopyTo(array, index);
            foreach (object obj in target)
            {
                object item = ((object[])array)[index];
                Assert.IsNotNull(item);
                Assert.AreSame(obj, item);
                index++;
            }
        }

        [TestMethod]
        public void ExistsTest()
        {
            Predicate<AssemblyInfo> match = (a) => a.ProcessorArchitecture == ProcessorArchitecture.None;
            AssemblyInfoCollection target = CreateTestCollection(true);
            bool actual = target.Exists(match);
            Assert.Inconclusive("ExistsTest not implemented");
        }

        [TestMethod]
        public void FindTest()
        {
            Predicate<AssemblyInfo> match = (a) => a.ProcessorArchitecture == ProcessorArchitecture.None;
            AssemblyInfoCollection target = CreateTestCollection(true);
            AssemblyInfo actual = target.Find(match);
            Assert.Inconclusive("FindTest not implemented");
        }


        [TestMethod]
        public void FindAllTest()
        {
            Predicate<AssemblyInfo> match = (a) => a.ProcessorArchitecture == ProcessorArchitecture.None;
            AssemblyInfoCollection target = CreateTestCollection(true);
            Collection<AssemblyInfo> actual = target.FindAll(match);
            Assert.Inconclusive("FindAllTest not implemented");
        }

        [TestMethod]
        public void FindIndexTest1()
        {
            int startIndex = 0;
            int count = 0;
            Predicate<AssemblyInfo> match = (a) => a.ProcessorArchitecture == ProcessorArchitecture.None;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.FindIndex(startIndex, count, match);
            Assert.Inconclusive("FindIndexTest1 not implemented");
        }

        [TestMethod]
        public void FindIndexTest2()
        {
            int startIndex = 0;
            Predicate<AssemblyInfo> match = (a) => a.ProcessorArchitecture == ProcessorArchitecture.None;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.FindIndex(startIndex, match);
            Assert.Inconclusive("FindIndexTest2 not implemented");
        }

        [TestMethod]
        public void FindIndexTest3()
        {
            Predicate<AssemblyInfo> match = (a) => a.ProcessorArchitecture == ProcessorArchitecture.None;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.FindIndex(match);
            Assert.Inconclusive("FindIndexTest3 not implemented");
        }


        [TestMethod]
        public void FindLastTest()
        {
            Predicate<AssemblyInfo> match = (a) => a.ProcessorArchitecture == ProcessorArchitecture.None;
            AssemblyInfoCollection target = CreateTestCollection(true);
            AssemblyInfo actual = target.FindLast(match);
            Assert.Inconclusive("FindLastTest not implemented");
        }

        [TestMethod]
        public void FindLastIndexTest1()
        {
            int startIndex = 0;
            int count = 0;
            Predicate<AssemblyInfo> match = (a) => a.ProcessorArchitecture == ProcessorArchitecture.None;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.FindLastIndex(startIndex, count, match);
            Assert.Inconclusive("FindLastIndexTest1 not implemented");
        }

        [TestMethod]
        public void FindLastIndexTest2()
        {
            int startIndex = 0;
            Predicate<AssemblyInfo> match = (a) => a.ProcessorArchitecture == ProcessorArchitecture.None;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.FindLastIndex(startIndex, match);
            Assert.Inconclusive("FindLastIndexTest2 not implemented");
        }

        [TestMethod]
        public void FindLastIndexTest3()
        {
            Predicate<AssemblyInfo> match = (a) => a.ProcessorArchitecture == ProcessorArchitecture.None;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.FindLastIndex(match);
            Assert.Inconclusive("FindLastIndexTest3 not implemented");
        }

        [TestMethod]
        public void ForEachTest()
        {
            List<string> output = new List<string>();
            Action<AssemblyInfo> action = (a) => output.Add(a.FullName);
            AssemblyInfoCollection target = CreateTestCollection(true);
            target.ForEach(action);
            Assert.Inconclusive("ForEachTest not implemented");
        }

        [TestMethod]
        public void GetEnumeratorTest1()
        {
            List<string> output = new List<string>();
            AssemblyInfoCollection target = CreateTestCollection(true);
            IEnumerator<AssemblyInfo> actual = target.GetEnumerator();
            Assert.IsNotNull(actual);
            using (actual)
            {
                while (actual.MoveNext())
                {
                    Assert.IsNotNull(actual.Current);
                    output.Add(actual.Current.FullName);
                }
            }
            Assert.Inconclusive("GetEnumeratorTest1 not implemented");
        }

        [TestMethod]
        public void GetEnumeratorTest2()
        {
            IEnumerable target = CreateTestCollection(true);
            IEnumerator actual = target.GetEnumerator();
            Assert.Inconclusive("GetEnumeratorTest2 not implemented");
        }

        [TestMethod]
        public void GetRangeTest()
        {
            int index = 0;
            int count = 0;
            AssemblyInfoCollection target = CreateTestCollection(true);
            Collection<AssemblyInfo> actual = target.GetRange(index, count);
            Assert.Inconclusive("GetRangeTest not implemented");
        }
        
        [TestMethod]
        public void IndexOfTest1()
        {
            AssemblyInfo item = null;
            int index = 0;
            int count = 0;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.IndexOf(item, index, count);
            Assert.Inconclusive("IndexOfTest1 not implemented");
        }

        [TestMethod]
        public void IndexOfTest2()
        {
            AssemblyInfo item = null;
            int index = 0;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.IndexOf(item, index);
            Assert.Inconclusive("IndexOfTest2not implemented");
        }

        [TestMethod]
        public void IndexOfTest3()
        {
            AssemblyInfo item = null;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.IndexOf(item);
            Assert.Inconclusive("IndexOfTest3 not implemented");
        }

        [TestMethod]
        public void IndexOfTest4()
        {
            object value = null;
            IList target = CreateTestCollection(true);
            int actual = target.IndexOf(value);
            Assert.Inconclusive("IndexOfTest4 not implemented");
        }

        [TestMethod]
        public void InsertTest1()
        {
            int index = 0;
            AssemblyInfo item = null;
            AssemblyInfoCollection target = CreateTestCollection(true);
            target.Insert(index, item);
            Assert.Inconclusive("InsertTest1 not implemented");
        }

        [TestMethod]
        public void InsertTest2()
        {
            int index = 0;
            object value = null;
            IList target = CreateTestCollection(true);
            Assert.Inconclusive(" not implemented");
        }

        [TestMethod]
        public void LastIndexOfTest1()
        {
            AssemblyInfo item = null;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.LastIndexOf(item);
            Assert.Inconclusive("InsertTest2 not implemented");
        }

        [TestMethod]
        public void LastIndexOfTest2()
        {
            AssemblyInfo item = null;
            int index = 0;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.LastIndexOf(item, index);
            Assert.Inconclusive("LastIndexOfTest2 not implemented");
        }

        [TestMethod]
        public void LastIndexOfTest3()
        {
            AssemblyInfo item = null;
            int index = 0;
            int count = 0;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.LastIndexOf(item, index, count);
            Assert.Inconclusive("LastIndexOfTest3 not implemented");
        }

        [TestMethod]
        public void RemoveTest1()
        {
            AssemblyInfo item = null;
            AssemblyInfoCollection target = CreateTestCollection(true);
            bool actual = target.Remove(item);
            Assert.Inconclusive("RemoveTest1 not implemented");
        }

        [TestMethod]
        public void RemoveTest2()
        {
            object value = null;
            IList target = CreateTestCollection(true);
            Assert.Inconclusive("RemoveTest2 not implemented");
        }

        [TestMethod]
        public void RemoveAllTest()
        {
            Predicate<AssemblyInfo> match = null;
            AssemblyInfoCollection target = CreateTestCollection(true);
            int actual = target.RemoveAll(match);
            Assert.Inconclusive("RemoveAllTest not implemented");
        }

        [TestMethod]
        public void RemoveAtTest()
        {
            int index = 0;
            AssemblyInfoCollection target = CreateTestCollection(true);
            target.RemoveAt(index);
            Assert.Inconclusive("RemoveAtTest not implemented");
        }

        [TestMethod]
        public void RemoveRangeTest()
        {
            int index = 0;
            int count = 0;
            AssemblyInfoCollection target = CreateTestCollection(true);
            target.RemoveRange(index, count);
            Assert.Inconclusive("RemoveRangeTest not implemented");
        }

        [TestMethod]
        public void ReverseTest1()
        {
            int index = 0;
            int count = 0;
            AssemblyInfoCollection target = CreateTestCollection(true);
            target.Reverse(index, count);
            Assert.Inconclusive("ReverseTest1 not implemented");
        }

        [TestMethod]
        public void ReverseTest2()
        {
            AssemblyInfoCollection target = CreateTestCollection(true);
            target.Reverse();
            Assert.Inconclusive("ReverseTest2 not implemented");
        }

        [TestMethod]
        public void SortTest1()
        {
            Comparison<AssemblyInfo> comparison = null;
            AssemblyInfoCollection target = CreateTestCollection(true);
            target.Sort(comparison);
            Assert.Inconclusive("SortTest1 not implemented");
        }

        [TestMethod]
        public void SortTest2()
        {
            int index = 0;
            int count = 0;
            IComparer<AssemblyInfo> comparer = null;
            AssemblyInfoCollection target = CreateTestCollection(true);
            target.Sort(index, count, comparer);
            Assert.Inconclusive("SortTest2 not implemented");
        }

        [TestMethod]
        public void SortTest3()
        {
            IComparer<AssemblyInfo> comparer = null;
            AssemblyInfoCollection target = CreateTestCollection(true);
            target.Sort(comparer);
            Assert.Inconclusive("SortTest4 not implemented");
        }

        [TestMethod]
        public void SortTest4()
        {
            AssemblyInfoCollection target = CreateTestCollection(true);
            target.Sort();
            Assert.Inconclusive("SortTest3 not implemented");
        }

        [TestMethod]
        public void ToArrayTest()
        {
            AssemblyInfoCollection target = CreateTestCollection(true);
            AssemblyInfo[] actual = target.ToArray();
            Assert.Inconclusive("ToArrayTest not implemented");
        }

        [TestMethod]
        public void TrueForAllTest()
        {
            Predicate<AssemblyInfo> match = null;
            AssemblyInfoCollection target = CreateTestCollection(true);
            bool actual = target.TrueForAll(match);
            Assert.Inconclusive("TrueForAllTest not implemented");
        }
    }
}
