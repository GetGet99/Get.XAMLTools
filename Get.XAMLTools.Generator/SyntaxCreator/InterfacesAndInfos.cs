using EasyCSharp.GeneratorTools.SyntaxCreator.Attributes;
using EasyCSharp.GeneratorTools.SyntaxCreator.Expression;
using EasyCSharp.GeneratorTools.SyntaxCreator.Members;
using System.Collections.Generic;
using System.Linq;

namespace EasyCSharp.GeneratorTools.SyntaxCreator;

enum SyntaxVisibility : byte { Default = default, Public, Protected, Private, DoNotGenerate }

interface ISyntax
{
    string StringRepresentaion { get; }
}

record struct ParameterDefinition(FullType Type, string Name) : ISyntax
{
    public LinkedList<Attribute> Attributes { get; } = new();
    IEnumerable<string> SyntaxHelper()
    {
        if (Attributes.Count > 0)
            yield return Attributes.Select(x => x.StringRepresentaion).JoinWith(" ");
        yield return Type.StringRepresentaion;
        yield return Name;
    }
    public string StringRepresentaion => SyntaxHelper().JoinWith(" ");
    public override string ToString() => StringRepresentaion;
}
record struct CallParameter(IExpression value, string? Name = null) : ISyntax
{
    public LinkedList<Attribute> Attributes { get; } = new();
    public string StringRepresentaion => Name is null ? value.StringRepresentaion : $"{Name}: {value.StringRepresentaion}";
    public override string ToString() => StringRepresentaion;
}