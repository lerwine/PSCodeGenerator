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
    public sealed class ValidateCodeTypeParameterAttribute : ValidateEnumeratedArgumentsAttribute
    {
        public bool AllowNullOrEmpty { get; set; }
        
        public bool HasConstructorConstraint { get; set; }

        public string[] Constraints { get; set; }

        public ValidateCodeTypeParameterAttribute(string positionalString)
        {
            throw new NotImplementedException();
        }

        protected override void ValidateElement(object element)
        {
            if (element == null)
            {
                if (AllowNullOrEmpty)
                    return;
                throw new ValidationMetadataException("Type Parameter cannot be null");
            }

            CodeTypeParameter parameter;
            object baseObject = (element is PSObject) ? ((PSObject)element).BaseObject : element;
            if (baseObject is CodeTypeParameter)
                parameter = (CodeTypeParameter)baseObject;
            else
            {
                string name;
                if (baseObject is string)
                    name = ((string)baseObject);
                else
                {
                    try
                    {
                        if (!LanguagePrimitives.TryConvertTo<string>(element, out name))
                            throw new PSArgumentOutOfRangeException();
                    }
                    catch (Exception exception) { throw new ValidationMetadataException("Unable to convert value to string", exception); }
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    if (AllowNullOrEmpty)
                        return;
                    throw new ValidationMetadataException("Type Parameter cannot be empty");
                }
                try { parameter = new CodeTypeParameter(name); }
                catch (Exception exception) { throw new ValidationMetadataException("Unable to convert string to Type Parameter object", exception); }
                parameter.
            }
        }
    }
}
