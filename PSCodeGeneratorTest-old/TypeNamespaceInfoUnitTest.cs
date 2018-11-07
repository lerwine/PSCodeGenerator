using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSCodeGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSCodeGeneratorTest
{
    [TestClass]
    public class TypeNamespaceInfoUnitTest
    {
        [TestMethod]
        public void ConstructorTest1()
        {
            string ns = "";
            TypeNamespaceInfo target = new TypeNamespaceInfo(ns);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.ConstructorTest1 not implemented.");
        }

        [TestMethod]
        public void ConstructorTest2()
        {
            string ns = "";
            TypeNamespaceInfo parent = null;
            TypeNamespaceInfo target = new TypeNamespaceInfo(ns, parent);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.ConstructorTest2 not implemented.");
        }

        [TestMethod]
        public void CompareToTest1()
        {
            TypeNamespaceInfo target = new TypeNamespaceInfo("");
            TypeNamespaceInfo other = null;
            int expected = 1;
            int actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.CompareToTest1 not implemented.");
        }

        [TestMethod]
        public void CompareToTest2()
        {
            TypeNamespaceInfo target = new TypeNamespaceInfo("");
            string other = null;
            int expected = 1;
            int actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.CompareToTest2 not implemented.");
        }

        [TestMethod]
        public void EqualsTest1()
        {
            TypeNamespaceInfo target = new TypeNamespaceInfo("");
            TypeNamespaceInfo other = null;
            bool expected = false;
            bool actual = target.Equals(other);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.EqualsTest1 not implemented.");
        }

        [TestMethod]
        public void EqualsTest2()
        {
            TypeNamespaceInfo target = new TypeNamespaceInfo("");
            string other = null;
            bool expected = false;
            bool actual = target.Equals(other);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.EqualsTest2 not implemented.");
        }

        [TestMethod]
        public void EqualsTest3()
        {
            TypeNamespaceInfo target = new TypeNamespaceInfo("");
            object other = null;
            bool expected = false;
            bool actual = target.Equals(other);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.EqualsTest3 not implemented.");
        }

        [TestMethod]
        public void GetFullNameTest()
        {
            string expected = "";
            TypeNamespaceInfo target = new TypeNamespaceInfo(expected);
            string actual = target.GetFullName();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.GetFullNameTest not implemented.");
        }

        [TestMethod]
        public void GetHashCodeTest()
        {
            string ns = "";
            TypeNamespaceInfo target = new TypeNamespaceInfo(ns);
            int expected = ns.GetHashCode();
            int actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.GetHashCodeTest not implemented.");
        }

        [TestMethod]
        public void NameTest()
        {
            string expected = "";
            TypeNamespaceInfo target = new TypeNamespaceInfo(expected);
            string actual = target.Name;
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.NameTest not implemented.");
        }

        [TestMethod]
        public void NestedNameIdentifiersTest()
        {
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.NestedNameIdentifiersTest not implemented.");
        }

        [TestMethod]
        public void ParentTest()
        {
            TypeNamespaceInfo target = new TypeNamespaceInfo("");
            TypeNamespaceInfo actual = target.Parent;
            Assert.IsNull(actual);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.ParentTest not implemented.");
        }

        [TestMethod]
        public void ToStringTest()
        {
            string expected = "";
            TypeNamespaceInfo target = new TypeNamespaceInfo(expected);
            string actual = target.ToString();
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.ToStringTest not implemented.");
        }

        [TestMethod]
        public void TypesTest()
        {
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.TypesTest not implemented.");
        }

        [TestMethod]
        public void IConvertibleTest()
        {
            string expected = "";
            IConvertible target = new TypeNamespaceInfo(expected);
            Type type = typeof(string);
            object actual = target.ToType(type, null);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, type);
            Assert.AreEqual(expected, (string)actual);
            Assert.Inconclusive("TypeNamespaceInfoUnitTest.IConvertibleTest not implemented.");
        }
    }
}
