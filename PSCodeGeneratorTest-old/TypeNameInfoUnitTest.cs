using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSCodeGenerator;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PSCodeGeneratorTest
{
    [TestClass]
    public class TypeNameInfoUnitTest
    {
        [TestMethod]
        public void ConstructorTest1()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            Assert.Inconclusive("TypeNameInfoUnitTest.ConstructorTest1 not implemented.");
        }

        [TestMethod]
        public void ConstructorTest2()
        {
            Type type = typeof(string);
            TypeNamespaceInfo parent = new TypeNamespaceInfo("System");
            TypeNameInfo target = new TypeNameInfo(type, parent);
            Assert.Inconclusive("TypeNameInfoUnitTest.ConstructorTest2 not implemented.");
        }

        [TestMethod]
        public void ConstructorTest3()
        {

            TypeInfo typeInfo = typeof(string).GetTypeInfo();
            TypeNameInfo target = new TypeNameInfo(typeInfo);
            Assert.Inconclusive("TypeNameInfoUnitTest.ConstructorTest3 not implemented.");
        }

        [TestMethod]
        public void ConstructorTest4()
        {

            TypeInfo typeInfo = typeof(string).GetTypeInfo();
            TypeNamespaceInfo parent = new TypeNamespaceInfo("System");
            TypeNameInfo target = new TypeNameInfo(typeInfo, parent);
            Assert.Inconclusive("TypeNameInfoUnitTest.ConstructorTest4 not implemented.");
        }

        [TestMethod]
        public void BaseTypeTest()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            TypeNameInfo other = null;
            string expected = "System.Object";
            string actual = target.BaseType;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.BaseTypeTest not implemented.");
        }

        [TestMethod]
        public void CompareToTest1()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            TypeNameInfo other = null;
            int expected = 1;
            int actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.CompareToTest1 not implemented.");
        }

        [TestMethod]
        public void CompareToTest2()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            string other = null;
            int expected = 1;
            int actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.CompareToTest2 not implemented.");
        }

        [TestMethod]
        public void EqualsTest1()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            TypeNameInfo other = null;
            bool expected = false;
            bool actual = target.Equals(other);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.EqualsTest1 not implemented.");
        }

        [TestMethod]
        public void EqualsTest2()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            string other = null;
            bool expected = false;
            bool actual = target.Equals(other);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.EqualsTest2 not implemented.");
        }

        [TestMethod]
        public void EqualsTest3()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            object other = null;
            bool expected = false;
            bool actual = target.Equals(other);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.EqualsTest3 not implemented.");
        }

        [TestMethod]
        public void GenericArgumentCountTest()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            TypeNameInfo other = null;
            int expected = 0;
            int actual = target.GenericArgumentCount;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.GenericArgumentCountTest not implemented.");
        }

        [TestMethod]
        public void GetFullNameTest()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            string actual = target.GetFullName();
            string expected = type.FullName;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.GetFullNameTest not implemented.");
        }

        [TestMethod]
        public void GetHashCodeTest()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            int expected = type.FullName.GetHashCode();
            int actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.GetHashCodeTest not implemented.");
        }

        [TestMethod]
        public void NameTest()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            string actual = target.Name;
            Assert.IsNotNull(actual);
            Assert.AreEqual(type.Name, actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.NameTest not implemented.");
        }

        [TestMethod]
        public void NestedNameIdentifiersTest()
        {
            Assert.Inconclusive("TypeNameInfoUnitTest.NestedNameIdentifiersTest not implemented.");
        }

        [TestMethod]
        public void ParentTest()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            ITypeNamingIdentifier actual = target.Parent;
            Assert.IsNotNull(actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.ParentTest not implemented.");
        }

        [TestMethod]
        public void ToStringTest()
        {
            Type type = typeof(string);
            TypeNameInfo target = new TypeNameInfo(type);
            string actual = target.ToString();
            Assert.IsNotNull(actual);
            Assert.AreEqual(type.FullName, actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.ToStringTest not implemented.");
        }

        [TestMethod]
        public void TypesTest()
        {
            Assert.Inconclusive("TypeNameInfoUnitTest.TypesTest not implemented.");
        }

        [TestMethod]
        public void IConvertibleTest()
        {
            Type type = typeof(string);
            IConvertible target = new TypeNameInfo(type);
            object actual = target.ToType(type, null);
            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, type);
            Assert.AreEqual(type.FullName, (string)actual);
            Assert.Inconclusive("TypeNameInfoUnitTest.IConvertibleTest not implemented.");
        }
    }
}
