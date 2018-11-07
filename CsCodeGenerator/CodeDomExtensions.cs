using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsCodeGenerator
{
    public static class CodeDomExtensions
    {
        public static bool IsSubNamespace(this CodeNamespace target, CodeNamespace ns) => target != null && ns != null && target.Name.StartsWith(ns.Name + ".");

        public static void WriteCSharpCode(this CodeNamespace @namespace, TextWriter textWriter, CodeGeneratorOptions options)
        {
            using (CodeDomProvider provider = CodeDomProvider.CreateProvider("c#"))
                @namespace.WriteCode(provider, textWriter, options);
        }

        public static void WriteCode(this CodeNamespace @namespace, CodeDomProvider provider, TextWriter textWriter, CodeGeneratorOptions options)
        {
            CodeGenerator.ValidateIdentifiers(@namespace);
            if (textWriter is IndentedTextWriter)
                provider.GenerateCodeFromNamespace(@namespace, textWriter, options);
            else
            {
                using (IndentedTextWriter indentedWriter = new IndentedTextWriter(textWriter))
                {
                    provider.GenerateCodeFromNamespace(@namespace, indentedWriter, options);
                    indentedWriter.Flush();
                }
            }
        }

        public static void WriteCode(this CodeNamespace @namespace, CodeDomProvider provider, TextWriter textWriter) { @namespace.WriteCode(provider, textWriter, null); }

        public static string GenerateCSharpCode(this CodeNamespace target, CodeGeneratorOptions options)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                target.WriteCSharpCode(textWriter, options);
                return textWriter.ToString();
            }
        }

        public static string GenerateCSharpCode(this CodeNamespace @namespace) { return @namespace.GenerateCSharpCode(null); }

        public static string GenerateCode(this CodeNamespace @namespace, CodeDomProvider provider, CodeGeneratorOptions options)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                @namespace.WriteCode(provider, textWriter, options);
                return textWriter.ToString();
            }
        }

        public static string GenerateCode(this CodeNamespace @namespace, CodeDomProvider provider) { return @namespace.GenerateCode(provider, null); }

        private static CodeTypeDeclaration AddTypeDeclaration(CodeNamespace @namespace, string name, TypeAttributes visibility, Action<CodeTypeDeclaration> onInit, TypeAttributes mask  = TypeAttributes.VisibilityMask)
        {
            if (@namespace == null)
                throw new ArgumentNullException("namespace");
            if (name == null)
                throw new ArgumentNullException("name");
            if (!CodeGenerator.IsValidLanguageIndependentIdentifier(name))
                throw new ArgumentException("Invalid type name", "name");
            CodeTypeDeclaration type = new CodeTypeDeclaration(name);
            onInit(type);
            type.TypeAttributes = (type.TypeAttributes & ~mask) | visibility;
            @namespace.Types.Add(type);
            return type;
        }

        private static CodeTypeDeclaration AddTypeDeclaration(this CodeTypeDeclaration parent, string name, TypeAttributes visibility, Action<CodeTypeDeclaration> onInit, TypeAttributes mask = TypeAttributes.VisibilityMask)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (name == null)
                throw new ArgumentNullException("name");
            if (!CodeGenerator.IsValidLanguageIndependentIdentifier(name))
                throw new ArgumentException("Invalid type name", "name");
            CodeTypeDeclaration type = new CodeTypeDeclaration(name);
            onInit(type);
            type.TypeAttributes = (type.TypeAttributes & ~mask) | visibility;
            parent.Members.Add(type);
            return type;
        }

        public static CodeTypeDeclaration AddClass(this CodeNamespace @namespace, string name, bool isInternal = false) => AddTypeDeclaration(@namespace, name, (isInternal) ? TypeAttributes.NotPublic : TypeAttributes.Public, t => t.IsClass = true);

        public static CodeTypeDeclaration AddClass(this CodeTypeDeclaration parent, string name) => AddTypeDeclaration(parent, name, TypeAttributes.NestedPublic, t => t.IsClass = true);

        public static CodeTypeDeclaration AddNonPublicClass(this CodeTypeDeclaration parent, string name, bool isProtected = false, bool isInternal = false) => AddTypeDeclaration(parent, name,
            (isProtected) ? ((isInternal) ? TypeAttributes.NestedFamORAssem : TypeAttributes.NestedFamily) : ((isInternal) ? TypeAttributes.NestedAssembly : TypeAttributes.NestedPrivate), t => t.IsClass = true);

        public static CodeTypeDeclaration AddInterface(this CodeNamespace @namespace, string name, bool isInternal = false) => AddTypeDeclaration(@namespace, name, (isInternal) ? TypeAttributes.NotPublic : TypeAttributes.Public, t => t.IsInterface = true);

        public static CodeTypeDeclaration AddInterface(this CodeTypeDeclaration parent, string name) => AddTypeDeclaration(parent, name, TypeAttributes.NestedPublic, t => t.IsInterface = true);

        public static CodeTypeDeclaration AddNonPublicInterface(this CodeTypeDeclaration parent, string name, bool isProtected = false, bool isInternal = false) => AddTypeDeclaration(parent, name,
            (isProtected) ? ((isInternal) ? TypeAttributes.NestedFamORAssem : TypeAttributes.NestedFamily) : ((isInternal) ? TypeAttributes.NestedAssembly : TypeAttributes.NestedPrivate), t => t.IsInterface = true);

        public static bool IsValidEnumBaseType(this Type type)
        {
            if (type != null && type.IsPrimitive && type.Assembly.FullName == typeof(int).Assembly.FullName)
            {
                switch (type.FullName)
                {
                    case "System.Byte":
                    case "System.SByte":
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                    case "System.UInt16":
                    case "System.UInt32":
                    case "System.UInt64":
                        return true;
                }
            }
            return false;
        }

        public static CodeTypeDeclaration AddEnum(this CodeNamespace @namespace, string name, Type baseType, bool isInternal = false) => AddTypeDeclaration(@namespace, name, (isInternal) ? TypeAttributes.NotPublic : TypeAttributes.Public, t =>
            {
                t.IsEnum = true;
                if (baseType != null)
                {
                    if (!baseType.IsValidEnumBaseType())
                        throw new ArgumentException("Invalid base enum type", "baseType");
                    t.BaseTypes.Add(baseType);
                }
            });

        public static CodeTypeDeclaration AddEnum(this CodeNamespace @namespace, string name, bool isInternal = false) => @namespace.AddEnum(name, null, isInternal);

        public static CodeTypeDeclaration AddEnum(this CodeTypeDeclaration parent, string name, Type baseType = null) => AddTypeDeclaration(parent, name, TypeAttributes.NestedPublic, t =>
            {
                t.IsEnum = true;
                if (baseType != null)
                {
                    if (!baseType.IsValidEnumBaseType())
                        throw new ArgumentException("Invalid base enum type", "baseType");
                    t.BaseTypes.Add(baseType);
                }
            });

        public static CodeTypeDeclaration AddEnum(this CodeTypeDeclaration parent, string name) => parent.AddEnum(name, null);

        public static CodeTypeDeclaration AddNonPublicEnum(this CodeTypeDeclaration parent, string name, Type baseType, bool isProtected = false, bool isInternal = false) => AddTypeDeclaration(parent, name,
            (isProtected) ? ((isInternal) ? TypeAttributes.NestedFamORAssem : TypeAttributes.NestedFamily) : ((isInternal) ? TypeAttributes.NestedAssembly : TypeAttributes.NestedPrivate), t =>
            {
                t.IsEnum = true;
                if (baseType != null)
                {
                    if (!baseType.IsValidEnumBaseType())
                        throw new ArgumentException("Invalid base enum type", "baseType");
                    t.BaseTypes.Add(baseType);
                }
            });

        public static CodeTypeDeclaration AddNonPublicEnum(this CodeTypeDeclaration parent, string name, bool isProtected = false, bool isInternal = false) => parent.AddNonPublicEnum(name, null, isProtected, isInternal);

        public static CodeTypeDeclaration AddStruct(this CodeNamespace @namespace, string name, bool isInternal = false) => AddTypeDeclaration(@namespace, name,
            TypeAttributes.AutoLayout | ((isInternal) ? TypeAttributes.NotPublic : TypeAttributes.Public), t => t.IsStruct = true, TypeAttributes.VisibilityMask | TypeAttributes.LayoutMask);

        public static CodeTypeDeclaration AddStruct(this CodeTypeDeclaration parent, string name) => AddTypeDeclaration(parent, name, TypeAttributes.AutoLayout | TypeAttributes.NestedPublic, t => t.IsStruct = true, TypeAttributes.VisibilityMask | TypeAttributes.LayoutMask);

        public static CodeTypeDeclaration AddNonPublicStruct(this CodeTypeDeclaration parent, string name, bool isProtected = false, bool isInternal = false) => AddTypeDeclaration(parent, name,
            TypeAttributes.AutoLayout | ((isProtected) ? ((isInternal) ? TypeAttributes.NestedFamORAssem : TypeAttributes.NestedFamily) : ((isInternal) ? TypeAttributes.NestedAssembly : TypeAttributes.NestedPrivate)),
            t => t.IsStruct = true, TypeAttributes.VisibilityMask | TypeAttributes.LayoutMask);

        public static CodeTypeDeclaration AddExplicitLayoutStruct(this CodeNamespace @namespace, string name, bool isInternal = false) => AddTypeDeclaration(@namespace, name,
            TypeAttributes.ExplicitLayout | ((isInternal) ? TypeAttributes.NotPublic : TypeAttributes.Public), t => t.IsStruct = true, TypeAttributes.VisibilityMask | TypeAttributes.LayoutMask);

        public static CodeTypeDeclaration AddExplicitLayoutStruct(this CodeTypeDeclaration parent, string name) => AddTypeDeclaration(parent, name, TypeAttributes.ExplicitLayout | TypeAttributes.NestedPublic, t => t.IsStruct = true, TypeAttributes.VisibilityMask | TypeAttributes.LayoutMask);

        public static CodeTypeDeclaration AddExplicitLayoutNonPublicStruct(this CodeTypeDeclaration parent, string name, bool isProtected = false, bool isInternal = false) => AddTypeDeclaration(parent, name,
            TypeAttributes.ExplicitLayout | ((isProtected) ? ((isInternal) ? TypeAttributes.NestedFamORAssem : TypeAttributes.NestedFamily) : ((isInternal) ? TypeAttributes.NestedAssembly : TypeAttributes.NestedPrivate)),
            t => t.IsStruct = true, TypeAttributes.VisibilityMask | TypeAttributes.LayoutMask);

        public static CodeTypeDeclaration AddSequentialLayoutStruct(this CodeNamespace @namespace, string name, bool isInternal = false) => AddTypeDeclaration(@namespace, name,
            TypeAttributes.SequentialLayout | ((isInternal) ? TypeAttributes.NotPublic : TypeAttributes.Public), t => t.IsStruct = true, TypeAttributes.VisibilityMask | TypeAttributes.LayoutMask);

        public static CodeTypeDeclaration AddSequentialLayoutStruct(this CodeTypeDeclaration parent, string name) => AddTypeDeclaration(parent, name, TypeAttributes.SequentialLayout | TypeAttributes.NestedPublic, t => t.IsStruct = true, TypeAttributes.VisibilityMask | TypeAttributes.LayoutMask);

        public static CodeTypeDeclaration AddSequentialLayoutNonPublicStruct(this CodeTypeDeclaration parent, string name, bool isProtected = false, bool isInternal = false) => AddTypeDeclaration(parent, name,
            TypeAttributes.SequentialLayout | ((isProtected) ? ((isInternal) ? TypeAttributes.NestedFamORAssem : TypeAttributes.NestedFamily) : ((isInternal) ? TypeAttributes.NestedAssembly : TypeAttributes.NestedPrivate)),
            t => t.IsStruct = true, TypeAttributes.VisibilityMask | TypeAttributes.LayoutMask);

        public static CodeMemberProperty AddProperty(this CodeTypeDeclaration parent, string name, CodeTypeReference type)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (name == null)
                throw new ArgumentNullException("name");
            if (!CodeGenerator.IsValidLanguageIndependentIdentifier(name))
                throw new ArgumentException("Invalid property name", "name");
            CodeMemberProperty property = new CodeMemberProperty();
            property.Name = name;
            property.Type = new CodeTypeReference()
        }

        public static bool IsPublic(this CodeTypeDeclaration type)
        {
            if (type == null)
                return false;
            TypeAttributes attr = type.TypeAttributes & TypeAttributes.VisibilityMask;
            return attr == TypeAttributes.Public || attr == TypeAttributes.NestedPublic;
        }

        public static bool IsPrivate(this CodeTypeDeclaration type) => type != null && (type.TypeAttributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPrivate;

        public static bool IsProtected(this CodeTypeDeclaration type) => type != null && (type.TypeAttributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamily;

        public static bool IsProtectedInternal(this CodeTypeDeclaration type)
        {
            if (type == null)
                return false;
            TypeAttributes attr = type.TypeAttributes & TypeAttributes.VisibilityMask;
            return attr == TypeAttributes.NestedFamANDAssem || attr == TypeAttributes.NestedFamORAssem;
        }

        public static bool IsInternal(this CodeTypeDeclaration type)
        {
            if (type == null)
                return false;
            TypeAttributes attr = type.TypeAttributes & TypeAttributes.VisibilityMask;
            return attr == TypeAttributes.NotPublic || attr == TypeAttributes.NestedAssembly;
        }
    }
}
