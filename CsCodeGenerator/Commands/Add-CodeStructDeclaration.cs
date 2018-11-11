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
    [Cmdlet(VerbsCommon.Add, "CodeStructDeclaration", DefaultParameterSetName = ParameterSetName_Nested)]
    public class Add_CodeStructDeclaration : CodeTypeDeclarationCommand
    {
        [Parameter(HelpMessage = "Indicates how fields are laid out.")]
        public CodeStructLayout Layout { get; set; }

        protected override TypeAttributes GetMask()
        {
            return base.GetMask() | TypeAttributes.LayoutMask;
        }

        protected override TypeAttributes GetFlags()
        {
            return base.GetFlags() | ((Layout == CodeStructLayout.Explicit) ? TypeAttributes.ExplicitLayout : (Layout == CodeStructLayout.Sequential) ? TypeAttributes.SequentialLayout : TypeAttributes.AutoLayout);
        }

        protected override void OnAddingType(CodeTypeDeclaration type)
        {
            base.OnAddingType(type);
            type.IsStruct = true;
        }
    }
}
