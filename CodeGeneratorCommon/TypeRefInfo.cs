using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGeneratorCommon
{
    public class TypeRefInfo
    {
        private string _shortName = null;
        public object OriginalValue { get; private set; }

        public string BaseName { get; private set; }

        public string ShortName 
        {
            get
            {
                string value = _shortName;
                if (value == null)
                {
                    if (ArrayRank > 0)
                        value = ElementType.ShortName + ((ArrayRank == 1) ? "[]" : ((ArrayRank == 2) ? "[,]" : "[" + new String(',', ArrayRank - 1)));
                    else if (GenericArgs.Count == 0)
                    {
                        value = BaseName;
                        switch (BaseName.ToLower())
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
                            case "system.int16":
                            case "int16":
                                value = "short";
                                break;
                            case "system.int32":
                            case "int32":
                                value = "int";
                                break;
                            case "system.int64":
                            case "int64":
                                value = "long";
                                break;
                            case "system.string":
                                value = "string";
                                break;
                            case "system.uint16":
                            case "uint16":
                                value = "ushort";
                                break;
                            case "system.uint32":
                            case "uint32":
                                value = "uint";
                                break;
                            case "system.uint64":
                            case "uint64":
                                value = "ulong";
                                break;
                            case "system.void":
                                value = "void";
                                break;
                        }
                    }
                    _shortName = value;
                }
                return value;
            }
        }

        public string FullName { get; private set; }

        public string Namespace { get; private set; }

        public TypeRefInfo ElementType { get; private set; }

        public int ArrayRank { get; private set; }

        public ReadOnlyCollection<TypeRefInfo> GenericArgs { get; private set; }

        public bool IsGenericParameter { get; private set; }

        private TypeRefInfo() { }
        
        public TypeRefInfo(object value)
        {
            OriginalValue = value;
            if (value != null)
            {
                object baseObject = (value is PSObject) ? ((PSObject)value).BaseObject : value;
                if (baseObject is PSTypeName)
                {
                    Initialize((PSTypeName)baseObject);
                    return;
                }
                if (baseObject is Type)
                {
                    Initialize(new PSTypeName((Type)baseObject));
                    return;
                }
                if (baseObject is CodeTypeReference)
                {
                    Initialize((CodeTypeReference)baseObject);
                    return;
                }
                string s;
                if (LanguagePrimitives.TryConvertTo<string>(value, out s))
                {
                    Initialize(new PSTypeName(s));
                    return;
                }
            }

            BaseName = "";
            _shortName = "";
            FullName = "";
            Namespace = "";
            ElementType = null;
            ArrayRank = 0;
            GenericArgs = new ReadOnlyCollection<TypeRefInfo>(new TypeRefInfo[0]);
            IsGenericParameter = false;

        }
        
        public static string ToFullName(CodeTypeReference typeReference)
        {
            if (typeReference == null)
                return "";
            if (typeReference.ArrayRank > 0)
                return ToFullName(typeReference.ArrayElementType) + ((typeReference.ArrayRank == 1) ? "[]" :
                    ((typeReference.ArrayRank == 2) ? "[,]" : "[" + new String(',', typeReference.ArrayRank - 1) + "]"));
            if (typeReference.TypeArguments.Count == 0)
                return typeReference.BaseType ?? "";
            return (typeReference.BaseType ?? "") + "[" + String.Join(",", typeReference.TypeArguments.OfType<CodeTypeReference>().Select(a => ToFullName(a))) + "]";
        }

        private void Initialize(string name, string @namespace, int arrayRank)
        {

        }

        private void Initialize(CodeTypeReference typeReference)
        {
        }
        
        private void Initialize(PSTypeName typeName)
        {
            if (typeName.Type != null)
            {
                if (typeName.Type.IsArray)
                {
                    ElementType = new TypeRefInfo(typeName.Type.GetElementType());
                    BaseName;
                    FullName;
                    Namespace;
                    ArrayRank;
                    GenericArgs;
                    
                }
                (typeName.Type.IsNested) ? typeName.Type.DeclaringType.FullName : typeName.Type.Namespace
            }
        }
    }
}