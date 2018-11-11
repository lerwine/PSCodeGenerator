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
    public class Add_CodeConstructor : Cmdlet
    {
        public const string ParameterSetName_ = "";

        [Parameter(Mandatory = true, HelpMessage = "")]
        [ValidateCodeTypeDeclaration(DeclarationType = DeclarationType.Class | DeclarationType.Struct)]
        public CodeTypeDeclaration TypeDeclaration { get; set; }
    }
}
