using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace CsCodeGenerator
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ValidateCodeTypeReferenceAttribute : ValidateEnumeratedArgumentsAttribute
    {
        public bool AllowNullOrEmpty { get; set; }

    }
}
