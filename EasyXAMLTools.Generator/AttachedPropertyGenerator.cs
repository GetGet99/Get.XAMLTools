using CopySourceGenerator;
using EasyCSharp;
using EasyCSharp.GeneratorTools;
using EasyCSharp.GeneratorTools.SyntaxCreator;
using EasyCSharp.GeneratorTools.SyntaxCreator.Members;
using EasyCSharp.GeneratorTools.SyntaxCreator.Lines;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using EasyCSharp.GeneratorTools.SyntaxCreator.Expression;

namespace EasyXAMLTools;
[Generator]
[CopySource("AttachedPropertyAttributeTxt", typeof(AttachedPropertyAttribute))]
[AddAttributeConverter(typeof(AttachedPropertyAttribute), ParametersAsString = "typeof(int), \"tempstr\"")]
//[AddAttributeConverter(typeof(DependencyPropertyAttribute<int>), ParametersAsString = "\"tempstr\"")]
partial class AttachedPropertyGenerator : AttributeBaseGenerator<
    AttachedPropertyAttribute,
    AttachedPropertyGenerator.AttachedPropertyAttributeWarpper,
    ClassDeclarationSyntax,
    INamedTypeSymbol
>
{
    const string DependencyProperty = "global::Microsoft.UI.Xaml.DependencyProperty";
    const string DependencyObject = "global::Microsoft.UI.Xaml.DependencyObject";
    const string PropertyMetadata = "global::Microsoft.UI.Xaml.PropertyMetadata";
    protected override void OnInitialize(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource($"{typeof(AttachedPropertyAttribute).FullName}.g.cs", AttachedPropertyAttributeTxt);
    }
    protected override string? OnPointVisit(GeneratorSyntaxContext genContext, ClassDeclarationSyntax syntaxNode, INamedTypeSymbol symbol, (AttributeData Original, AttachedPropertyAttributeWarpper Wrapper)[] attributeData)
    {
        return GetCode(genContext, syntaxNode, symbol, attributeData).JoinDoubleNewLine();
    }
    IEnumerable<string> GetCode(GeneratorSyntaxContext genContext, ClassDeclarationSyntax syntaxNode, INamedTypeSymbol symbol, (AttributeData Original, AttachedPropertyAttributeWarpper Wrapper)[] attributeData)
    {
        //Debugger.Launch();
        foreach (var (a, attr) in attributeData)
        {
            string DependencyPropertyName = $"{attr.Name}Property";
            string ChangedEventName = $"{attr.Name}Changed";
            string LocalOnPropertyChangedName = attr.LocalOnPropertyChangedMethodName ?? $"On{attr.Name}Changed";
            var DependencyObjectType = attr.DependencyObjectType?.FullName() ?? DependencyObject;
            var ParamTypeName = attr.PropertyType.FullName(attr.UseNullableReferenceType);
            if (attr.GenerateLocalOnPropertyChangedMethod && (attr.LocalOnPropertyChangedMethodWithPartial ?? attr.LocalOnPropertyChangedMethodWithPropertyValueParameter))
            {
                if (attr.LocalOnPropertyChangedMethodWithPropertyValueParameter)
                    yield return $"static partial void {LocalOnPropertyChangedName}({DependencyObjectType} obj, {ParamTypeName} oldValue, {ParamTypeName} newValue);";
                else
                    yield return $"static partial void {LocalOnPropertyChangedName}({DependencyObjectType} obj);";
            }
            yield return $$"""
                /// <summary>
                /// Identifies the {{attr.Name}} dependency property.
                /// </summary>
                public static readonly {{DependencyProperty}} {{DependencyPropertyName}} = {{DependencyProperty}}.RegisterAttached(
                    "{{attr.Name}}",
                    typeof({{attr.PropertyType.FullNameWithoutAnnotation()}}),
                    typeof({{symbol.FullNameWithoutAnnotation()}}),
                    new {{PropertyMetadata}}(
                        default({{attr.PropertyType.FullNameWithoutAnnotation()}}),
                        static (d, e) => {
                            var @this = (({{DependencyObjectType}})d);
                            {{(attr.GenerateLocalOnPropertyChangedMethod ?
                                (attr.LocalOnPropertyChangedMethodWithPropertyValueParameter ?
                                $"""
                                {LocalOnPropertyChangedName}(
                                    @this,
                                    ({attr.PropertyType.FullName(attr.UseNullableReferenceType)})e.OldValue,
                                    ({attr.PropertyType.FullName(attr.UseNullableReferenceType)})e.NewValue
                                );
                                """.IndentWOF(3) :
                                $"{LocalOnPropertyChangedName}(@this);"
                                ) :
                                "// GenerateLocalOnPropertyChangedMethod is false"
                                )}}
                        }
                    )
                );
                //
                // Summary:
                //     Gets the value of the {{symbol.Name}}.{{attr.Name}} XAML attached property from the specified <see cref="{{DependencyObjectType}}"/>.
                //
                // Parameters:
                //   obj:
                //     The object from which to read the property value.
                //
                // Returns:
                //     The value of the {{symbol.Name}}.{{attr.Name}} XAML attached property on the target element.
                public static {{attr.PropertyType.FullName(attr.UseNullableReferenceType)}} Get{{attr.Name}}({{DependencyObjectType}} obj)
                    => ({{attr.PropertyType.FullName(attr.UseNullableReferenceType)}})obj.GetValue({{DependencyPropertyName}});
                
                //
                // Summary:
                //     Gets the value of the {{symbol.Name}}.{{attr.Name}} XAML attached property from the specified <see cref="{{DependencyObjectType}}"/>.
                //
                // Parameters:
                //   obj:
                //     The object from which to read the property value.
                //
                //   value:
                //     The property value to set.
                public static void Set{{attr.Name}}({{DependencyObjectType}} obj, {{attr.PropertyType.FullName(attr.UseNullableReferenceType)}} value)
                    => obj.SetValue({{DependencyPropertyName}}, value);
                """;
        }
    }
    protected override AttachedPropertyAttributeWarpper TransformAttribute(AttributeData attributeData, Compilation compilation)
    {
        var attr = AttributeDataToAttachedPropertyAttribute(attributeData, compilation);
        return attr;
    }
}