using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace CsCodeGenerator.Commands
{
    [Cmdlet(VerbsCommon.Find, "CodeTypeDeclaration")]
    public class Find_CodeTypeDeclaration : Cmdlet
    {
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty()]
        public string Name { get; set; }
        
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNull()]
        public CodeTypeMember[] InputObject { get; set; }
        
        public static IEnumerable<CodeTypeDeclaration> ByName(IEnumerable<CodeTypeDeclaration> types, string name) => types.Where(t => t != null && t.Name == name);

        public static IEnumerable<CodeTypeDeclaration> ByName(IEnumerable types, string name) => types.OfType<CodeTypeDeclaration>().Where(t => t.Name == name);

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
        }
    }
}
