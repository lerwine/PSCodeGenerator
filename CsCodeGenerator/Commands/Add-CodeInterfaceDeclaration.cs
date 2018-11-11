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
    [Cmdlet(VerbsCommon.Add, "CodeInterfaceDeclaration", DefaultParameterSetName = ParameterSetName_Nested)]
    public class Add_CodeInterfaceDeclaration : CodeTypeDeclarationCommand
    {
        protected override void OnAddingType(CodeTypeDeclaration type)
        {
            base.OnAddingType(type);
            type.IsInterface = true;
        }
    }
}
