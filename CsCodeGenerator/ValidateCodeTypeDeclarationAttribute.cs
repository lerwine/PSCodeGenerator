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
        private bool? _isClass = null;
        private bool? _isEnum = null;
        private bool? _isInterface = null;
        private bool? _isStruct = null;

        public bool AllowNull { get; set; }
        public bool IsClass { get => _isClass.HasValue && _isClass.Value; set => _isClass = value; }
        public bool IsEnum { get => _isEnum.HasValue && _isEnum.Value; set => _isEnum = value; }
        public bool IsInterface { get => _isInterface.HasValue && _isInterface.Value; set => _isInterface = value; }
        public bool IsStruct { get => _isStruct.HasValue && _isStruct.Value; set => _isStruct = value; }
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

            if (_isClass.HasValue)
            {
                if (_isClass.Value)
                {
                    // IsClass = true;
                    if (_isInterface.HasValue)
                    {
                        if (_isInterface.Value)
                        {
                            // IsClass = true;
                            // IsInterface = true;
                            if (_isStruct.HasValue)
                            {
                                if (_isStruct.Value)
                                {
                                    // IsClass = true;
                                    // IsInterface = true;
                                    // IsStruct = true;
                                    if (_isEnum.HasValue)
                                    {
                                        if (_isEnum.Value)
                                        {
                                            // IsClass = true;
                                            // IsInterface = true;
                                            // IsStruct = true;
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsClass = true;
                                            // IsInterface = true;
                                            // IsStruct = true;
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsClass = true;
                                        // IsInterface = true;
                                        // IsStruct = true;
                                        // IsEnum = null;
                                    }
                                }
                                else
                                {
                                    // IsClass = true;
                                    // IsInterface = true;
                                    // IsStruct = false;
                                    if (_isEnum.HasValue)
                                    {
                                        if (_isEnum.Value)
                                        {
                                            // IsClass = true;
                                            // IsInterface = true;
                                            // IsStruct = false;
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsClass = true;
                                            // IsInterface = true;
                                            // IsStruct = false;
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsClass = true;
                                        // IsInterface = true;
                                        // IsStruct = false;
                                        // IsEnum = null;
                                    }
                                }
                            }
                            else
                            {
                                // IsClass = true;
                                // IsInterface = true;
                                // IsStruct = null;
                                if (_isEnum.HasValue)
                                {
                                    if (_isEnum.Value)
                                    {
                                        // IsClass = true;
                                        // IsInterface = true;
                                        // IsStruct = null;
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsClass = true;
                                        // IsInterface = true;
                                        // IsStruct = null;
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsClass = true;
                                    // IsInterface = true;
                                    // IsStruct = null;
                                    // IsEnum = null;
                                }
                            }
                        }
                        else
                        {
                            // IsClass = true;
                            // IsInterface = false;
                            if (_isStruct.HasValue)
                            {
                                if (_isStruct.Value)
                                {
                                    // IsClass = true;
                                    // IsInterface = false;
                                    // IsStruct = true;
                                    if (_isEnum.HasValue)
                                    {
                                        if (_isEnum.Value)
                                        {
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsClass = true;
                                            // IsInterface = false;
                                            // IsStruct = true;
                                            // IsClass = true;
                                            // IsInterface = false;
                                            // IsStruct = true;
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsClass = true;
                                        // IsInterface = false;
                                        // IsStruct = true;
                                        // IsEnum = null;
                                    }
                                }
                                else
                                {
                                    // IsClass = true;
                                    // IsInterface = false;
                                    // IsStruct = false;
                                    if (_isEnum.HasValue)
                                    {
                                        if (_isEnum.Value)
                                        {
                                            // IsClass = true;
                                            // IsInterface = false;
                                            // IsStruct = false;
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsClass = true;
                                            // IsInterface = false;
                                            // IsStruct = false;
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsClass = true;
                                        // IsInterface = false;
                                        // IsStruct = false;
                                        // IsEnum = null;
                                    }
                                }
                            }
                            else
                            {
                                // IsClass = true;
                                // IsInterface = false;
                                // IsStruct = null;
                                if (_isEnum.HasValue)
                                {
                                    if (_isEnum.Value)
                                    {
                                        // IsClass = true;
                                        // IsInterface = false;
                                        // IsStruct = null;                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsClass = true;
                                        // IsInterface = false;
                                        // IsStruct = null;
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsClass = true;
                                    // IsInterface = false;
                                    // IsStruct = null;
                                    // IsEnum = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        // IsClass = true;
                        // IsInterface = null;
                        if (_isStruct.HasValue)
                        {
                            if (_isStruct.Value)
                            {
                                // IsClass = true;
                                // IsInterface = null;
                                // IsStruct = true;
                                if (_isEnum.HasValue)
                                {
                                    if (_isEnum.Value)
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
                                if (_isEnum.HasValue)
                                {
                                    if (_isEnum.Value)
                                    {
                                        // IsClass = true;
                                        // IsInterface = null;
                                        // IsStruct = false;
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsClass = true;
                                        // IsInterface = null;
                                        // IsStruct = false;
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsClass = true;
                                    // IsInterface = null;
                                    // IsStruct = false;
                                    // IsEnum = null;
                                }
                            }
                        }
                        else
                        {
                            // IsClass = true;
                            // IsInterface = null;
                            // IsStruct = null;
                            if (_isEnum.HasValue)
                            {
                                if (_isEnum.Value)
                                {
                                    // IsClass = true;
                                    // IsInterface = null;
                                    // IsStruct = null;
                                    // IsEnum = true;
                                }
                                else
                                {
                                    // IsClass = true;
                                    // IsInterface = null;
                                    // IsStruct = null;
                                    // IsEnum = false;
                                }
                            }
                            else
                            {
                                // IsClass = true;
                                // IsInterface = null;
                                // IsStruct = null;
                                // IsEnum = null;
                            }
                        }
                    }
                }
                else
                {
                    // IsClass = false;
                    if (_isInterface.HasValue)
                    {
                        if (_isInterface.Value)
                        {
                            // IsClass = false;
                            // IsInterface = true;
                            if (_isStruct.HasValue)
                            {
                                if (_isStruct.Value)
                                {
                                    // IsClass = false;
                                    // IsInterface = true;
                                    // IsStruct = true;
                                    if (_isEnum.HasValue)
                                    {
                                        if (_isEnum.Value)
                                        {
                                            // IsClass = false;
                                            // IsInterface = true;
                                            // IsStruct = true;
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsClass = false;
                                            // IsInterface = true;
                                            // IsStruct = true;
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsClass = false;
                                        // IsInterface = true;
                                        // IsStruct = true;
                                        // IsEnum = null;
                                    }
                                }
                                else
                                {
                                    // IsClass = false;
                                    // IsInterface = true;
                                    // IsStruct = false;
                                    if (_isEnum.HasValue)
                                    {
                                        if (_isEnum.Value)
                                        {
                                            // IsClass = false;
                                            // IsInterface = true;
                                            // IsStruct = false;
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsClass = false;
                                            // IsInterface = true;
                                            // IsStruct = false;
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsClass = false;
                                        // IsInterface = true;
                                        // IsStruct = false;
                                        // IsEnum = null;
                                    }
                                }
                            }
                            else
                            {
                                // IsClass = false;
                                // IsInterface = true;
                                // IsStruct = null;
                                if (_isEnum.HasValue)
                                {
                                    if (_isEnum.Value)
                                    {
                                        // IsClass = false;
                                        // IsInterface = true;
                                        // IsStruct = null;
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsClass = false;
                                        // IsInterface = true;
                                        // IsStruct = null;
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsClass = false;
                                    // IsInterface = true;
                                    // IsStruct = null;
                                    // IsEnum = null;
                                }
                            }
                        }
                        else
                        {
                            // IsClass = false;
                            // IsInterface = false;
                            if (_isStruct.HasValue)
                            {
                                if (_isStruct.Value)
                                {
                                    // IsClass = false;
                                    // IsInterface = false;
                                    // IsStruct = true;
                                    if (_isEnum.HasValue)
                                    {
                                        if (_isEnum.Value)
                                        {
                                            // IsClass = false;
                                            // IsInterface = false;
                                            // IsStruct = true;
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsClass = false;
                                            // IsInterface = false;
                                            // IsStruct = true;
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsClass = false;
                                        // IsInterface = false;
                                        // IsStruct = true;
                                        // IsEnum = null;
                                    }
                                }
                                else
                                {
                                    // IsClass = false;
                                    // IsInterface = false;
                                    // IsStruct = false;
                                    if (_isEnum.HasValue)
                                    {
                                        if (_isEnum.Value)
                                        {
                                            // IsClass = false;
                                            // IsInterface = false;
                                            // IsStruct = false;
                                            // IsEnum = true;
                                        }
                                        else
                                        {
                                            // IsClass = false;
                                            // IsInterface = false;
                                            // IsStruct = false;
                                            // IsEnum = false;
                                        }
                                    }
                                    else
                                    {
                                        // IsClass = false;
                                        // IsInterface = false;
                                        // IsStruct = false;
                                        // IsEnum = null;
                                    }
                                }
                            }
                            else
                            {
                                // IsClass = false;
                                // IsInterface = false;
                                // IsStruct = null;
                                if (_isEnum.HasValue)
                                {
                                    if (_isEnum.Value)
                                    {
                                        // IsClass = false;
                                        // IsInterface = false;
                                        // IsStruct = null;
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsClass = false;
                                        // IsInterface = false;
                                        // IsStruct = null;
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsClass = false;
                                    // IsInterface = false;
                                    // IsStruct = null;
                                    // IsEnum = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        // IsClass = false;
                        // IsInterface = null;
                        if (_isStruct.HasValue)
                        {
                            if (_isStruct.Value)
                            {
                                // IsClass = false;
                                // IsInterface = null;
                                // IsStruct = true;
                                if (_isEnum.HasValue)
                                {
                                    if (_isEnum.Value)
                                    {
                                        // IsClass = false;
                                        // IsInterface = null;
                                        // IsStruct = true;
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsClass = false;
                                        // IsInterface = null;
                                        // IsStruct = true;
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsClass = false;
                                    // IsInterface = null;
                                    // IsStruct = true;
                                    // IsEnum = null;
                                }
                            }
                            else
                            {
                                // IsClass = false;
                                // IsInterface = null;
                                // IsStruct = false;
                                if (_isEnum.HasValue)
                                {
                                    if (_isEnum.Value)
                                    {
                                        // IsClass = false;
                                        // IsInterface = null;
                                        // IsStruct = false;
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsClass = false;
                                        // IsInterface = null;
                                        // IsStruct = false;
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsClass = false;
                                    // IsInterface = null;
                                    // IsStruct = false;
                                    // IsEnum = null;
                                }
                            }
                        }
                        else
                        {
                            // IsClass = false;
                            // IsInterface = null;
                            // IsStruct = null;
                            if (_isEnum.HasValue)
                            {
                                if (_isEnum.Value)
                                {
                                    // IsClass = false;
                                    // IsInterface = null;
                                    // IsStruct = null;
                                    // IsEnum = true;
                                }
                                else
                                {
                                    // IsClass = false;
                                    // IsInterface = null;
                                    // IsStruct = null;
                                    // IsEnum = false;
                                }
                            }
                            else
                            {
                                // IsClass = false;
                                // IsInterface = null;
                                // IsStruct = null;
                                // IsEnum = null;
                            }
                        }
                    }
                }
            }
            else
            {
                // IsClass = null;
                if (_isInterface.HasValue)
                {
                    if (_isInterface.Value)
                    {
                        // IsClass = null;
                        // IsInterface = true;
                        if (_isStruct.HasValue)
                        {
                            if (_isStruct.Value)
                            {
                                // IsClass = null;
                                // IsInterface = true;
                                // IsStruct = true;
                                if (_isEnum.HasValue)
                                {
                                    if (_isEnum.Value)
                                    {
                                        // IsClass = null;
                                        // IsInterface = true;
                                        // IsStruct = true;
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsClass = null;
                                        // IsInterface = true;
                                        // IsStruct = true;
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsClass = null;
                                    // IsInterface = true;
                                    // IsStruct = true;
                                    // IsEnum = null;
                                }
                            }
                            else
                            {
                                // IsClass = null;
                                // IsInterface = true;
                                // IsStruct = false;
                                if (_isEnum.HasValue)
                                {
                                    if (_isEnum.Value)
                                    {
                                        // IsClass = null;
                                        // IsInterface = true;
                                        // IsStruct = false;
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsClass = null;
                                        // IsInterface = true;
                                        // IsStruct = false;
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsClass = null;
                                    // IsInterface = true;
                                    // IsStruct = false;
                                    // IsEnum = null;
                                }
                            }
                        }
                        else
                        {
                            // IsClass = null;
                            // IsInterface = true;
                            // IsStruct = null;
                            if (_isEnum.HasValue)
                            {
                                if (_isEnum.Value)
                                {
                                    // IsClass = null;
                                    // IsInterface = true;
                                    // IsStruct = null;
                                    // IsEnum = true;
                                }
                                else
                                {
                                    // IsClass = null;
                                    // IsInterface = true;
                                    // IsStruct = null;
                                    // IsEnum = false;
                                }
                            }
                            else
                            {
                                // IsClass = null;
                                // IsInterface = true;
                                // IsStruct = null;
                                // IsEnum = null;
                            }
                        }
                    }
                    else
                    {
                        // IsClass = null;
                        // IsInterface = false;
                        if (_isStruct.HasValue)
                        {
                            if (_isStruct.Value)
                            {
                                // IsClass = null;
                                // IsInterface = false;
                                // IsStruct = true;
                                if (_isEnum.HasValue)
                                {
                                    if (_isEnum.Value)
                                    {
                                        // IsClass = null;
                                        // IsInterface = false;
                                        // IsStruct = true;
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsClass = null;
                                        // IsInterface = false;
                                        // IsStruct = true;
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsClass = null;
                                    // IsInterface = false;
                                    // IsStruct = true;
                                    // IsEnum = null;
                                }
                            }
                            else
                            {
                                // IsClass = null;
                                // IsInterface = false;
                                // IsStruct = false;
                                if (_isEnum.HasValue)
                                {
                                    if (_isEnum.Value)
                                    {
                                        // IsClass = null;
                                        // IsInterface = false;
                                        // IsStruct = false;
                                        // IsEnum = true;
                                    }
                                    else
                                    {
                                        // IsClass = null;
                                        // IsInterface = false;
                                        // IsStruct = false;
                                        // IsEnum = false;
                                    }
                                }
                                else
                                {
                                    // IsClass = null;
                                    // IsInterface = false;
                                    // IsStruct = false;
                                    // IsEnum = null;
                                }
                            }
                        }
                        else
                        {
                            // IsClass = null;
                            // IsInterface = false;
                            // IsStruct = null;
                            if (_isEnum.HasValue)
                            {
                                if (_isEnum.Value)
                                {
                                    // IsClass = null;
                                    // IsInterface = false;
                                    // IsStruct = null;
                                    // IsEnum = true;
                                }
                                else
                                {
                                    // IsClass = null;
                                    // IsInterface = false;
                                    // IsStruct = null;
                                    // IsEnum = false;
                                }
                            }
                            else
                            {
                                // IsClass = null;
                                // IsInterface = false;
                                // IsStruct = null;
                                // IsEnum = null;
                            }
                        }
                    }
                }
                else
                {
                    // IsClass = null;
                    // IsInterface = null;
                    if (_isStruct.HasValue)
                    {
                        if (_isStruct.Value)
                        {
                            // IsClass = null;
                            // IsInterface = null;
                            // IsStruct = true;
                            if (_isEnum.HasValue)
                            {
                                if (_isEnum.Value)
                                {
                                    // IsClass = null;
                                    // IsInterface = null;
                                    // IsStruct = true;
                                    // IsEnum = true;
                                }
                                else
                                {
                                    // IsClass = null;
                                    // IsInterface = null;
                                    // IsStruct = true;
                                    // IsEnum = false;
                                }
                            }
                            else
                            {
                                // IsClass = null;
                                // IsInterface = null;
                                // IsStruct = true;
                                // IsEnum = null;
                            }
                        }
                        else
                        {
                            // IsClass = null;
                            // IsInterface = null;
                            // IsStruct = false;
                            if (_isEnum.HasValue)
                            {
                                if (_isEnum.Value)
                                {
                                    // IsClass = null;
                                    // IsInterface = null;
                                    // IsStruct = false;
                                    // IsEnum = true;
                                }
                                else
                                {
                                    // IsClass = null;
                                    // IsInterface = null;
                                    // IsStruct = false;
                                    // IsEnum = false;
                                }
                            }
                            else
                            {
                                // IsClass = null;
                                // IsInterface = null;
                                // IsStruct = false;
                                // IsEnum = null;
                            }
                        }
                    }
                    else
                    {
                        // IsClass = null;
                        // IsInterface = null;
                        // IsStruct = null;
                        if (_isEnum.HasValue)
                        {
                            if (_isEnum.Value)
                            {
                                // IsClass = null;
                                // IsInterface = null;
                                // IsStruct = null;
                                // IsEnum = true;
                            }
                            else
                            {
                                // IsClass = null;
                                // IsInterface = null;
                                // IsStruct = null;
                                // IsEnum = false;
                            }
                        }
                        else
                        {
                            // IsClass = null;
                            // IsInterface = null;
                            // IsStruct = null;
                            // IsEnum = null;
                        }
                    }
                }
            }
        }
    }
}
