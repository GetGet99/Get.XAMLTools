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

namespace Get.XAMLTools;
[Generator]
[CopySource("DependencyPropertyAttributeTxt", typeof(DependencyPropertyAttributeBase))]
[AddAttributeConverter(typeof(DependencyPropertyAttributeBase), SampleObjectType = typeof(DependencyPropertyAttribute), ParametersAsString = "typeof(int), \"tempstr\"")]
[AddAttributeConverter(typeof(DependencyPropertyAttribute), ParametersAsString = "typeof(int), \"tempstr\"")]
[AddAttributeConverter(typeof(DependencyPropertyAttribute<int>), ParametersAsString = "\"tempstr\"", StructName = "DependencyPropertyAttributeGenericWrapper", MethodName = "AttributeDataToDependencyPropertyAttributeGeneric")]
//[AddAttributeConverter(typeof(DependencyPropertyAttribute<int>), ParametersAsString = "\"tempstr\"")]
partial class DependencyPropertyGenerator : AttributeBaseGenerator<
    DependencyPropertyAttributeBase,
    DependencyPropertyGenerator.DependencyPropertyAttributeBaseWarpper,
    ClassDeclarationSyntax,
    INamedTypeSymbol
>
{
    const string DependencyProperty = "global::Microsoft.UI.Xaml.DependencyProperty";
    const string PropertyMetadata = "global::Microsoft.UI.Xaml.PropertyMetadata";
    partial class DependencyPropertyAttributeBaseWarpper
    {
        public ITypeSymbol PropertyType { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool UseNullableReferenceType { get; set; } = default;
    }
    protected override void OnInitialize(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource($"{typeof(DependencyPropertyAttributeBase).FullName}.g.cs", DependencyPropertyAttributeTxt);
    }
    protected override string? OnPointVisit(GeneratorSyntaxContext genContext, ClassDeclarationSyntax syntaxNode, INamedTypeSymbol symbol, (AttributeData Original, DependencyPropertyAttributeBaseWarpper Wrapper)[] attributeData)
    {
        return GetCode(genContext, syntaxNode, symbol, attributeData).JoinDoubleNewLine();
    }
    IEnumerable<string> GetCode(GeneratorSyntaxContext genContext, ClassDeclarationSyntax syntaxNode, INamedTypeSymbol symbol, (AttributeData Original, DependencyPropertyAttributeBaseWarpper Wrapper)[] attributeData)
    {
        //Debugger.Launch();
        foreach (var (a, attr) in attributeData)
        {
            string DependencyPropertyName = $"{attr.Name}Property";
            string ChangedEventName = $"{attr.Name}Changed";
            string LocalOnPropertyChangedName = attr.LocalOnPropertyChangedMethodName ?? $"On{attr.Name}Changed";
            if (attr.GenerateChangedEvent)
                yield return $"public event global::System.EventHandler? {ChangedEventName};";
            var ParamTypeNameWithAnnotation = attr.PropertyType.FullName();
            if (attr.UseNullableReferenceType && !attr.PropertyType.IsValueType) ParamTypeNameWithAnnotation += "?";
            if (attr.GenerateLocalOnPropertyChangedMethod && (attr.LocalOnPropertyChangedMethodWithPartial ?? attr.LocalOnPropertyChangedMethodWithParameter))
            {
                if (attr.LocalOnPropertyChangedMethodWithParameter)
                    yield return $"partial void {LocalOnPropertyChangedName}({ParamTypeNameWithAnnotation} oldValue, {ParamTypeNameWithAnnotation} newValue);";
                else
                    yield return $"partial void {LocalOnPropertyChangedName}();";
            }
            yield return $$"""
                /// <summary>
                /// Identifies the {{attr.Name}} dependency property.
                /// </summary>
                public static {{DependencyProperty}} {{DependencyPropertyName}} = {{DependencyProperty}}.Register(
                    nameof({{attr.Name}}),
                    typeof({{attr.PropertyType.FullName()}}),
                    typeof({{symbol.FullName()}}),
                    new {{PropertyMetadata}}(
                        {{attr.DefaultValueExpression ?? $"default({attr.PropertyType.FullNameWithoutAnnotation()})"}},
                        static (d, e) => {
                            var @this = (({{symbol.FullName()}})d);
                            {{(attr.GenerateLocalOnPropertyChangedMethod ?
                                (attr.LocalOnPropertyChangedMethodWithParameter ?
                                $"""
                                @this.{LocalOnPropertyChangedName}(
                                    ({attr.PropertyType.FullName()})e.OldValue,
                                    ({attr.PropertyType.FullName()})e.NewValue
                                );
                                """.IndentWOF(3) :
                                $"@this.{LocalOnPropertyChangedName}();"
                                ) :
                                "// GenerateLocalOnPropertyChangedMethod is false"
                                )}}
                            {{(attr.GenerateChangedEvent ?
                                $"@this.{ChangedEventName}?.Invoke(d, new());" :
                                "// GeneratedChangedEvent is false"
                            )}}
                        }
                    )
                );
                """;
            yield return new Property(SyntaxVisibility.Public, new(ParamTypeNameWithAnnotation), attr.Name)
            {
                Documentation = attr.Documentation is null ? null : new CustomDocumentation(attr.Documentation),
                Get =
                {
                    Code = {
                        new Return(
                            new Cast(
                                new(ParamTypeNameWithAnnotation),
                                new MethodCall("GetValue", new CallParameter(new Variable(DependencyPropertyName)))
                            )
                        )
                    }
                },
                Set =
                {
                    Code = {
                        x => {
                            if (attr.CheckForChanges)
                            {
                                var methodCall = attr.CustomComparisonCodeForChanges ??
                                (
                                    attr.ReferenceCompareForChanges ?? !attr.PropertyType.IsValueType ?
                                    $"object.ReferenceEquals({attr.Name}, value)" : $"global::System.Collections.Generic.EqualityComparer<{attr.PropertyType.FullName()}>.Default.Equals({attr.Name}, value)"
                                );
                                x.AddLast(new CustomLine(
                                    $"""
                                    if ({methodCall}) return;
                                    """
                                ));
                            }
                        },
                        new MethodCall("SetValue",
                            new CallParameter(new Variable(DependencyPropertyName)),
                            new CallParameter(new Variable("value"))
                        ).EndLine()
                    }
                }
            }.StringRepresentaion;
        }
    }

    protected override DependencyPropertyAttributeBaseWarpper TransformAttribute(AttributeData attributeData, Compilation compilation)
    {
        var attr = AttributeDataToDependencyPropertyAttributeBase(attributeData, compilation);
        if (attributeData.AttributeClass?.TypeParameters.Length > 0)
        {
            var attr2 = AttributeDataToDependencyPropertyAttributeGeneric(attributeData, compilation);
            attr.PropertyType = attr2.PropertyType;
            attr.Name = attr2.Name;
            attr.UseNullableReferenceType = attr2.UseNullableReferenceType;
        }
        else
        {
            var attr2 = AttributeDataToDependencyPropertyAttribute(attributeData, compilation);
            attr.PropertyType = attr2.PropertyType;
            attr.Name = attr2.Name;
            attr.UseNullableReferenceType = attr2.UseNullableReferenceType;
        }
        return attr;
    }
}