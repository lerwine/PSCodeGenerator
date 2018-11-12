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
        public const string ParameterSetName_ = "";

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The type reference to test.")]
        [ValidateNotNullOrEmpty()]
        public CodeTypeReference[] InputType { get; set; }

        [Parameter(HelpMessage = "")]
        [ValidateTypeSpecification(AllowNull = true)]
        [AllowNull()]
        [AllowEmptyString()]
        public object ArrayElementType { get; set; }

        [Parameter(ParameterSetName = ParameterSetName_Type, HelpMessage = "The type reference ")]
        [ValidateTypeSpecification()]
        [ValidateNotNullOrEmpty()]
        public object[] Type { get; set; }

        [Parameter(HelpMessage = "")]
        public int ArrayRank { get; set; }

        [Parameter(HelpMessage = "")]
        public string BaseType { get; set; }

        [Parameter(HelpMessage = "")]
        public CodeTypeReferenceOptions ReferenceType { get; set; }
        
        [Parameter(HelpMessage = "The base type to test for")]
        [ValidateTypeSpecification()]
        [ValidateNotNull()]
        [AllowEmptyCollection()]
        public object[] TypeArguments { get; set; }

        protected override void ProcessRecord()
        {
            if (Type == null)
                WriteObject(false);
            else
            {
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
