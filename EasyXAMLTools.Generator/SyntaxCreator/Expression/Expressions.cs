using EasyCSharp.GeneratorTools.SyntaxCreator.Lines;
using EasyCSharp.GeneratorTools.SyntaxCreator.Members;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyCSharp.GeneratorTools.SyntaxCreator.Expression;

interface IExpression : ISyntax
{
}

interface ILineExpression : IExpression
{
    ILine EndLine();
}

record struct Variable(string VariableName) : IExpression
{
    public string StringRepresentaion => VariableName;

    //public static implicit operator Variable(string VariableName) => new(VariableName);
    public override string ToString() => StringRepresentaion;

    public void Invoke(params IExpression[] Expressions) => Invoke(Expressions.AsEnumerable());
    public void Invoke(IEnumerable<IExpression> Expressions)
    {
        
    }
}

record struct Assign(Variable Variable, IExpression Expression) : ILineExpression
{
    public string StringRepresentaion => $"{Variable} = {Expression}";
    public override string ToString() => StringRepresentaion;
    public ILine EndLine() => new CustomLine($"{StringRepresentaion};");
}

record struct Cast(FullType Type, IExpression Expression) : ILineExpression
{
    public string StringRepresentaion => $"({Type})({Expression})";
    public override string ToString() => StringRepresentaion;
    public ILine EndLine() => new CustomLine($"{StringRepresentaion};");
}

record struct MethodCall(string MethodName, IEnumerable<CallParameter> Parameters) : ILineExpression
{
    public MethodCall(string MethodName, params CallParameter[] Parameters) : this(MethodName, Parameters.AsEnumerable()) { }
    public string StringRepresentaion => $"{MethodName}({(from param in Parameters select param.StringRepresentaion).JoinWith(", ")})";
    public override string ToString() => StringRepresentaion;
    public ILine EndLine() => new CustomLine($"{StringRepresentaion};");
}

record struct CustomExpression(string StringRepresentaion) : IExpression {
    public override string ToString() => StringRepresentaion;
}
static class Expressions
{
    public static IExpression This => new CustomExpression("this");
}