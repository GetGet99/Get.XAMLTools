#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyXAMLTools;
[AttributeUsage(AttributeTargets.Class)]
class AttachedPropertyAttribute<T> : Attribute
{
    public AttachedPropertyAttribute(string Name)
    {

    }
}