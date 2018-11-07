using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PSCodeGenerator
{
    /// <summary>
    /// Represents assembly name information.
    /// </summary>
    public class AssemblyInfo : IEquatable<AssemblyInfo>, IEquatable<AssemblyName>, IEquatable<Assembly>, IEquatable<string>,
        IComparable<AssemblyInfo>, IComparable<AssemblyName>, IComparable<Assembly>, IComparable<string>, IComparable, IConvertible
    {
        private static readonly StringComparer _ciComparer = StringComparer.InvariantCultureIgnoreCase;
        private static readonly StringComparer _csComparer = StringComparer.InvariantCulture;

        /// <summary>
        /// Gets the full name of the assembly, also known as the display name.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the simple name of the assembly. This is usually, but not necessarily, the file name of the manifest file of the assembly, minus its extension
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the name of the culture associated with the assembly.
        /// </summary>
        public string CultureName { get; private set; }

        /// <summary>
        /// Gets the major, minor, build, and revision numbers of the assembly.
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        /// Gets the public key token as a hexidecimal string, which represents the last 8 bytes of the SHA-1 hash of the public key under which the application or assembly is signed.
        /// </summary>
        public string PublicKeyToken { get; private set; }

        /// <summary>
        /// Gets a value that identifies the processor and bits-per-word of the platform targeted by an executable.
        /// </summary>
        public ProcessorArchitecture ProcessorArchitecture { get; private set; }

        /// <summary>
        /// Types contained within this <see cref="AssemblyInfo"/>.
        /// </summary>
        public AssemblyTypeNamingCollection Types { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfo"/> class using name of the specified assembly.
        /// </summary>
        /// <param name="assembly">Assembly from which to obtain the name.</param>
        /// <param name="importAllTypes"><c>true</c> to import all assembly types; otherwise, <c>false</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assembly"/> is <c>null</c>.</exception>
        public AssemblyInfo(Assembly assembly, bool importAllTypes)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            Types = new AssemblyTypeNamingCollection(this);
            Initialize(assembly.GetName());
            foreach (Type t in assembly.GetTypes().Where(t => t.IsPublic))
                Types.Import(t);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfo"/> class using name of the specified assembly.
        /// </summary>
        /// <param name="assembly">Assembly from which to obtain the name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assembly"/> is <c>null</c>.</exception>
        public AssemblyInfo(Assembly assembly) : this(assembly, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfo"/> class using the specified assembly name.
        /// </summary>
        /// <param name="name">Name of assembly.</param>
        public AssemblyInfo(AssemblyName name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            Initialize(name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfo"/> class with the specified display name.
        /// </summary>
        /// <param name="name">The display name of the assembly, as returned by the <seealso cref="AssemblyInfo.FullName"/> property.</param>
        public AssemblyInfo(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Trim().Length == 0)
                throw new ArgumentOutOfRangeException("name", "Name cannot be empty");
            try
            {
                Initialize(new AssemblyName(name));
            } catch (Exception e) { throw new ArgumentException("Invalid assembly name", "name", e); }
        }

        private void Initialize(AssemblyName assemblyName)
        {
            FullName = assemblyName.FullName ?? "";
            Name = assemblyName.Name ?? "";
            CultureName = assemblyName.CultureName ?? "";
            Version = assemblyName.Version;
            PublicKeyToken = assemblyName.GetPublicKeyToken().ToHexString() ?? "";
            ProcessorArchitecture = assemblyName.ProcessorArchitecture;
        }

        /// <summary>
        /// Compares the current instance with another <see cref="AssemblyInfo"/> object and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">The object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <para>&lt; 0: This instance precedes <paramref name="other"/> in the sort order.</para>
        /// <para>= 0: This instance occurs in the same position in the sort order as <paramref name="other"/>.</para>
        /// <para>&gt; 0: This instance follows <paramref name="other"/> in the sort order.</para></returns>
        public int CompareTo(AssemblyInfo other) => (other == null) ? 1 : ((ReferenceEquals(this, other)) ? 0 : CompareTo(other.FullName));

        /// <summary>
        /// Compares the current instance with an <see cref="AssemblyName"/> object and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">The object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <para>&lt; 0: This instance precedes <paramref name="other"/> in the sort order.</para>
        /// <para>= 0: This instance occurs in the same position in the sort order as <paramref name="other"/>.</para>
        /// <para>&gt; 0: This instance follows <paramref name="other"/> in the sort order.</para></returns>
        public int CompareTo(AssemblyName other) => (other == null) ? 1 : CompareTo(other.FullName ?? "");

        /// <summary>
        /// Compares the current instance with the <see cref="AssemblyName"/> an <see cref="Assembly"/> object and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">The object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <para>&lt; 0: This instance precedes <paramref name="other"/> in the sort order.</para>
        /// <para>= 0: This instance occurs in the same position in the sort order as <paramref name="other"/>.</para>
        /// <para>&gt; 0: This instance follows <paramref name="other"/> in the sort order.</para></returns>
        public int CompareTo(Assembly other) => (other == null) ? 1 : CompareTo(other.FullName);

        /// <summary>
        /// Compares the <seealso cref="FullName"/> property current instance with the a <see cref="string"/> value and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">The object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <para>&lt; 0: This instance precedes <paramref name="other"/> in the sort order.</para>
        /// <para>= 0: This instance occurs in the same position in the sort order as <paramref name="other"/>.</para>
        /// <para>&gt; 0: This instance follows <paramref name="other"/> in the sort order.</para></returns>
        public int CompareTo(string other)
        {
            if (other == null)
                return 1;
            int i = _ciComparer.Compare(FullName, other);
            return (i == 0) ? _csComparer.Compare(FullName, other) : i;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null || obj is string)
                return CompareTo((string)obj);
            if (obj is AssemblyInfo)
                return CompareTo((AssemblyInfo)obj);
            if (obj is AssemblyName)
                return CompareTo((AssemblyName)obj);
            if (obj is Assembly)
                return CompareTo((Assembly)obj);
            if (obj is IConvertible c)
            {
                if (c.GetTypeCode() == TypeCode.String)
                    try { return CompareTo(c.ToString(System.Globalization.CultureInfo.InvariantCulture)); } catch { }
            }
            try { return CompareTo(obj.ToString()); } catch { return 1; }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another <see cref="AssemblyInfo"/> object.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(AssemblyInfo other) => other != null && (ReferenceEquals(other, this) || _csComparer.Equals(FullName, other.FullName));

        /// <summary>
        /// Indicates whether the current object is equal to an <see cref="AssemblyName"/> object.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(AssemblyName other) => other != null && _csComparer.Equals(FullName, other.FullName ?? "");

        /// <summary>
        /// Indicates whether the current object is equal to the <see cref="AssemblyName"/> of an <see cref="AssemblyInfo"/> object.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(Assembly other) => other != null && _csComparer.Equals(FullName, other.FullName);

        /// <summary>
        /// Indicates whether the <seealso cref="FullName"/> property of the current object is equal to a <see cref="string"/> value.
        /// </summary>
        /// <param name="other">A value to compare with this object.</param>
        /// <returns><c>true</c> if the <seealso cref="FullName"/> property of the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        public bool Equals(string other) => other != null && _csComparer.Equals(FullName, other);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj is string)
                return Equals((string)obj);
            if (obj is AssemblyInfo)
                return Equals((AssemblyInfo)obj);
            if (obj is AssemblyName)
                return Equals((AssemblyName)obj);
            if (obj is Assembly)
                return Equals((Assembly)obj);
            if (obj is IConvertible c)
            {
                if (c.GetTypeCode() == TypeCode.String)
                    try { return Equals(c.ToString(System.Globalization.CultureInfo.InvariantCulture)); } catch { }
            }
            try { return Equals(obj.ToString()); } catch { return false; }
        }

        /// <summary>
        /// Gets the hash code for the current object.
        /// </summary>
        /// <returns>The hash code for the <seealso cref="FullName"/> property value.</returns>
        public override int GetHashCode() => FullName.GetHashCode();

        TypeCode IConvertible.GetTypeCode() => TypeCode.String;

        /// <summary>
        /// Gets a string representation of the current object, which is the value of the <seealso cref="FullName"/> property.
        /// </summary>
        /// <returns>The value of the <seealso cref="FullName"/> property.</returns>
        public override string ToString() => FullName;

        string IConvertible.ToString(IFormatProvider provider) => FullName;

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            string assemblyQualifiedName;
            if (conversionType == null || (assemblyQualifiedName = conversionType.AssemblyQualifiedName) == (typeof(string)).AssemblyQualifiedName)
                return FullName;
            if (assemblyQualifiedName == (typeof(AssemblyName)).AssemblyQualifiedName)
                return new AssemblyName(FullName);
            if (assemblyQualifiedName == (typeof(AssemblyInfo)).AssemblyQualifiedName)
                return this;
            throw new NotSupportedException("Cannot convert type " + (typeof(AssemblyInfo)).AssemblyQualifiedName + " to " + assemblyQualifiedName);
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