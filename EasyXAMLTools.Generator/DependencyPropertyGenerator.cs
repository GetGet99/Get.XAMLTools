using CopySourceGenerator;
using EasyCSharp;
using EasyCSharp.GeneratorTools;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace EasyXAMLTools;
[AddAttributeConverter(typeof(DependencyPropertyAttribute))]
[CopySource("DependencyPropertyAttributeSource", typeof(DependencyPropertyAttribute))]
[Generator]
partial class DependencyPropertyGenerator : PropertyGeneratorBase<DependencyPropertyAttribute>
{
    protected override void OnInitialize(GeneratorInitializationContext context)
    {
        context.RegisterForPostInitialization(
            x => x.AddSource($"{typeof(DependencyPropertyAttribute)}.g.cs", DependencyPropertyAttributeSource)
        );
    }

    const string DependencyObjectNS = "Microsoft.UI.Xaml.DependencyObject";
    const string DependencyPropertyNS = "Microsoft.UI.Xaml.DependencyProperty";
    const string PropertyMetadataNS = "Microsoft.UI.Xaml.PropertyMetadata";
    protected override string FileNameOverride(string ClaseName) => $"{ClaseName}.GeneratedDependencyProperty.g.cs";
    protected override string GetCustomLogicOverride(IFieldSymbol FieldSymbol, string PropertyName, string Original)
        => $"{FieldSymbol.Name} = ({FieldSymbol.Type})GetValue({PropertyName}Property)";

    protected override string OnSet(IFieldSymbol FieldSymbol, string PropertyName, AttributeData attributeData)
        => $"SetValue({PropertyName}Property, {FieldSymbol.Name} = value);";

    protected override string BeforePropertyLogic(GeneratorExecutionContext context, IFieldSymbol fieldSymbol, string PropertyName, AttributeData attributeData)
    {
        var attr = AttributeDataToDependencyPropertyAttribute(attributeData, context.Compilation);
        return $"""
                /// <summary>
                /// <inheritdoc cref=""{fieldSymbol.Name}""/>
                /// </summary>
                {GetVisiblity(attr.Visibility)}static readonly {DependencyPropertyNS} {attr.PropertyName}Property
                    = {DependencyPropertyNS}.Register(
                        ""{attr.PropertyName}Property"",
                        typeof({fieldSymbol.Type}),
                        typeof({fieldSymbol.ContainingType}),
                        new {PropertyMetadataNS}({(fieldSymbol.DeclaringSyntaxReferences[0].GetSyntax() as VariableDeclaratorSyntax)
                            ?.Initializer?.Value.ToFullString() ?? $"default({fieldSymbol.Type})"}, {attr.OnChanged ?? "null"})
                    );
                """;
    }
    protected override string ClassHeadLogic(GeneratorExecutionContext context, INamedTypeSymbol classSymbol)
    {
        return classSymbol.BaseType?.Equals(
            context.Compilation.GetTypeByMetadataName(DependencyObjectNS),
            SymbolEqualityComparer.Default) ?? false ? "" : $" : {DependencyObjectNS}";
    }
    static string? GetVisiblity(GeneratorVisibility propertyVisibility)
        => propertyVisibility switch
        {
            GeneratorVisibility.Default => "",
            GeneratorVisibility.DoNotGenerate => null,
            GeneratorVisibility.Public => $"public ",
            GeneratorVisibility.Private => $"private ",
            GeneratorVisibility.Protected => $"protected ",
            _ => throw new ArgumentOutOfRangeException()
        };
}