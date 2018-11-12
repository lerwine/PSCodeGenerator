using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeGeneratorCommon;

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

        [Parameter(HelpMessage = "The assignable type to test for")]
        [ValidateTypeSpecification()]
        [ValidateNotNull()]
        [AllowEmptyCollection()]
        public object[] AssignableTo { get; set; }

        [Parameter(HelpMessage = "If this is present, then the test will pass (returns true) if none of the AssignableTo values match; otherwise, the test will pass when t least one of the BaseType values match.")]
        public SwitchParameter InvertAssignableToResult { get; set; }

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
            if (AssignableTo != null)
            {
                if (InvertBaseTypeResult)
                    _tests.Add(t =>
                    {
                        if (t.BaseTypes.Count == 0)
                            return true;
                        TypeNameInfo n = TypeNameInfo.AsTypeNameInfo(t);
                        foreach (object obj in AssignableTo)
                        {
                            if (obj != null && TypeNameInfo.AsTypeNameInfo(obj).IsAssignableFrom(n))
                                return false;
                        }
                        return true;
                    });
                else
                    _tests.Add(t =>
                    {
                        if (t.BaseTypes.Count == 0)
                            return false;
                        TypeNameInfo n = TypeNameInfo.AsTypeNameInfo(t);
                        foreach (object obj in AssignableTo)
                        {
                            if (obj != null && TypeNameInfo.AsTypeNameInfo(obj).IsAssignableFrom(n))
                                return true;
                        }
                        return false;
                    });
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
                    _tests.Add(t => t.BaseTypes.Count == 0 || !BaseType.Select(o => TypeNameInfo.AsTypeNameInfo(o))
                        .Any(b => t.BaseTypes.OfType<CodeTypeReference>().Any(r => b.IsAssignableFrom(TypeNameInfo.AsTypeNameInfo(r)))));
                else
                    _tests.Add(t => t.BaseTypes.Count > 0 && BaseType.Select(o => TypeNameInfo.AsTypeNameInfo(o))
                        .Any(b => t.BaseTypes.OfType<CodeTypeReference>().Any(r => b.IsAssignableFrom(TypeNameInfo.AsTypeNameInfo(r)))));
            }
        }
        
        
        protected override void ProcessRecord()
        {
            if (Declaration == null)
                WriteObject(false);
            else
            {
                foreach (CodeTypeDeclaration type in Declaration)
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
