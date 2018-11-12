using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsCodeGenerator.Commands
{
    [Cmdlet(VerbsCommon.Set, "CodeBaseType", DefaultParameterSetName = ParameterSetName_Replace)]
    public class Set_CodeBaseType : Cmdlet
    {
        public const string ParameterSetName_Replace = "Replace";
        public const string ParameterSetName_Append = "Append";

        [Parameter(Mandatory = true, HelpMessage = "The target type declaration.")]
        [ValidateCodeTypeDeclaration(DeclarationType = CodeTypeDeclarationType.Class)]
        public CodeTypeDeclaration TypeDeclaration { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The base type to set")]
        [ValidateTypeSpecification()]
        [ValidateNotNullOrEmpty()]
        [Alias("Type", "BaseType")]
        public object[] InputType { get; set; }

        [Parameter(ParameterSetName = ParameterSetName_Replace, HelpMessage = "Replace existing BaseTypes. This is the default behavior.")]
        public SwitchParameter Replace { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ParameterSetName_Append, HelpMessage = "Append to existing BaseTypes.")]
        public SwitchParameter Append { get; set; }

        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            
        }
    }
}
