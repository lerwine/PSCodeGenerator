using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneratorCommon
{
    public class TypeNameInfo : ITypeName, IScriptExtent
    {
        private string _fullName = null;
        private PSTypeName _typeName;
        private ScriptPosition _startScriptPosition = null;
        private ScriptPosition _endScriptPosition = null;

        public object OriginalValue { get; }

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

        public CodeTypeReference TypeReference { get; }
        
        public string Name => TypeReference.BaseType;

        public string AssemblyName => (TypeName.Type == null) ? null : TypeName.Type.Assembly.FullName;

        public bool IsArray => TypeReference.ArrayRank > 0;

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

        public TypeNameInfo(object value)
        {
            if (value == null)
            {
                OriginalValue = value;
            }

            OriginalValue = (value is PSObject) ? ((PSObject)value).BaseObject : value;
            if (OriginalValue is Type)
                TypeReference = new CodeTypeReference((_typeName = new PSTypeName((Type)OriginalValue)).Type);
            else if (OriginalValue is PSTypeName)
            {
                _typeName = (PSTypeName)OriginalValue;
                TypeReference = (_typeName.Type == null) ? new CodeTypeReference(_typeName.Name) : new CodeTypeReference(_typeName.Type);
            }
            else if (OriginalValue is CodeTypeReference)
                TypeReference = (CodeTypeReference)OriginalValue;
            else
            {
                string name;
                if (LanguagePrimitives.TryConvertTo<string>(value, out name) || name == null)
                    name = "";
                _typeName = new PSTypeName(name);
                TypeReference = (_typeName.Type == null) ? new CodeTypeReference(name) : new CodeTypeReference(_typeName.Type);
            }
        }

        Type ITypeName.GetReflectionAttributeType() => TypeName.Type;

        Type ITypeName.GetReflectionType() => TypeName.Type;

        public override string ToString() => LanguagePrimitives.ConvertTypeNameToPSTypeName(FullName);

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


        public static bool IsValidLanguageIndependentFullName(CodeTypeReference typeReference)
        {
            if (typeReference == null || string.IsNullOrWhiteSpace(typeReference.BaseType))
                return false;
            string baseType = typeReference.BaseType;
            int index = baseType.LastIndexOf('`');
            if (index > 0)
            {
                string n = baseType.Substring(index + 1);
                if (n.Length > 0 && int.TryParse(n, out index) && (typeReference.TypeArguments.Count == 0 || typeReference.TypeArguments.Count == index))
                    baseType = baseType.Substring(0, index);
            }

            return baseType.Split('.').All(n => n.Length > 0 && CodeGenerator.IsValidLanguageIndependentIdentifier(baseType)) && typeReference.TypeArguments.Count == 0 || typeReference.TypeArguments.
                return false;
        }
    }
}
