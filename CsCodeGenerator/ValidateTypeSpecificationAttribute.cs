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
    public sealed class ValidateTypeSpecificationAttribute : ValidateEnumeratedArgumentsAttribute
    {
        public bool AllowNull { get; set; }
        public bool AllowEmpty { get; set; }
        protected override void ValidateElement(object element)
        {
            if (element == null)
            {
                if (AllowNull)
                    return;
                throw new ValidationMetadataException("Value cannot be null");
            }
            string name;
            try
            {
                object obj = (element is PSObject) ? ((PSObject)element).BaseObject : element;
                if (obj is Type || obj is CodeTypeReference)
                    return;
                name = LanguagePrimitives.ConvertTo<string>(element);
            }
            catch (Exception exception) { throw new ValidationMetadataException("Could not convert value to string", exception); }

            if (string.IsNullOrWhiteSpace(name))
            {
                if (AllowEmpty)
                    return;
                throw new ValidationMetadataException("Value cannot be empty");
            }

            if (!IsValidFullBaseName(name))
                throw new ValidationMetadataException("Invalid language-independent identifier");
        }

        public static bool IsValidFullBaseName(string name) => name != null && name.Split('.').All(s => s.Trim().Length > 0 && CodeGenerator.IsValidLanguageIndependentIdentifier(s));
    }
}
