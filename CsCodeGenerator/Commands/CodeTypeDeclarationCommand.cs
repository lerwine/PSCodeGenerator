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
    public class CodeTypeDeclarationCommand : Cmdlet
    {
        public const string ParameterSetName_Nested = "Nested";
        public const string ParameterSetName_Namespace = "Namespace";
        public const string ParameterSetName_Private = "Private";
        public const string HelpMessage_Parent = "The type under which the new type will be nested.";
        public const string HelpMessage_Internal = "Specifies that the type is visible within the parent assembly.";

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "Name of type to be added.")]
        [ValidateLanguageIndependentIdentifier()]
        public string[] Name { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ParameterSetName_Namespace, HelpMessage = "Namespace to which the type defintion will be added.")]
        public virtual CodeNamespace Namespace { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ParameterSetName_Nested, HelpMessage = HelpMessage_Parent)]
        [Parameter(Mandatory = true, ParameterSetName = ParameterSetName_Private, HelpMessage = HelpMessage_Parent)]
        public virtual CodeTypeDeclaration Parent { get; set; }

        [Parameter(ParameterSetName = ParameterSetName_Nested, HelpMessage = "Specifies that the type is visible to inheriting classes.")]
        public SwitchParameter Protected { get; set; }

        [Parameter(ParameterSetName = ParameterSetName_Namespace, HelpMessage = HelpMessage_Internal)]
        [Parameter(ParameterSetName = ParameterSetName_Nested, HelpMessage = HelpMessage_Internal)]
        public SwitchParameter Internal { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = ParameterSetName_Private, HelpMessage = "Specifies that the new type will be a nested, private type.")]
        public SwitchParameter Private { get; set; }
        
        [Parameter(HelpMessage = "Return the type(s) that were added.")]
        public SwitchParameter PassThru { get; set; }

        private TypeAttributes _flags;
        private TypeAttributes _mask;
        Action<CodeTypeDeclaration> _addType;
        Func<string, bool> _containsName;

        public CodeTypeDeclaration Add(string name, CodeNamespace @namespace, bool isInternal, TypeAttributes mask, TypeAttributes flags)
        {
            CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(name);
            codeTypeDeclaration.TypeAttributes = (codeTypeDeclaration.TypeAttributes & ~(mask | TypeAttributes.VisibilityMask)) | flags | ((isInternal) ? TypeAttributes.NotPublic : TypeAttributes.Public);
            @namespace.Types.Add(codeTypeDeclaration);
            return codeTypeDeclaration;
        }

        public CodeTypeDeclaration Add(string name, CodeTypeDeclaration parent, bool isInternal, bool isProtected, TypeAttributes mask, TypeAttributes flags)
        {
            CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(name);
            codeTypeDeclaration.TypeAttributes = (codeTypeDeclaration.TypeAttributes & ~(mask | TypeAttributes.VisibilityMask)) | flags | ((isInternal) ? TypeAttributes.NotPublic : TypeAttributes.Public);
            @namespace.Types.Add(codeTypeDeclaration);
            return codeTypeDeclaration;
        }

        protected override void BeginProcessing()
        {
            if (Namespace != null)
            {
                _flags = (Internal.IsPresent) ? TypeAttributes.NotPublic : TypeAttributes.Public;
                _containsName = n => Find_CodeTypeDeclaration.ByName(Namespace.Types, n) != null;
                if (PassThru.IsPresent)
                    _addType = t =>
                    {
                        OnAddingType(t);
                        Namespace.Types.Add(t);
                        WriteObject(t);
                    };
                else
                    _addType = t =>
                    {
                        OnAddingType(t);
                        Namespace.Types.Add(t);
                    };
            }
            else
            {
                if (Private.IsPresent)
                    _flags = TypeAttributes.NestedPrivate;
                else if (Protected.IsPresent)
                    _flags = (Internal.IsPresent) ? TypeAttributes.NestedFamANDAssem : TypeAttributes.NestedFamily;
                else
                    _flags = (Internal.IsPresent) ? TypeAttributes.NestedAssembly : TypeAttributes.NestedPublic;

                _containsName = n => Find_CodeTypeDeclaration.ByName(Parent.Members, n) != null;
                if (PassThru.IsPresent)
                    _addType = t =>
                    {
                        OnAddingType(t);
                        Parent.Members.Add(t);
                        WriteObject(t);
                    };
                else
                    _addType = t =>
                    {
                        OnAddingType(t);
                        Parent.Members.Add(t);
                    };
            }
            _flags = GetFlags() | _flags;
            _mask = TypeAttributes.VisibilityMask;
            _mask = ~(GetMask() | TypeAttributes.VisibilityMask);
        }

        protected override void ProcessRecord()
        {
            foreach (string n in Name)
            {
                if (Stopping)
                    break;
                try
                {
                    if (n == null)
                        throw new PSArgumentNullException("Name");
                    if (n.Trim().Length == 0)
                        throw new PSArgumentOutOfRangeException("Name", n, "Name cannot be empty");
                    if (!CodeGenerator.IsValidLanguageIndependentIdentifier(n))
                        throw new PSArgumentOutOfRangeException("Name", n, "Name is not a valid language-independent identifier");
                    if (_containsName(n))
                        throw new PSArgumentException((Namespace != null) ? "The target namespace already contains a type declaration with that name" : "The target type declaration already contains a nested type declaration with that name", "Name");
                    CodeTypeDeclaration t = new CodeTypeDeclaration(n);
                    t.TypeAttributes = (t.TypeAttributes & _mask) | _flags;
                    _addType(t);
                }
                catch (PSArgumentNullException exception)
                {
                    if (!Stopping)
                        WriteError(new ErrorRecord(exception, "NullName", ErrorCategory.InvalidArgument, n));
                }
                catch (PSArgumentOutOfRangeException exception)
                {
                    if (!Stopping)
                        WriteError(new ErrorRecord(exception, "InvalidName", ErrorCategory.InvalidArgument, n));
                }
                catch (PSArgumentException exception)
                {
                    if (!Stopping)
                        WriteError(new ErrorRecord(exception, "NameExists", ErrorCategory.ResourceExists, n));
                }
                catch (Exception exception)
                {
                    if (!Stopping)
                        WriteError(new ErrorRecord(exception, "AddFail", ErrorCategory.WriteError, n));
                }
            }
        }

        protected virtual TypeAttributes GetMask() => _mask;

        protected virtual TypeAttributes GetFlags() => _flags;
        
        protected virtual void OnAddingType(CodeTypeDeclaration type) { }
    }
}
