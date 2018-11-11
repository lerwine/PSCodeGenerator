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
    [Cmdlet(VerbsCommon.Add, "CodeInterfaceDeclaration", DefaultParameterSetName = ParameterSetName_)]
    public class Add_CodeProperty : Cmdlet
    {
        public const string ParameterSetName_ = "";

        [Parameter(Mandatory = true, HelpMessage = "")]
        [ValidateCodeTypeDeclaration(DeclarationType = CodeTypeDeclarationType.Class | CodeTypeDeclarationType.Interface | CodeTypeDeclarationType.Struct)]
        public CodeTypeDeclaration TypeDeclaration { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "")]
        [ValidateLanguageIndependentIdentifier()]
        public string[] Name { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "")]
        [ValidateTypeSpecification()]
        public object PropertyType { get; set; }

        [Parameter()]
        public SwitchParameter Static { get; set; }
    }
}
