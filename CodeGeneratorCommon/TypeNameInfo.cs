using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGeneratorCommon
{
    /// <summary>
    /// Represents parsed type name information.
    /// </summary>
    public class TypeNameInfo : ITypeName, IScriptExtent, IEquatable<TypeNameInfo>
    {
        private string _fullName = null;
        private string _shortName = null;
        private PSTypeName _typeName;
        private ScriptPosition _startScriptPosition = null;
        private ScriptPosition _endScriptPosition = null;

        /// <summary>
        /// Pattern for parsing namespace, name and generic argument count from a string value (usually from <seealso cref="CodeTypeReference.BaseType" />).
        /// </summary>
        public static readonly Regex NameParseRegex = new Regex(@"^((?<ns>(?=[^.\[\]]*\.)[^.\[\]]*(\.[^.\[\]]*(?=\.))*)\.)?(?<n>[^`\[\]]+)?(`(?<gc>\d+))?", RegexOptions.Compiled);
        
        /// <summary>
        /// Original base value that was used to initialize the current <see cref="TypeNameInfo" />.
        /// </summary>
        public object OriginalBaseValue { get; }

        /// <summary>
        /// Represents a type string that can be used in PowerShell.
        /// </summary>
        public PSTypeName TypeName
        {
            get
            {
                PSTypeName value = _typeName;
                if (value == null)
                    _typeName = value = new PSTypeName(ToFullName(TypeReference));
                return value;
            }
        }

        /// <summary>
        /// Full type name.
        /// </summary>
        public string FullName
        {
            get
            {
                string value = _fullName;
                if (value == null)
                    _fullName = value = ToFullName(TypeReference);
                return value;
            }
        }

        /// <summary>
        /// Full name of delcaring type for nested types; otherwise, mamespace of type reference.
        /// </summary>
        public string Namespace { get; }

        class ScriptPosition : IScriptPosition
        {
            private readonly TypeNameInfo _parent;

            string IScriptPosition.File => null;

            int IScriptPosition.LineNumber => 1;

            public int ColumnNumber { get; }

            public int Offset { get; }

            string IScriptPosition.Line => _parent.ToString();

            internal ScriptPosition(TypeNameInfo parent, int offset)
            {
                _parent = parent;
                Offset = offset;
                ColumnNumber = offset + 1;
            }

            public string GetFullScript() => _parent.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public CodeTypeReference TypeReference { get; }
        
        /// <summary>
        /// Name of type without namespace, array indices or generic parameters.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Short type name.
        /// </summary>
        public string ShortName
        {
            get
            {
                string value = _shortName;
                if (value == null)
                {
                    switch (Name.ToLower())
                    {
                        case "system.boolean":
                        case "boolean":
                            value = "bool";
                            break;
                        case "system.byte":
                            value = "byte";
                            break;
                        case "system.char":
                            value = "char";
                            break;
                        case "system.decimal":
                            value = "decimal";
                            break;
                        case "system.double":
                            value = "double";
                            break;
                        case "system.single":
                        case "single":
                            value = "float";
                            break;
                        case "system.int32":
                        case "int32":
                            value = "int";
                            break;
                        case "system.int64":
                        case "int64":
                            value = "long";
                            break;
                        case "system.object":
                            value = "object";
                            break;
                        case "system.sbyte":
                            value = "sbyte";
                            break;
                        case "system.int16":
                        case "int16":
                            value = "short";
                            break;
                        case "system.string":
                            value = "string";
                            break;
                        case "system.uint32":
                        case "uint32":
                            value = "uint";
                            break;
                        case "system.uint64":
                        case "uint64":
                            value = "ulong";
                            break;
                        case "system.uint16":
                        case "uint16":
                            value = "ushort";
                            break;
                        case "system.void":
                            value = "void";
                            break;
                        case "bool":
                        case "byte":
                        case "char":
                        case "decimal":
                        case "double":
                        case "float":
                        case "int":
                        case "long":
                        case "object":
                        case "sbyte":
                        case "short":
                        case "string":
                        case "uint":
                        case "ulong":
                        case "ushort":
                        case "void":
                            value = Name.ToLower();
                            break;
                    }
                }
                return value;
            }
        }

        /// <summary>
        /// The full name of the <seealso cref="Type.Assembly"> if <seealso cref="PSTypeName.Type" /> is not null; otherwise null.
        /// </summary>
        public string AssemblyName => (TypeName.Type == null) ? null : TypeName.Type.Assembly.FullName;

        /// <summary>
        /// <c>true</C> if <seealso cref="TypeReference" /> represents an array; otherwise, <c>false</C>.
        /// </summary>
        public bool IsArray => TypeReference.ArrayRank > 0;

        /// <summary>
        /// <c>true</C> if <seealso cref="TypeReference" /> represents a geberic type; otherwise, <c>false</C>.
        /// </summary>
        public bool IsGeneric => TypeReference.TypeArguments != null && TypeReference.TypeArguments.Count > 0;

        IScriptExtent ITypeName.Extent => this;

        string IScriptExtent.File => null;

        IScriptPosition IScriptExtent.StartScriptPosition
        {
            get
            {
                ScriptPosition value = _startScriptPosition;
                if (value == null)
                    _startScriptPosition = value = new ScriptPosition(this, 0);
                return value;
            }
        }

        ScriptPosition EndScriptPosition
        {
            get
            {
                ScriptPosition value = _startScriptPosition;
                if (value == null)
                    _startScriptPosition = value = new ScriptPosition(this, ToString().Length);
                return value;
            }
        }

        IScriptPosition IScriptExtent.EndScriptPosition => EndScriptPosition;

        int IScriptExtent.StartLineNumber => 1;

        int IScriptExtent.StartColumnNumber => 1;

        int IScriptExtent.EndLineNumber => 1;

        int IScriptExtent.EndColumnNumber => 1;

        string IScriptExtent.Text => FullName;

        int IScriptExtent.StartOffset => 0;

        int IScriptExtent.EndOffset => EndScriptPosition.Offset;

        /// <summary>
        /// Converts an tobject to a <see cref="TypeNameInfo"/>.
        /// </summary>
        /// <param name="value">Object to convert.</param>
        /// <returns>Object converted to a <see cref="TypeNameInfo"/>.</returns>
        public static TypeNameInfo AsTypeNameInfo(object value)
        {
            if (value != null && value is TypeNameInfo)
                return (TypeNameInfo)value;
            return new TypeNameInfo(value);
        }
        
        private TypeNameInfo(object value)
        {
            if (value == null)
            {
                OriginalBaseValue = null;
                TypeReference = new CodeTypeReference();
                return;
            }

            OriginalBaseValue = (value is PSObject) ? ((PSObject)value).BaseObject : value;

            string ns, tail;
            int gc;

            if (OriginalBaseValue is Type)
                TypeReference = new CodeTypeReference((_typeName = new PSTypeName((Type)OriginalBaseValue)).Type);
            else if (OriginalBaseValue is PSTypeName)
            {
                _typeName = (PSTypeName)OriginalBaseValue;
                TypeReference = (_typeName.Type == null) ? new CodeTypeReference(_typeName.Name) : new CodeTypeReference(_typeName.Type);
            }
            else if (OriginalBaseValue is CodeTypeReference)
                TypeReference = (CodeTypeReference)OriginalBaseValue;
            else
            {
                string name;
                if (LanguagePrimitives.TryConvertTo<string>(value, out name) || name == null)
                    name = "";
                _typeName = new PSTypeName(name);
                if (_typeName.Type == null)
                {
                    name = ParseTypeBaseName(TypeReference.BaseType, out ns, out gc, out tail) ?? "";
                    Namespace = ns;
                    if (gc > 0 && TypeReference.TypeArguments.Count > 0 && TypeReference.TypeArguments.Count != gc && tail.Length > 0)
                        name += tail;
                    switch (name)
                    {
                        case "short":
                            _typeName = new PSTypeName((typeof(short)).FullName + tail);
                            break;
                        case "uint":
                            _typeName = new PSTypeName((typeof(uint)).FullName + tail);
                            break;
                        case "ulong":
                            _typeName = new PSTypeName((typeof(ulong)).FullName + tail);
                            break;
                        case "ushort":
                            _typeName = new PSTypeName((typeof(ushort)).FullName + tail);
                            break;
                    }

                }
                TypeReference = (_typeName.Type == null) ? new CodeTypeReference(name) : new CodeTypeReference(_typeName.Type);
            }

            Name = ParseTypeBaseName(TypeReference.BaseType, out ns, out gc, out tail) ?? "";
            Namespace = ns;
            if (gc > 0 && TypeReference.TypeArguments.Count > 0 && TypeReference.TypeArguments.Count != gc && tail.Length > 0)
                Name += tail;
        }

        Type ITypeName.GetReflectionAttributeType() => TypeName.Type;

        Type ITypeName.GetReflectionType() => TypeName.Type;

        /// <summary>
        /// Gets the PowerShell type name.
        /// </summary>
        /// <returns>The PowerShell type name.</returns>
        public override string ToString() => LanguagePrimitives.ConvertTypeNameToPSTypeName(FullName);

        /// <summary>
        /// Converts a code type reference to a full name string.
        /// </summary>
        /// <param name="typeRef">Code type reference to convert.</param>
        /// <returns>Full type name of code type reference.</returns>
        public static string ToFullName(CodeTypeReference typeRef)
        {
            if (typeRef == null)
                return "";
            if (typeRef.ArrayRank > 0)
                return ToFullName(typeRef.ArrayElementType) + ((typeRef.ArrayRank == 1) ? "[]" : ((typeRef.ArrayRank == 2) ? "[,]" : "[" + (new String(',', typeRef.ArrayRank - 1)) + "]"));
            if (typeRef.TypeArguments != null && typeRef.TypeArguments.Count > 0)
                return typeRef.BaseType + "[" + String.Join(",", typeRef.TypeArguments.OfType<CodeTypeReference>().Select(a => ToFullName(a)).ToArray()) + "]";
            return typeRef.BaseType;
        }

        /// <summary>
        /// Determines whether all names in a type reference are valid language-independent identifiers.
        /// </summary>
        /// <param name="typeReference">Type reference to validate.</param>
        /// <returns><c>true</c> if all names are valid language-independent identifiers; otherwise, <c>false</C>.</returns>
        public static bool IsValidLanguageIndependentFullName(CodeTypeReference typeReference)
        {
            if (typeReference == null || string.IsNullOrWhiteSpace(typeReference.BaseType))
                return false;
            string baseType = typeReference.BaseType;
            string ns, tail;
            int gc;
            baseType = ParseTypeBaseName(baseType, out ns, out gc, out tail) ?? "";
            if (gc > 0 && typeReference.TypeArguments.Count > 0 && typeReference.TypeArguments.Count != gc && tail.Length > 0)
                baseType += tail;

            return baseType.Length > 0 && CodeGenerator.IsValidLanguageIndependentIdentifier(baseType) &&
                (ns.Length == 0 || ns.Split('.').All(n => n.Length > 0 && CodeGenerator.IsValidLanguageIndependentIdentifier(n))) &&
                (typeReference.TypeArguments.Count == 0 || typeReference.TypeArguments.OfType<CodeTypeReference>().All(g => IsValidLanguageIndependentFullName(g)));
        }

        /// <summary>
        /// Parses a type name string.
        /// </summary>
        /// <param name="name">Type name string to parse.</param>
        /// <param name="namespace">Namespace of type name string or an empty string if there was no namespace.</param>
        /// <param name="genericCount">Number of generic arguments indicated by type name string or zero if not specified.</param>
        /// <param name="trailing">Additional text after parsed type namespace and base name.</param>
        /// <returns>The base name of the type string.</returns>
        public static string ParseTypeBaseName(string name, out string @namespace, out int genericCount, out string trailing)
        {
            if (!string.IsNullOrEmpty(name))
            {
                int index = IndexOfNamespaceNameSeparator(name);
                if (index < 0)
                    @namespace = "";
                else
                {
                    @namespace = (index == 0) ? "" : name.Substring(0, index);
                    index++;
                    if (index == name.Length)
                    {
                        genericCount = 0;
                        trailing = "";
                        return "";
                    }
                    name = name.Substring(index);
                }
                index = name.IndexOfAny(new char[] { '`', '[', ']' });
                if (index >= 0)
                {
                    if (name[index] != '`')
                    {
                        trailing = "";
                        genericCount = 0;
                        return name;
                    }
                    trailing = name.Substring(index);
                    index++;
                    int endIdx = index;
                    while (endIdx < name.Length && char.IsDigit(name[endIdx]))
                        endIdx++;
                    if (index < name.Length && endIdx == name.Length && int.TryParse(name.Substring(index, endIdx - index), out genericCount))
                        return name.Substring(0, index - 1);
                    trailing = "";
                    genericCount = 0;
                    return name;
                }
            }
            else
                @namespace = "";
            genericCount = 0;
            trailing = "";
            return name;
        }

        /// <summary>
        /// Gets index of character that separates the base type name from the namespace or declaring type.
        /// </summary>
        /// <param name="name">Type name string.</param>
        /// <returns>Index of the character that separates the base type name from the namespace / declaring type or null if none was found.</returns>
        public static int IndexOfNamespaceNameSeparator(string name)
        {
            if (string.IsNullOrEmpty(name))
                return -1;
            int startIndex = 0;
            int nsIndex = -1;
            int level = 0;
            char[] tokenChars = new char[] { '.', '+', '[', ']' };
            while (startIndex < name.Length)
            {
                int index = name.IndexOfAny(tokenChars, startIndex);
                if (index < 0)
                    return nsIndex;
                switch (name[index])
                {
                    case ']':
                        if (level > 0)
                            level--;
                        break;
                    case '[':
                        level++;
                        break;
                    default:
                        if (level == 0)
                            nsIndex = index;
                        break;
                }
                startIndex = index + 1;
            }
            return nsIndex;
        }

        /// <summary>
        /// Determines whether another <see cref="TypeNameInfo" /> is equal to the current.
        /// </summary>
        /// <param name="other">Other <see cref="TypeNameInfo" /> to compare for equality.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current object; otherwise, <c>false</C>.</returns>
        public bool Equals(TypeNameInfo other) => other != null && (ReferenceEquals(this, other) || FullName.Equals(other.FullName));

        /// <summary>
        /// Determines whether another object is equal to the current.
        /// </summary>
        /// <param name="obj">Object to compare for equality.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is equal to the current object; otherwise, <c>false</C>.</returns>
        public override bool Equals(object obj) => Equals((obj == null || obj is TypeNameInfo) ? (TypeNameInfo)obj : new TypeNameInfo(obj));

        /// <summary>
        /// Gets hashcode for the current object.
        /// </summary>
        /// <returns>The hashcode for the current object.</returns>
        public override int GetHashCode() => FullName.GetHashCode();

        public bool IsAssignableFrom(TypeNameInfo other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (TypeName.Type != null)
            {
                if (other.TypeName.Type != null)
                    return TypeName.Type.IsAssignableFrom(other.TypeName.Type);
                return other.IsArray && TypeName.Type.IsAssignableFrom(typeof(Array));
            }

            if (other.TypeName.Type != null)
                return IsArray && (typeof(Array)).IsAssignableFrom(other.TypeName.Type);
                
            return TypeReference.Options != other.TypeReference.Options && (FullName == other.FullName ||
                (TypeReference.ArrayRank > 0 && TypeReference.ArrayRank == other.TypeReference.ArrayRank &&
                (new TypeNameInfo(TypeReference.ArrayElementType)).IsAssignableFrom(new TypeNameInfo(other.TypeReference.ArrayElementType))));
        }
    }
}
