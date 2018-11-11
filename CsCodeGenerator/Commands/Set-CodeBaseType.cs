﻿using System;
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
    [Cmdlet(VerbsCommon.Set, "CodeBaseType", DefaultParameterSetName = ParameterSetName_)]
    public class Set_CodeBaseType : Cmdlet
    {
        public const string ParameterSetName_ = "";

        [Parameter(Mandatory = true, HelpMessage = "")]
        [ValidateCodeTypeDeclaration(DeclarationType = CodeTypeDeclarationType.Class)]
        public CodeTypeDeclaration TypeDeclaration { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "")]
        [ValidateTypeSpecification()]
        public object Type { get; set; }
    }
}
