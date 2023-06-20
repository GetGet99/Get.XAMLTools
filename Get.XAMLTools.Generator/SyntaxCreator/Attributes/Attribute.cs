using EasyCSharp.GeneratorTools.SyntaxCreator.Members;
using System.Collections.Generic;
using System.Linq;

namespace EasyCSharp.GeneratorTools.SyntaxCreator.Attributes;
interface IAttribute : ISyntax { }
record struct Attribute(FullType AttributeType) : IAttribute
{
    public LinkedList<CallParameter> Parameters = new();
    public string StringRepresentaion => $"[{AttributeType.StringRepresentaion}({Parameters.Select(x => x.StringRepresentaion).JoinWith(", ")})]";
}
record struct CustomAttribute(string StringRepresentaion) : IAttribute { }