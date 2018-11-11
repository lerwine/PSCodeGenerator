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
    [Cmdlet(VerbsCommon.Add, "CodeClassDeclaration" , DefaultParameterSetName = ParameterSetName_Nested)]
    public class Add_CodeClassDeclaration : CodeTypeDeclarationCommand
    {
        public const string HelpMessage_Extensibility = "Specifies the class extensibility.";

        [Parameter(ParameterSetName = ParameterSetName_Namespace, HelpMessage = HelpMessage_Extensibility)]
        [Parameter(ParameterSetName = ParameterSetName_Nested, HelpMessage = HelpMessage_Extensibility)]
        public CodeClassExtensibility Extensibility { get; set; }

        [Parameter(ParameterSetName = ParameterSetName_Private, HelpMessage = "Specifies that the class cannot be extended.")]
        public SwitchParameter Sealed { get; set; }

        [Parameter(HelpMessage = "Specifies that the class can be serialized.")]
        public SwitchParameter Serializable { get; set; }

        protected override TypeAttributes GetMask()
        {
            return base.GetMask() | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Serializable;
        }

        protected override TypeAttributes GetFlags()
        {
            if (Private.IsPresent)
                return (Sealed.IsPresent) ? base.GetFlags() | TypeAttributes.Sealed : base.GetFlags();

            if (Extensibility == CodeClassExtensibility.Abstract)
                return base.GetFlags() | TypeAttributes.Abstract;

            if (Extensibility == CodeClassExtensibility.Sealed)
                return base.GetFlags() | TypeAttributes.Sealed;

            return base.GetFlags();
        }

        protected override void OnAddingType(CodeTypeDeclaration type)
        {
            base.OnAddingType(type);
            type.IsClass = true;
        }
    }
}
