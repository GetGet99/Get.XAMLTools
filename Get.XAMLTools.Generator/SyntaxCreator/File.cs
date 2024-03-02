using EasyCSharp.GeneratorTools.SyntaxCreator.Members;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace EasyCSharp.GeneratorTools.SyntaxCreator;
record struct Namespace(string NamespaceString) : ISyntax
{
    public Namespace(INamespaceSymbol symbol) : this(symbol.ToString()) { }

    public string StringRepresentaion => NamespaceString;
}
class SingleNamespaceFile : ISyntax
{
    public SingleNamespaceFile(Namespace @namespace)
    {
        Namespace = @namespace;
    }

    public Namespace Namespace { get; set; }
    public LinkedList<Namespace> Usings { get; } = new();
    public LinkedList<IType> Types { get; } = new();

    public string StringRepresentaion => ToString();
    public override string ToString()
    {
        return $$"""
                #nullable enable
                {{Usings.Select(x => $"using {x};").JoinNewLine()}}

                namespace {{Namespace}} {
                    {{Types.Select(x => x.StringRepresentaion).JoinDoubleNewLine()}}
                }
                """;
    }
}