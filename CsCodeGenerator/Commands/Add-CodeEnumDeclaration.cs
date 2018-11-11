using System;
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
    [Cmdlet(VerbsCommon.Add, "CodeEnumDeclaration", DefaultParameterSetName = ParameterSetName_Nested)]
    public class Add_CodeEnumDeclaration : CodeTypeDeclarationCommand
    {
        [Parameter(HelpMessage = "Underlying type for enum")]
        public CodeEnumUnderlyingType UnderlyingType { get; set; }

        private Type _underlyingType;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            switch (UnderlyingType)
            {
                case CodeEnumUnderlyingType.Byte:
                    _underlyingType = typeof(byte);
                    break;
                case CodeEnumUnderlyingType.Long:
                    _underlyingType = typeof(long);
                    break;
                case CodeEnumUnderlyingType.SByte:
                    _underlyingType = typeof(sbyte);
                    break;
                case CodeEnumUnderlyingType.Short:
                    _underlyingType = typeof(short);
                    break;
                case CodeEnumUnderlyingType.UInt:
                    _underlyingType = typeof(uint);
                    break;
                case CodeEnumUnderlyingType.ULong:
                    _underlyingType = typeof(ulong);
                    break;
                case CodeEnumUnderlyingType.UShort:
                    _underlyingType = typeof(ushort);
                    break;
                default:
                    _underlyingType = typeof(int);
                    break;
            }
        }

        protected override void OnAddingType(CodeTypeDeclaration type)
        {
            base.OnAddingType(type);
            type.IsEnum = true;
            if (type.BaseTypes.Count > 0)
                type.BaseTypes.Clear();
            type.BaseTypes.Add(_underlyingType);
        }
    }
}
