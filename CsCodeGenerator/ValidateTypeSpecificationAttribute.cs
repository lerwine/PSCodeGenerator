using CodeGeneratorCommon;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CsCodeGenerator
{
    public sealed class ValidateTypeSpecificationAttribute : ValidateEnumeratedArgumentsAttribute
    {
        public bool AllowNull { get; set; }
        public bool AllowEmpty { get; set; }
        private object _syncRoot = new object();
        
        protected override void ValidateElement(object element)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                if (element == null)
                {
                    if (AllowNull)
                        return;
                    throw new ValidationMetadataException("Value cannot be null");
                }

                TypeNameInfo typeName = TypeNameInfo.AsTypeNameInfo(element);
                
                if (string.IsNullOrWhiteSpace(typeName.FullName))
                {
                    if (AllowEmpty)
                        return;
                    throw new ValidationMetadataException("Value cannot be empty");
                }

                if (!TypeNameInfo.IsValidLanguageIndependentFullName(typeName.TypeReference))
                    throw new ValidationMetadataException("Invalid language-independent identifier");
            }
            finally { Monitor.Exit(_syncRoot); }
        }
    }
}
