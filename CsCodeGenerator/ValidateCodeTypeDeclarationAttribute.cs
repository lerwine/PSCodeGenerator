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
    public sealed class ValidateCodeTypeDeclarationAttribute : ValidateEnumeratedArgumentsAttribute
    {
        public bool AllowNull { get; set; }
        public bool? IsClass { get; set; }
        public bool? IsEnum { get; set; }
        public bool? IsInterface { get; set; }
        public bool? IsStruct { get; set; }
        protected override void ValidateElement(object element)
        {
            if (element == null)
            {
                if (AllowNull)
                    return;
                throw new ValidationMetadataException("Value cannot be null");
            }
            CodeTypeDeclaration type;
            try
            {
                type = (CodeTypeDeclaration)((element is PSObject) ? ((PSObject)element).BaseObject : element);
            }
            catch (Exception exception) { throw new ValidationMetadataException("Could not converto value to CodeTypeDeclaration", exception); }

            if (string.IsNullOrEmpty(type.Name) || !CodeGenerator.IsValidLanguageIndependentIdentifier(type.Name))
                throw new ValidationMetadataException("Invalid language-independent identifier name");

            if (IsClass.HasValue)
            {
                if (IsClass.Value)
                {
                    // IsClass = true;
                    if (IsInterface.HasValue)
                    {
                        if (IsInterface.Value)
                        {
                            // IsClass = true;
                            // IsInterface = true;
                            if (IsStruct.HasValue)
                            {
                                if (IsStruct.Value)
                                {
                                    // IsClass = true;
                                    // IsInterface = true;
                                    // IsStruct = true;
                                    if (IsEnum.HasValue)
                                    {
                                        if (IsEnum.Value)
                                        {
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsEnum = null;
                                    }
                                }
                                else
                                {
                                    // IsClass = true;
                                    // IsInterface = true;
                                    // IsStruct = false;
                                    if (IsEnum.HasValue)
                                    {
                                        if (IsEnum.Value)
                                        {
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsEnum = null;
                                    }
                                }
                            }
                            else
                            {
                                // IsClass = true;
                                // IsInterface = true;
                                // IsStruct = null;
                                if (IsEnum.HasValue)
                                {
                                    if (IsEnum.Value)
                                    {
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsEnum = null;
                                }
                            }
                        }
                        else
                        {
                            // IsClass = true;
                            // IsInterface = false;
                            if (IsStruct.HasValue)
                            {
                                if (IsStruct.Value)
                                {
                                    // IsClass = true;
                                    // IsInterface = false;
                                    // IsStruct = true;
                                    if (IsEnum.HasValue)
                                    {
                                        if (IsEnum.Value)
                                        {
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsEnum = null;
                                    }
                                }
                                else
                                {
                                    // IsClass = true;
                                    // IsInterface = false;
                                    // IsStruct = false;
                                    if (IsEnum.HasValue)
                                    {
                                        if (IsEnum.Value)
                                        {
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsEnum = null;
                                    }
                                }
                            }
                            else
                            {
                                // IsClass = true;
                                // IsInterface = false;
                                // IsStruct = null;
                                if (IsEnum.HasValue)
                                {
                                    if (IsEnum.Value)
                                    {
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsEnum = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        // IsClass = true;
                        // IsInterface = null;
                        if (IsStruct.HasValue)
                        {
                            if (IsStruct.Value)
                            {
                                // IsClass = true;
                                // IsInterface = null;
                                // IsStruct = true;
                                if (IsEnum.HasValue)
                                {
                                    if (IsEnum.Value)
                                    {
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsEnum = null;
                                }
                            }
                            else
                            {
                                // IsClass = true;
                                // IsInterface = null;
                                // IsStruct = false;
                                if (IsEnum.HasValue)
                                {
                                    if (IsEnum.Value)
                                    {
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsEnum = null;
                                }
                            }
                        }
                        else
                        {
                            // IsClass = true;
                            // IsInterface = null;
                            // IsStruct = null;
                            if (IsEnum.HasValue)
                            {
                                if (IsEnum.Value)
                                {
                                    // IsEnum = true;
                                }
                                else
                                {
                                    // IsEnum = false;
                                }
                            }
                            else
                            {
                                // IsEnum = null;
                            }
                        }
                    }
                }
                else
                {
                    // IsClass = false;
                    if (IsInterface.HasValue)
                    {
                        if (IsInterface.Value)
                        {
                            // IsClass = false;
                            // IsInterface = true;
                            if (IsStruct.HasValue)
                            {
                                if (IsStruct.Value)
                                {
                                    // IsClass = false;
                                    // IsInterface = true;
                                    // IsStruct = true;
                                    if (IsEnum.HasValue)
                                    {
                                        if (IsEnum.Value)
                                        {
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsEnum = null;
                                    }
                                }
                                else
                                {
                                    // IsClass = false;
                                    // IsInterface = true;
                                    // IsStruct = false;
                                    if (IsEnum.HasValue)
                                    {
                                        if (IsEnum.Value)
                                        {
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsEnum = null;
                                    }
                                }
                            }
                            else
                            {
                                // IsClass = false;
                                // IsInterface = true;
                                // IsStruct = null;
                                if (IsEnum.HasValue)
                                {
                                    if (IsEnum.Value)
                                    {
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsEnum = null;
                                }
                            }
                        }
                        else
                        {
                            // IsClass = false;
                            // IsInterface = false;
                            if (IsStruct.HasValue)
                            {
                                if (IsStruct.Value)
                                {
                                    // IsClass = false;
                                    // IsInterface = false;
                                    // IsStruct = true;
                                    if (IsEnum.HasValue)
                                    {
                                        if (IsEnum.Value)
                                        {
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsEnum = null;
                                    }
                                }
                                else
                                {
                                    // IsClass = false;
                                    // IsInterface = false;
                                    // IsStruct = false;
                                    if (IsEnum.HasValue)
                                    {
                                        if (IsEnum.Value)
                                        {
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsEnum = null;
                                    }
                                }
                            }
                            else
                            {
                                // IsClass = false;
                                // IsInterface = false;
                                // IsStruct = null;
                                if (IsEnum.HasValue)
                                {
                                    if (IsEnum.Value)
                                    {
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsEnum = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        // IsClass = false;
                        // IsInterface = null;
                        if (IsStruct.HasValue)
                        {
                            if (IsStruct.Value)
                            {
                                // IsClass = false;
                                // IsInterface = null;
                                // IsStruct = true;
                                if (IsEnum.HasValue)
                                {
                                    if (IsEnum.Value)
                                    {
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsEnum = null;
                                }
                            }
                            else
                            {
                                // IsClass = false;
                                // IsInterface = null;
                                // IsStruct = false;
                                if (IsEnum.HasValue)
                                {
                                    if (IsEnum.Value)
                                    {
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsEnum = null;
                                }
                            }
                        }
                        else
                        {
                            // IsClass = false;
                            // IsInterface = null;
                            if (IsEnum.HasValue)
                            {
                                if (IsEnum.Value)
                                {
                                    // IsEnum = true;
                                }
                                else
                                {
                                    // IsEnum = false;
                                }
                            }
                            else
                            {
                                // IsEnum = null;
                            }
                            // IsStruct = null;
                        }
                    }
                }
            }
            else
            {
                // IsClass = null;
                if (IsInterface.HasValue)
                {
                    if (IsInterface.Value)
                    {
                        // IsClass = null;
                        // IsInterface = true;
                        if (IsStruct.HasValue)
                        {
                            if (IsStruct.Value)
                            {
                                // IsClass = null;
                                // IsInterface = true;
                                // IsStruct = true;
                                if (IsEnum.HasValue)
                                {
                                    if (IsEnum.Value)
                                    {
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsEnum = null;
                                }
                            }
                            else
                            {
                                // IsClass = null;
                                // IsInterface = true;
                                // IsStruct = false;
                                if (IsEnum.HasValue)
                                {
                                    if (IsEnum.Value)
                                    {
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsEnum = null;
                                }
                            }
                        }
                        else
                        {
                            // IsClass = null;
                            // IsInterface = true;
                            // IsStruct = null;
                            if (IsEnum.HasValue)
                            {
                                if (IsEnum.Value)
                                {
                                    // IsEnum = true;
                                }
                                else
                                {
                                    // IsEnum = false;
                                }
                            }
                            else
                            {
                                // IsEnum = null;
                            }
                        }
                    }
                    else
                    {
                        // IsClass = null;
                        // IsInterface = false;
                        if (IsStruct.HasValue)
                        {
                            if (IsStruct.Value)
                            {
                                // IsClass = null;
                                // IsInterface = false;
                                // IsStruct = true;
                                if (IsEnum.HasValue)
                                {
                                    if (IsEnum.Value)
                                    {
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsEnum = null;
                                }
                            }
                            else
                            {
                                // IsClass = null;
                                // IsInterface = false;
                                // IsStruct = false;
                                if (IsEnum.HasValue)
                                {
                                    if (IsEnum.Value)
                                    {
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsEnum = null;
                                }
                            }
                        }
                        else
                        {
                            // IsClass = null;
                            // IsInterface = false;
                            // IsStruct = null;
                            if (IsEnum.HasValue)
                            {
                                if (IsEnum.Value)
                                {
                                    // IsEnum = true;
                                }
                                else
                                {
                                    // IsEnum = false;
                                }
                            }
                            else
                            {
                                // IsEnum = null;
                            }
                        }
                    }
                }
                else
                {
                    // IsClass = null;
                    // IsInterface = null;
                    if (IsStruct.HasValue)
                    {
                        if (IsStruct.Value)
                        {
                            // IsClass = null;
                            // IsInterface = null;
                            // IsStruct = true;
                            if (IsEnum.HasValue)
                            {
                                if (IsEnum.Value)
                                {
                                    // IsEnum = true;
                                }
                                else
                                {
                                    // IsEnum = false;
                                }
                            }
                            else
                            {
                                // IsEnum = null;
                            }
                        }
                        else
                        {
                            // IsClass = null;
                            // IsInterface = null;
                            // IsStruct = false;
                            if (IsEnum.HasValue)
                            {
                                if (IsEnum.Value)
                                {
                                    // IsEnum = true;
                                }
                                else
                                {
                                    // IsEnum = false;
                                }
                            }
                            else
                            {
                                // IsEnum = null;
                            }
                        }
                    }
                    else
                    {
                        // IsClass = null;
                        // IsInterface = null;
                        // IsStruct = null;
                        if (IsEnum.HasValue)
                        {
                            if (IsEnum.Value)
                            {
                                // IsEnum = true;
                            }
                            else
                            {
                                // IsEnum = false;
                            }
                        }
                        else
                        {
                            // IsEnum = null;
                        }
                    }
                }
            }
        }
    }
}
