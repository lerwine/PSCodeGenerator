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
    	public CodeTypeDeclarationType DeclarationType { get; set; }
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
    		if (DeclarationType == CodeTypeDeclarationType.Any)
    			return;
    		if (type.IsEnum)
    		{
    			if (DeclarationType.HasFlag(CodeTypeDeclarationType.Enum))
    				return;
    		}
    		else if (type.IsStruct)
    		{
    			if (DeclarationType.HasFlag(CodeTypeDeclarationType.Struct))
    				return;
    		}
    		else if (type.IsInterface)
    		{
    			if (DeclarationType.HasFlag(CodeTypeDeclarationType.Interface))
    				return;
    		}
    		else if (type.IsClass && DeclarationType.HasFlag(CodeTypeDeclarationType.Class))
    			return;
    		switch (DeclarationType)
    		{
    			case CodeTypeDeclarationType.Value:
    				throw new ValidationMetadataException("Type definition is not a value type");
    			case CodeTypeDeclarationType.Class:
    				throw new ValidationMetadataException("Type definition is not a class");
    			case CodeTypeDeclarationType.Interface:
    				throw new ValidationMetadataException("Type definition is not an interface");
    			case CodeTypeDeclarationType.Struct:
    				throw new ValidationMetadataException("Type definition is not a struct");
    			case CodeTypeDeclarationType.Enum:
    				throw new ValidationMetadataException("Type definition is not an enum");
    			default:
    				if (DeclarationType.HasFlag(CodeTypeDeclarationType.Class))
    				{
    					if (DeclarationType.HasFlag(CodeTypeDeclarationType.Interface))
    					{
    						if (DeclarationType.HasFlag(CodeTypeDeclarationType.Struct))
    							throw new ValidationMetadataException("Type definition is neither a class, interface nor an enum");
    						if (DeclarationType.HasFlag(CodeTypeDeclarationType.Enum))
    							throw new ValidationMetadataException("Type definition is neither a class, interface nor a struct");
    						throw new ValidationMetadataException("Type definition is neither a class nor an interface");
    					}
    					if (DeclarationType.HasFlag(CodeTypeDeclarationType.Struct))
    					{
    						if (DeclarationType.HasFlag(CodeTypeDeclarationType.Enum))
    							throw new ValidationMetadataException("Type definition is neither a class nor a value type");
    						throw new ValidationMetadataException("Type definition is neither a class nor a struct");
    					}
    					throw new ValidationMetadataException("Type definition is neither a class nor an enum");
   					}
   					// DeclarationType.HasFlag(CodeTypeDeclarationType.Interface))
    				if (DeclarationType.HasFlag(CodeTypeDeclarationType.Struct))
    				{
    					if (DeclarationType.HasFlag(CodeTypeDeclarationType.Enum))
    						throw new ValidationMetadataException("Type definition is neither an interface nor a value type");
    					throw new ValidationMetadataException("Type definition is neither an interface nor a struct");
    				}
    				// DeclarationType.HasFlag(CodeTypeDeclarationType.Enum)
    				throw new ValidationMetadataException("Type definition is neither an interface nor an enum");
    		}
    	}
    }
    
    [Flags]
    public sealed enum CodeTypeDeclarationType : byte
    {
    	Any = 0x0F,
    	NonValue = 0x03,
    	Value = 0x0C,
    	Class = 0x01,
    	Interface = 0x02,
    	Struct = 0x04,
    	Enum = 0x08
    }
}
