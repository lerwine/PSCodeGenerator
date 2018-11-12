using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsCodeGenerator.Commands
{
    [Cmdlet(VerbsDiagnostic.Test, "CodeTypeDeclaration", DefaultParameterSetName = ParameterSetName_)]
    public class Test_CodeTypeDeclaration : Cmdlet
    {
        public const string ParameterSetName_ = "";
        private TypeAttributes? _typeAttributeMask;
        private TypeAttributes _typeAttributeMaskValue;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The type declaration to test.")]
        public CodeTypeDeclaration[] Declaration { get; set; }

        [Parameter(HelpMessage = "Tests the TypeAttributes property of the TypeDeclaration for a matching value.")]
        [ValidateNotNullOrEmpty()]
        public TypeAttributes[] TypeAttributeFlag { get; set; }

        [Parameter(HelpMessage = "Bit mask  to apply to the TypeAttributes value being tested. A logical AND operation will be performed on the TypeAttributes property of the TypeDeclaration using the bitwise inverse of this value, to look for a matching TypeAttributesFlag value. This is ignored if TypeAttributeFlag is not specified.")]
        public TypeAttributes TypeAttributeMask
        {
            get
            {
                TypeAttributes? value = _typeAttributeMask;
                return (value.HasValue) ? value.Value : default(TypeAttributes);
            }
            set => _typeAttributeMask = value;
        }

        [Parameter(HelpMessage = "If this is present, then the test will pass (returns true) if none of the TypeAttributesFlag values match; otherwise, the test will pass when t least one of the TypeAttributesFlag values match.")]
        public SwitchParameter InvertTypeAttributeResult { get; set; }

        [Parameter(HelpMessage = "The base type to test for")]
        [ValidateTypeSpecification()]
        [ValidateNotNull()]
        [AllowEmptyCollection()]
        public object[] BaseType { get; set; }

        [Parameter(HelpMessage = "If this is present, then the test will pass (returns true) if none of the BaseType values match; otherwise, the test will pass when t least one of the BaseType values match.")]
        public SwitchParameter InvertBaseTypeResult { get; set; }

        private List<Func<CodeTypeDeclaration, bool>> _tests;

        protected override void BeginProcessing()
        {
            _tests = new List<Func<CodeTypeDeclaration, bool>>();
            if (TypeAttributeFlag != null && TypeAttributeFlag.Length > 0)
            {
                if (_typeAttributeMask.HasValue)
                {
                    _typeAttributeMaskValue = ~_typeAttributeMask.Value;
                    if (InvertTypeAttributeResult.IsPresent)
                        _tests.Add(t => !TypeAttributeFlag.Contains(t.TypeAttributes & _typeAttributeMaskValue));
                    else
                        _tests.Add(t => TypeAttributeFlag.Contains(t.TypeAttributes & _typeAttributeMaskValue));
                }
                else if (InvertTypeAttributeResult.IsPresent)
                    _tests.Add(t => !TypeAttributeFlag.Contains(t.TypeAttributes));
                else
                    _tests.Add(t => TypeAttributeFlag.Contains(t.TypeAttributes));
            }
            if (BaseType != null)
            {
                if (BaseType.Length == 0)
                {
                    if (InvertBaseTypeResult)
                        _tests.Add(t => t.BaseTypes.Count > 0);
                    else
                        _tests.Add(t => t.BaseTypes.Count == 0);
                }
                else if (InvertBaseTypeResult)
                    _tests.Add(t =>
                    {
                        if (t.BaseTypes.Count == 0)
                            return true;
                        foreach (object obj in BaseType)
                        {
                            if (obj == null)
                                continue;
                            object type = (obj is PSObject) ? ((PSObject)obj).BaseObject : obj;
                            if (type is CodeTypeReference)
                            {
                                if (ContainsBaseType(t, (CodeTypeReference)type))
                                    return false;
                            }
                            else if (type is Type)
                            {
                                if (ContainsBaseType(t, (Type)type))
                                    return false;
                            }
                            else
                            {
                                if (LanguagePrimitives.TryConvertTo<string>(obj, out string name) && ContainsBaseType(t, name))
                                    return false;
                            }
                        }
                        return true;
                    });
                else
                    _tests.Add(t =>
                    {
                        if (t.BaseTypes.Count == 0)
                            return false;
                        foreach (object obj in BaseType)
                        {
                            if (obj == null)
                                continue;
                            object type = (obj is PSObject) ? ((PSObject)obj).BaseObject : obj;
                            if (type is CodeTypeReference)
                            {
                                if (ContainsBaseType(t, (CodeTypeReference)type))
                                    return true;
                            }
                            else if (type is Type)
                            {
                                if (ContainsBaseType(t, (Type)type))
                                    return true;
                            }
                            else
                            {
                                if (LanguagePrimitives.TryConvertTo<string>(obj, out string name) && ContainsBaseType(t, name))
                                    return true;
                            }
                        }
                        return false;
                    });
            }
        }

        public static bool ContainsBaseType(CodeTypeDeclaration typeDeclaration, CodeTypeReference typeRef)
        {
            if (typeRef == null)
                return typeDeclaration == null || typeDeclaration.BaseTypes.Count == 0;
            if (typeDeclaration == null || typeDeclaration.BaseTypes.Count == 0)
                return false;
            if (typeDeclaration.BaseTypes.OfType<CodeTypeReference>())
            throw new NotImplementedException();
        }
        
        public static bool ContainsBaseType(CodeTypeDeclaration typeDeclaration, Type type)
        {
            throw new NotImplementedException();
        }

        public static bool ContainsBaseType(CodeTypeDeclaration typeDeclaration, string fullName)
        {
            if (String.IsNullOrEmpty(fullName))
                return typeDeclaration == null || typeDeclaration.BaseTypes.Count == 0;
            if (TryParseTypeReference(fullName, out CodeTypeReference typeRef))
                return ContainsBaseType(typeDeclaration, typeRef);

            throw new NotImplementedException();
        }

        public static readonly Regex IndexerRefRegex = new Regex(@"^(?<e>[^\[\]]+)(?<i>(\[[\s,]*\])+)$", RegexOptions.Compiled);

        public static bool TryParseTypeName(string name, out Type type)
        {
            string b = name ?? "";
            string i;
            Match m = IndexerRefRegex.Match(b);
            if (m.Success)
            {
                b = m.Groups["e"].Value;
                i = m.Groups["i"].Value;
            }
            else
                i = "";
            switch (b)
            {
                case "bool":
                    type = typeof(bool);
                    break;
                case "byte":
                    type = typeof(byte);
                    break;
                case "char":
                    type = typeof(char);
                    break;
                case "decimal":
                    type = typeof(decimal);
                    break;
                case "double":
                    type = typeof(double);
                    break;
                case "float":
                    type = typeof(float);
                    break;
                case "int":
                    type = typeof(int);
                    break;
                case "long":
                    type = typeof(long);
                    break;
                case "object":
                    type = typeof(object);
                    break;
                case "sbyte":
                    type = typeof(sbyte);
                    break;
                case "short":
                    type = typeof(short);
                    break;
                case "string":
                    type = typeof(string);
                    break;
                case "uint":
                    type = typeof(uint);
                    break;
                case "ulong":
                    type = typeof(ulong);
                    break;
                case "ushort":
                    type = typeof(ushort);
                    break;
                case "void":
                    type = typeof(void);
                    break;
                default:
                    type = Type.GetType(b, false, false);
                    if (type == null && (type = Type.GetType(b, false, true)) == null)
                        return false;
                    break;
            }
            
            return i.Length == 0 || (type = Type.GetType(type.FullName + i, false, false)) != null;
        }

        public static bool TryParseTypeReference(string name, out CodeTypeReference typeRef)
        {
            Type type;
            if (TryParseTypeName(name, out type))
            {
                typeRef = new CodeTypeReference(type);
                return true;
            }

            try { typeRef = new CodeTypeReference(name); }
            catch
            {
                typeRef = null;
                return false;
            }

            return true;
        }

        protected override void ProcessRecord()
        {
            if (TypeDeclaration == null)
                WriteObject(false);
            else
            {
                foreach (CodeTypeDeclaration type in TypeDeclaration)
                {
                    if (type == null)
                        WriteObject(false);
                    else
                    {
                        bool success = true;
                    }
                }
            }
        }
    }
}
