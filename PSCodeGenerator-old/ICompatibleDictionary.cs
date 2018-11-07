using System;
using System.Collections;
using System.Collections.Generic;

namespace PSCodeGenerator
{
    /// <summary>
    /// Represents a generically accessible collection of key/value pairs compatible with PowerShell operations.
    /// </summary>
    ///	<typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    ///	<typeparam name="TValue">The type of values in the dictionary.</typeparam>
    public interface ICompatibleDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets the <seealso cref="IEqualityComparer{T}"/> that is used to determine equality of keys for the dictionary.
        /// </summary>
        IEqualityComparer<TKey> Comparer { get; }
    }
}