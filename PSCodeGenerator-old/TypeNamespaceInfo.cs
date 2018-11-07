using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PSCodeGenerator
{
    public class TypeNamespaceInfo : ITypeNamingIdentifier
    {
        public string Name => throw new NotImplementedException();

        public TypeNamespaceInfo Parent => throw new NotImplementedException();

        ITypeNamingIdentifier ITypeNamingIdentifier.Parent => Parent;

        bool ITypeNamingIdentifier.IsType => false;

        public ICompatibleCollection<TypeInfo> Types => throw new NotImplementedException();

        public ICompatibleCollection<ITypeNamingIdentifier> NestedNameIdentifiers => throw new NotImplementedException();

        public TypeNamespaceInfo(string ns, TypeNamespaceInfo parent)
        {
            throw new NotImplementedException();
        }

        public TypeNamespaceInfo(string ns)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(ITypeNamingIdentifier other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(string other)
        {
            throw new NotImplementedException();
        }

        int IComparable.CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ITypeNamingIdentifier other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(string other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        TypeCode IConvertible.GetTypeCode() => TypeCode.String;

        string IConvertible.ToString(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        #region IConvertible not supported

        bool IConvertible.ToBoolean(IFormatProvider provider) => throw new NotSupportedException();

        byte IConvertible.ToByte(IFormatProvider provider) => throw new NotSupportedException();

        char IConvertible.ToChar(IFormatProvider provider) => throw new NotSupportedException();

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new NotSupportedException();

        decimal IConvertible.ToDecimal(IFormatProvider provider) => throw new NotSupportedException();

        double IConvertible.ToDouble(IFormatProvider provider) => throw new NotSupportedException();

        short IConvertible.ToInt16(IFormatProvider provider) => throw new NotSupportedException();

        int IConvertible.ToInt32(IFormatProvider provider) => throw new NotSupportedException();

        long IConvertible.ToInt64(IFormatProvider provider) => throw new NotSupportedException();

        sbyte IConvertible.ToSByte(IFormatProvider provider) => throw new NotSupportedException();

        float IConvertible.ToSingle(IFormatProvider provider) => throw new NotSupportedException();

        ushort IConvertible.ToUInt16(IFormatProvider provider) => throw new NotSupportedException();

        uint IConvertible.ToUInt32(IFormatProvider provider) => throw new NotSupportedException();

        ulong IConvertible.ToUInt64(IFormatProvider provider) => throw new NotSupportedException();

        #endregion
    }
}