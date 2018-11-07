using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSCodeGenerator;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PSCodeGeneratorTest
{
    [TestClass]
    public class AssemblyInfoUnitTest
    {
        [TestMethod]
        public void ConstructorTest1()
        {
            Assembly assembly = typeof(string).Assembly;
            AssemblyInfo target = new AssemblyInfo(assembly);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ConstructorTest2()
        {
            AssemblyName assemblyName = typeof(string).Assembly.GetName();
            AssemblyInfo target = new AssemblyInfo(assemblyName);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ConstructorTest3()
        {
            string name = typeof(string).Assembly.FullName;
            AssemblyInfo target = new AssemblyInfo(name);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void FullNameTest()
        {
            Assembly assembly = typeof(string).Assembly;
            AssemblyInfo target = new AssemblyInfo(assembly);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void NameTest()
        {
            AssemblyName assemblyName = typeof(string).Assembly.GetName();
            AssemblyInfo target = new AssemblyInfo(assemblyName);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CultureNameTest()
        {
            AssemblyName assemblyName = typeof(string).Assembly.GetName();
            AssemblyInfo target = new AssemblyInfo(assemblyName);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void PublicKeyToken()
        {
            AssemblyName assemblyName = typeof(string).Assembly.GetName();
            AssemblyInfo target = new AssemblyInfo(assemblyName);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ProcessorArchitectureTest()
        {
            AssemblyName assemblyName = typeof(string).Assembly.GetName();
            AssemblyInfo target = new AssemblyInfo(assemblyName);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void TypesTest()
        {
            Assembly assembly = typeof(string).Assembly;
            AssemblyInfo target = new AssemblyInfo(assembly);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CompareToTest1()
        {
            Assembly assembly = typeof(string).Assembly;
            AssemblyInfo target = new AssemblyInfo(assembly, false);
            Assert.Inconclusive();
            AssemblyInfo other = null;
            target.CompareTo(other);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CompareToTest2()
        {
            AssemblyName assemblyName = typeof(string).Assembly.GetName();
            AssemblyInfo target = new AssemblyInfo(assemblyName);
            Assert.Inconclusive();
            AssemblyName other = null;
            target.CompareTo(other);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CompareToTest3()
        {
            Assembly assembly = typeof(string).Assembly;
            AssemblyInfo target = new AssemblyInfo(assembly, false);
            Assert.Inconclusive();
            Assembly other = null;
            target.CompareTo(other);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CompareToTest4()
        {
            Assembly assembly = typeof(string).Assembly;
            AssemblyInfo target = new AssemblyInfo(assembly, false);
            Assert.Inconclusive();
            string other = null;
            target.CompareTo(other);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void CompareToTest5()
        {
            IComparable target = new AssemblyInfo(typeof(string).Assembly);
            Assert.Inconclusive();
            object other = null;
            target.CompareTo(other);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void EqualsTest1()
        {
            Assembly assembly = typeof(string).Assembly;
            AssemblyInfo target = new AssemblyInfo(assembly);
            AssemblyInfo other = null;
            Assert.Inconclusive();
        }

        [TestMethod]
        public void EqualsTest2()
        {
            AssemblyName assemblyName = typeof(string).Assembly.GetName();
            AssemblyInfo target = new AssemblyInfo(assemblyName);
            AssemblyName other = null;
            Assert.Inconclusive();
        }

        [TestMethod]
        public void EqualsTest3()
        {
            Assembly assembly = typeof(string).Assembly;
            AssemblyInfo target = new AssemblyInfo(assembly);
            Assembly other = null;
            Assert.Inconclusive();
        }

        [TestMethod]
        public void EqualsTest4()
        {
            string name = typeof(string).Assembly.FullName;
            AssemblyInfo target = new AssemblyInfo(name);
            string other = null;
            Assert.Inconclusive();
        }

        [TestMethod]
        public void EqualsTest5()
        {
            Assembly assembly = typeof(string).Assembly;
            AssemblyInfo target = new AssemblyInfo(assembly);
            object other = null;
            Assert.Inconclusive();
        }

        [TestMethod]
        public void GetHashCodeTest()
        {
            string name = typeof(string).Assembly.FullName;
            AssemblyInfo target = new AssemblyInfo(name);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void GetTypeCodeTest()
        {
            Assembly assembly = typeof(string).Assembly;
            IConvertible target = new AssemblyInfo(assembly);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ToStringTest1()
        {
            string name = typeof(string).Assembly.FullName;
            AssemblyInfo target = new AssemblyInfo(name);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ToStringTest2()
        {
            string name = typeof(string).Assembly.FullName;
            IConvertible target = new AssemblyInfo(name);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void ToTypeTest()
        {
            Assembly assembly = typeof(string).Assembly;
            IConvertible target = new AssemblyInfo(assembly);
            Assert.Inconclusive();
        }
    }
}
