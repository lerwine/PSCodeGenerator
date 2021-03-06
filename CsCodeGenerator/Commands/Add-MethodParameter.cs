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
    [Cmdlet(VerbsCommon.Add, "MethodParameter", DefaultParameterSetName = ParameterSetName_)]
    public class Add_MethodParameter : Cmdlet
    {
        public const string ParameterSetName_ = "";

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "")]
        [ValidateLanguageIndependentIdentifier()]
        public string[] Name { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "")]
        public CodeTypeDeclaration TypeDeclaration { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "")]
        [ValidateTypeSpecification()]
        public object ParameterType { get; set; }
    }
}