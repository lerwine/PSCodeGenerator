using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace CsCodeGenerator
{
    public sealed class ValidateLanguageIndependentIdentifierAttribute : ValidateEnumeratedArgumentsAttribute
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
            try { name = LanguagePrimitives.ConvertTo<string>(element); }
            catch (Exception exception) { throw new ValidationMetadataException("Could not converto value to string", exception); }
            if (string.IsNullOrWhiteSpace(name))
            {
                if (AllowEmpty)
                    return;
                throw new ValidationMetadataException("Value cannot be empty");
            }

            if (CodeGenerator.IsValidLanguageIndependentIdentifier(name))
                return;
            throw new ValidationMetadataException("Invalid language-independent identifier");
        }
    }
}
