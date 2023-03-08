using EasyCSharp.GeneratorTools.SyntaxCreator.Lines;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace EasyCSharp.GeneratorTools.SyntaxCreator.Members;

interface IType : IMember
{

}
record struct FullType(string TypeWithNamespace, bool Nullable = false) : ISyntax {
    static FullType Void = new("void");
    public FullType(ITypeSymbol typeSymbol) : this(typeSymbol.FullName(), typeSymbol.NullableAnnotation == NullableAnnotation.Annotated) { }

    public string StringRepresentaion => ToString();

    public override string ToString() => TypeWithNamespace;
}
abstract class Type : IType
{
    protected Type(string Name, SyntaxVisibility Visibility)
    {
        this.Name = Name;
        this.Visibility = Visibility;
    }
    public string Name { get; }
    public SyntaxVisibility Visibility { get; }
    public IDocumentation? Documentation { get; }
    public LinkedList<IMember> Members { get; } = new();
    public LinkedList<FullType> BaseType { get; } = new();

    protected abstract string TypeKindKeyword { get; }

    public string StringRepresentaion => ToString();

    protected virtual IEnumerable<string> GetKeywords()
    {
        if (Visibility.GetString() is string s)
            yield return s;
        yield return TypeKindKeyword;
    }
    public override string ToString()
    {
        return $$"""
                {{Documentation?.StringRepresentaion ?? "// No Documentation was provided"}}
                {{GetKeywords().JoinWith(" ")}} {{Name}} : {{BaseType.Select(x => x.TypeWithNamespace).JoinWith(", ")}} {
                    {{Members.Select(x => x.StringRepresentaion).JoinDoubleNewLine().IndentWOF(1)}}
                }
                """;
    }
}
class Class : Type
{
    public Class(string Name, SyntaxVisibility Visibility) : base(Name, Visibility) { }

    protected override string TypeKindKeyword => "partial";
}
class PartialClass : Class
{
    public PartialClass(string Name, SyntaxVisibility Visibility) : base(Name, Visibility) { }

    protected override string TypeKindKeyword => "partial class";
}
class Struct : Type
{
    public Struct(string Name, SyntaxVisibility Visibility) : base(Name, Visibility) { }

    protected override string TypeKindKeyword => "struct";
}
class PartialStruct : Struct
{
    public PartialStruct(string Name, SyntaxVisibility Visibility) : base(Name, Visibility) { }

    protected override string TypeKindKeyword => "partial struct";
}
