using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace CsCodeGenerator.Commands
{
    [Cmdlet(VerbsDiagnostic.Test, "CodeTypeReference", DefaultParameterSetName = ParameterSetName_)]
    public class Test_CodeTypeReference : Cmdlet
    {
        public const string ParameterSetName_Type = "Type";
        public const string ParameterSetName_Array = "Array";
        public const string ParameterSetName_ = "";

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The type reference to test.")]
        [ValidateNotNullOrEmpty()]
        public CodeTypeReference[] InputType { get; set; }

        [Parameter(ParameterSetName = ParameterSetName_Type, HelpMessage = "The declaring type for nested types; otherwise, the type's namespace.")]
        [ValidateTypeSpecification()]
        [ValidateNotNullOrEmpty()]
        public object[] DeclaringReference { get; set; }

        [Parameter(ParameterSetName = ParameterSetName_Type, HelpMessage = "Generic Argument types to match.")]
        [ValidateTypeSpecification()]
        [ValidateNotNull()]
        [AllowEmptyCollection()]
        public object[] TypeArguments { get; set; }

        [Parameter(ParameterSetName = ParameterSetName_Array, HelpMessage = "Array element type to match.")]
        [ValidateTypeSpecification(AllowNull = true)]
        [AllowNull()]
        [AllowEmptyString()]
        public object ArrayElementType { get; set; }

        [Parameter(ParameterSetName = ParameterSetName_Array, HelpMessage = "Array rank to match.")]
        public int ArrayRank { get; set; }
        
        [Parameter(HelpMessage = "")]
        public SwitchParameter IsGenericParameter { get; set; }
        
        protected override void ProcessRecord()
        {
            if (Type == null)
                WriteObject(false);
            else
            {
                Type t = typeof(t);
                t.DeclaringType
                foreach (CodeTypeReference t in Type)
                {
                    if (Stopping)
                        break;
                    if (t == null)
                        WriteObject(false);
                }
            }
        }
    }
}
