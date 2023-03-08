namespace EasyCSharp.GeneratorTools.SyntaxCreator.Lines;

interface ILine : ISyntax { }

interface IComment : ILine { }
interface IDocumentation : IComment { }
record struct CustomDocumentation(string StringRepresentaion) : IDocumentation
{
    
}