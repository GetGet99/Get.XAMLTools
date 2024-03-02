using EasyCSharp.GeneratorTools.SyntaxCreator.Expression;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCSharp.GeneratorTools.SyntaxCreator.Lines;

record struct CustomLine(string StringRepresentaion) : ILine { }

struct BlankLine : ILine
{
    public string StringRepresentaion => "";
}

record struct Comment(string CommentText) : ILine
{
    public string StringRepresentaion => $"// {CommentText}";
}

record struct Return(IExpression Expression) : ILine
{
    public string StringRepresentaion => $"return {Expression};";
    public override string ToString() => StringRepresentaion;
    public ILine AsLine()
    {
        throw new System.NotImplementedException();
    }
}