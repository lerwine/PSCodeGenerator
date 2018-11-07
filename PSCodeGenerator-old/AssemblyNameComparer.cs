using System;
using System.Collections.Generic;
using System.Reflection;

namespace PSCodeGenerator
{
    internal class AssemblyNameComparer : IEqualityComparer<AssemblyName>
    {
        public bool Equals(AssemblyName x, AssemblyName y) => (x == null) ? y == null : (y != null && (ReferenceEquals(x, y) || x.FullName == y.FullName));

        public int GetHashCode(AssemblyName obj) => ((obj == null) ? "" : obj.FullName).GetHashCode();
    }
}