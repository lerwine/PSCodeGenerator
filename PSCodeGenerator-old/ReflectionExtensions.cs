using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PSCodeGenerator
{
    /// <summary>
    /// Extension methods for reflection types.
    /// </summary>
    public static class ReflectionExtensions
    {
        private static readonly string _nullableQName;
        private static readonly ReadOnlyDictionary<string, string> _mappings;

        /// <summary>
        /// Indicates whether a type's namespace or the full c# type name of its <seealso cref="Type.DeclaringType"/> (if nested) matches a given string.
        /// </summary>
        /// <param name="type">The type to be tested.</param>
        /// <param name="nsOrDeclaringType">A c# type string that includes the namespace and, if testing a nested type, includes the names of declaring types.</param>
        /// <returns>Returns <c>true</c> if the <seealso cref="Type.IsNested"/> property of <paramref name="type"/> is true and the full c#-style name of its <seealso cref="Type.DeclaringType"/>
        /// matches <paramref name="nsOrDeclaringType"/>
        /// <para>-or-</para>
        /// <para>the <seealso cref="Type.IsNested"/> property of <paramref name="type"/> is false and the <seealso cref="Type.Namespace"/> matches <paramref name="nsOrDeclaringType"/>;</para>
        /// <para>Otherwise, <c>false</c> if <paramref name="type"/> is null or there is no match.</para></returns>
        public static bool MatchesNestedNamespace(this Type type, string nsOrDeclaringType)
        {
            if (type == null)
                return false;
            if (type.IsNested)
                return nsOrDeclaringType != null && nsOrDeclaringType == type.DeclaringType.GetCsFullName();
            if (string.IsNullOrEmpty(type.Namespace))
                return string.IsNullOrEmpty(nsOrDeclaringType);
            return !string.IsNullOrEmpty(nsOrDeclaringType) && nsOrDeclaringType == type.Namespace;
        }

        /// <summary>
        /// Indicates whether a type's namespace or the full c# type name of any recursive <seealso cref="Type.DeclaringType"/>s (if nested) matches a given string.
        /// </summary>
        /// <param name="type">The type to be tested.</param>
        /// <param name="nsOrDeclaringType">A c# type string that includes the namespace and, if testing a nested type, includes the names of declaring types.</param>
        /// <returns>Returns <c>true</c> if the <seealso cref="Type.IsNested"/> property of <paramref name="type"/> is true and the full c#-style name of its <seealso cref="Type.DeclaringType"/>,
        /// including any recursive delcaring types (if deeply nested) matches <paramref name="nsOrDeclaringType"/>
        /// <para>-or-</para>
        /// <para>the <seealso cref="Type.IsNested"/> property of <paramref name="type"/> is false and the <seealso cref="Type.Namespace"/> matches <paramref name="nsOrDeclaringType"/>;</para>
        /// <para>Otherwise, <c>false</c> if <paramref name="type"/> is null or there is no match.</para></returns>
        public static bool HasNestedNamespace(this Type type, string nsOrDeclaringType)
        {
            if (type == null)
                return false;
            if (nsOrDeclaringType == null)
                nsOrDeclaringType = "";
            string nn = (type.IsNested) ? type.DeclaringType.GetCsFullName() : ((type.FullName == null) ? "" : type.FullName);
            if (nsOrDeclaringType.Length == nn.Length)
                return nsOrDeclaringType == nn;
            return nn.Length > (nsOrDeclaringType.Length + 1) && nn.StartsWith(nsOrDeclaringType + ".");
        }

        /// <summary>
        /// Gets dot-separated elements for the <seealso cref="Type.Namespace"/> inluding c# name of <seealso cref="Type.DeclaringType"/> if <seealso cref="Type.IsNested"/> is true.
        /// </summary>
        /// <param name="type">The type to extract nested name elements from.</param>
        /// <returns>Name elements of the <seealso cref="Type.Namespace"/> inluding c# name of <seealso cref="Type.DeclaringType"/> if <seealso cref="Type.IsNested"/> is true.</returns>
        public static string[] GetNestedNamespaceElements(this Type type)
        {
            if (type == null)
                return new string[0];
            if (type.IsNested)
            {
                List<string> list = new List<string>();
                type = type.DeclaringType;
                list.Add(type.GetCsName());
                while (type.IsNested)
                {
                    type = type.DeclaringType;
                    list.Insert(0, type.GetCsName());
                }
                if (!String.IsNullOrEmpty(type.Namespace))
                    return type.Namespace.Split('.').Concat(list).ToArray();
                return list.ToArray();
            }
            if (!String.IsNullOrEmpty(type.Namespace))
                return type.Namespace.Split('.');
            return new string[] { "" };
        }

        static ReflectionExtensions()
        {
            _nullableQName = (typeof(Nullable<>)).AssemblyQualifiedName;

            Dictionary<string, string> d = new Dictionary<string, string>(StringComparer.InvariantCulture);
            foreach (var e in (new[]
            {
                new { Type = typeof(byte), ShortName = "byte" },
                new { Type = typeof(sbyte), ShortName = "sbyte" },
                new { Type = typeof(short), ShortName = "short" },
                new { Type = typeof(ushort), ShortName = "ushort" },
                new { Type = typeof(int), ShortName = "int" },
                new { Type = typeof(uint), ShortName = "uint" },
                new { Type = typeof(long), ShortName = "long" },
                new { Type = typeof(ulong), ShortName = "ulong" },
                new { Type = typeof(char), ShortName = "char" },
                new { Type = typeof(float), ShortName = "float" },
                new { Type = typeof(double), ShortName = "double" },
                new { Type = typeof(decimal), ShortName = "decimal" },
                new { Type = typeof(void), ShortName = "void" },
                new { Type = typeof(object), ShortName = "object" }
            }))
                d.Add(e.Type.AssemblyQualifiedName, e.ShortName);
            _mappings = new ReadOnlyDictionary<string, string>(d);
        }

        /// <summary>
        /// Converts the <seealso cref="Type.Name"/> of a type, including any generic arguments and/or indices, to a c#-compatible name.
        /// </summary>
        /// <param name="type">Type to be represented.</param>
        /// <returns>A c# compatible name, not fully qualified, including any generic arguments and/or indices.</returns>
        public static string GetCsName(this Type type)
        {
            if (type == null)
                return null;
            if (type.IsArray)
            {
                int r = type.GetArrayRank();
                if (r < 2)
                    return type.GetElementType().GetCsName() + "[]";
                if (r == 2)
                    return type.GetElementType().GetCsName() + "[,]";
                return type.GetElementType().GetCsName() + "[" + new string(',', r - 1) + "]";
            }
            if (type.IsByRef)
                return type.GetElementType().GetCsName() + "&";
            if (type.IsPointer)
                return type.GetElementType().GetCsName() + "*";
            string name = type.Name;
            if (type.IsGenericType)
            {
                int i;
                Type[] ga = type.GetGenericArguments();
                if (type.IsGenericTypeDefinition)
                {
                    i = name.LastIndexOf('`');
                    if (i > 0)
                        name = name.Substring(0, i);
                    if (ga.Length < 2)
                        return name + "<>";
                    if (ga.Length == 2)
                        return name + "<,>";
                    return name + "<" + new string(',', ga.Length - 1) + ">";
                }
                if (ga.Length == 1 && type.GetGenericTypeDefinition().AssemblyQualifiedName == _nullableQName)
                    return Nullable.GetUnderlyingType(type).GetCsName() + "?";
                i = name.LastIndexOf('`');
                if (i > 0)
                    name = name.Substring(0, i);
                return name + "<" + String.Join(",", ga.Select(t => t.GetCsName()).ToArray()) + ">";
            }

            string qn = type.AssemblyQualifiedName;
            if (_mappings.ContainsKey(qn))
                return _mappings[qn];
            return name;
        }

        /// <summary>
        /// Converts the <seealso cref="Type.Namespace"/>, <seealso cref="Type.DeclaringType"/>  (if nested) and <seealso cref="Type.Name"/> of a type, including any generic arguments and/or indices, to a c#-compatible full name.
        /// </summary>
        /// <param name="type">Type to be represented.</param>
        /// <returns>A full c# compatible name, not fully qualified, including any generic arguments and/or indices.</returns>
        public static string GetCsFullName(this Type type)
        {
            if (type == null)
                return null;
            string name = type.GetCsName();
            if (type.IsNested)
                return type.DeclaringType.GetCsFullName() + "." + name;

            return (string.IsNullOrEmpty(type.Namespace)) ? name : type.Namespace + "." + name;
        }

        /// <summary>
        /// Gets namespace of a <seealso cref="Type"/> or the full c# compatible name of its <seealso cref="Type.DeclaringType"/> if nested.
        /// </summary>
        /// <param name="type">Type to be represented</param>
        /// <returns>The full c# compatible name of its <seealso cref="Type.DeclaringType"/> if nested or <seealso cref="Type.Namespace"/>.</returns>
        public static string GetCsNestedNamespace(this Type type)
        {
            if (type == null)
                return null;
            return (type.IsNested) ? type.DeclaringType.GetCsFullName() : type.Namespace ?? "";
        }

        /// <summary>
        /// Converts an array of bytes to a hexidecimal string.
        /// </summary>
        /// <param name="buffer">Bytes to be converted.</param>
        /// <returns>A string representing an array of hexidecimal byte values or null if <paramref name="buffer"/> was null.</returns>
        public static string ToHexString(this byte[] buffer) => (buffer == null) ? null : ((buffer.Length == 0) ? "" : String.Join("", buffer.Select(b => ((int)b).ToString("x2")).ToArray()));

        /// <summary>
        /// Parses a character values into an array of bytes.
        /// </summary>
        /// <param name="hexChars">Character values to be parsed.</param>
        /// <param name="parseToEnd">If the initial character of a hexidecimal character pair is not valid, then parsing is stopped if this parameter is false,
        /// otherwise a <seealso cref="HexStringFormatException"/> is thrown.</param>
        /// <returns>An array of bytes parsed from <paramref name="hexChars"/> or null if <paramref name="hexChars"/> was null.</returns>
        /// <exception cref="HexStringFormatException">Format of characters in <paramref name="hexChars"/> was invalid or no hexidecimal characters were found.</exception>
        /// <remarks>Hexidecimal characters in <paramref name="hexChars"/> must be in pairs that represent a single byte.
        /// Hexidecimal character pairs can be separated by a single punctuation character and/or multiple control and whitespace characters.</remarks>
        public static byte[] FromHexString(IEnumerable<char> hexChars, bool parseToEnd)
        {
            if (hexChars == null)
                return null;
            return _FromHexString(hexChars, parseToEnd).ToArray();
        }

        /// <summary>
        /// Parses a character values into an array of bytes.
        /// </summary>
        /// <param name="hexChars">Character values to be parsed.</param>
        /// <returns>An array of parsed bytes or null if <paramref name="hexChars"/> was null.</returns>
        /// <exception cref="HexStringFormatException">Format of characters in <paramref name="hexChars"/> was invalid or no hexidecimal characters were found.</exception>
        /// <remarks>Hexidecimal characters in <paramref name="hexChars"/> must be in pairs that represent a single byte.
        /// Hexidecimal character pairs can be separated by a single punctuation character and/or multiple control and whitespace characters.</remarks>
        public static byte[] FromHexString(IEnumerable<char> hexChars) => FromHexString(hexChars, false);

        private static IEnumerable<byte> _FromHexString(IEnumerable<char> hexChars, bool parseToEnd)
        {
            bool isFirstChar = true;
            bool foundSymbol = false;
            int position = -1;
            byte current = 0;
            using (IEnumerator<char> enumerator = hexChars.GetEnumerator())
            {
                if (parseToEnd)
                {
                    do
                    {
                        if (!enumerator.MoveNext())
                            throw new HexStringFormatException(0, hexChars, "No hexidecimal character pairs found");
                        position++;
                    } while (Char.IsWhiteSpace(enumerator.Current) || Char.IsControl(enumerator.Current));

                    if (Char.IsPunctuation(enumerator.Current))
                        throw new HexStringFormatException(position, hexChars, "Unexpected separator character at position " + position.ToString());
                }
                else
                {
                    do
                    {
                        if (!enumerator.MoveNext() || Char.IsPunctuation(enumerator.Current))
                            throw new HexStringFormatException(0, hexChars, "No hexidecimal character pairs found");
                        position++;
                    } while (Char.IsWhiteSpace(enumerator.Current) || Char.IsControl(enumerator.Current));
                    switch (enumerator.Current)
                    {
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case 'A':
                        case 'a':
                        case 'B':
                        case 'b':
                        case 'C':
                        case 'c':
                        case 'D':
                        case 'd':
                        case 'E':
                        case 'e':
                        case 'F':
                        case 'f':
                            break;
                        default:
                            throw new HexStringFormatException(0, hexChars, "No hexidecimal character pairs found");
                    }
                }

                position--;

                do
                {
                    position++;
                    switch (enumerator.Current)
                    {
                        case '0':
                            if (isFirstChar)
                                current = 0;
                            else
                                yield return current;
                            break;
                        case '1':
                            if (isFirstChar)
                                current = 0x10;
                            else
                                yield return (byte)(current | 0x01);
                            break;
                        case '2':
                            if (isFirstChar)
                                current = 0x20;
                            else
                                yield return (byte)(current | 0x02);
                            break;
                        case '3':
                            if (isFirstChar)
                                current = 0x30;
                            else
                                yield return (byte)(current | 0x03);
                            break;
                        case '4':
                            if (isFirstChar)
                                current = 0x40;
                            else
                                yield return (byte)(current | 0x04);
                            break;
                        case '5':
                            if (isFirstChar)
                                current = 0x50;
                            else
                                yield return (byte)(current | 0x05);
                            break;
                        case '6':
                            if (isFirstChar)
                                current = 0x60;
                            else
                                yield return (byte)(current | 0x06);
                            break;
                        case '7':
                            if (isFirstChar)
                                current = 0x70;
                            else
                                yield return (byte)(current | 0x07);
                            break;
                        case '8':
                            if (isFirstChar)
                                current = 0x80;
                            else
                                yield return (byte)(current | 0x08);
                            break;
                        case '9':
                            if (isFirstChar)
                                current = 0x90;
                            else
                                yield return (byte)(current | 0x09);
                            break;
                        case 'A':
                        case 'a':
                            if (isFirstChar)
                                current = 0xA0;
                            else
                                yield return (byte)(current | 0x0A);
                            break;
                        case 'B':
                        case 'b':
                            if (isFirstChar)
                                current = 0xB0;
                            else
                                yield return (byte)(current | 0x0B);
                            break;
                        case 'C':
                        case 'c':
                            if (isFirstChar)
                                current = 0xC0;
                            else
                                yield return (byte)(current | 0x0C);
                            break;
                        case 'D':
                        case 'd':
                            if (isFirstChar)
                                current = 0xD0;
                            else
                                yield return (byte)(current | 0x0D);
                            break;
                        case 'E':
                        case 'e':
                            if (isFirstChar)
                                current = 0xE0;
                            else
                                yield return (byte)(current | 0x0E);
                            break;
                        case 'F':
                        case 'f':
                            if (isFirstChar)
                                current = 0xF0;
                            else
                                yield return (byte)(current | 0x0F);
                            break;
                        default:
                            if (Char.IsWhiteSpace(enumerator.Current) || Char.IsControl(enumerator.Current))
                            {
                                if (isFirstChar)
                                    continue;
                            }
                            else if (Char.IsPunctuation(enumerator.Current))
                            {
                                if (isFirstChar)
                                {
                                    if (foundSymbol)
                                    {
                                        if (!parseToEnd)
                                            yield break;
                                        throw new HexStringFormatException(position, hexChars, "Unexpected separator character at position " + position.ToString());
                                    }
                                    foundSymbol = true;
                                    continue;
                                }
                            }
                            else
                            {
                                if (isFirstChar && !parseToEnd)
                                    yield break;
                                throw new HexStringFormatException(position, hexChars);
                            }
                            throw new HexStringFormatException(position, hexChars, "Incomplete hexidecimal pair at position " + position.ToString());
                    }
                    foundSymbol = false;
                    isFirstChar = !isFirstChar;
                } while (enumerator.MoveNext());
            }

            if (!isFirstChar)
                throw new HexStringFormatException(position, hexChars, "Incomplete hexidecimal pair at position " + position.ToString());
        }

        public static string GetFullName(this ITypeNamingIdentifier item)
        {
            throw new NotImplementedException();
        }
    }
}