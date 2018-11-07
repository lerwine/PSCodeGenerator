using System;
using System.Reflection;

namespace PSCodeGenerator
{
    /// <summary>
    /// Identifies a namespace and/or type.
    /// </summary>
    public interface ITypeNamingIdentifier : IEquatable<ITypeNamingIdentifier>, IEquatable<string>, IComparable<ITypeNamingIdentifier>, IComparable<string>, IComparable, IConvertible
    {
        /// <summary>
        /// Gets last element of the dot-separated nested namespace.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Parent namespace element or declaring type.
        /// </summary>
        ITypeNamingIdentifier Parent { get; }

        /// <summary>
        /// Gets a value that indicates whether it is a type or a namespace.
        /// </summary>
        bool IsType { get; }

        /// <summary>
        /// Gets types contained within the current nested namespace.
        /// </summary>
        ICompatibleCollection<TypeInfo> Types { get; }

        /// <summary>
        /// Gets the namespaces or nested types within the current namespace or type.
        /// </summary>
        ICompatibleCollection<ITypeNamingIdentifier> NestedNameIdentifiers { get; }
    }
}