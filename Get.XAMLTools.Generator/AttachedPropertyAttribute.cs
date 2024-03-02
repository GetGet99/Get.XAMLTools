#nullable enable
using System;

namespace Get.XAMLTools;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
class AttachedPropertyAttribute : Attribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="PropertyType">Property Type</param>
    /// <param name="Name">Property Name</param>
    /// <param name="DependencyObjectType">Dependency Object Type that PropertyType can be set for. If null, uses DependencyObject</param>
    public AttachedPropertyAttribute(Type PropertyType, string Name, Type? DependencyObjectType = null)
    {

    }
    /// <summary>
    /// Use custom default value
    /// If null, it is <c>default(PropertyType)</c>
    /// </summary>
    public string? DefaultValueExpression { get; set; } = null;
    /// <summary>
    /// Whether to use Nullable Reference Type annotation or not
    /// </summary>
    public bool UseNullableReferenceType { get; set; } = false;
    /// <summary>
    /// Whether Local OnPropertyChanged Method should be generated
    /// </summary>
    public bool GenerateLocalOnPropertyChangedMethod { get; set; } = false;
    /// <summary>
    /// Whether Local OnPropertyChanged Method should be generated with property value paramter
    /// </summary>
    public bool LocalOnPropertyChangedMethodWithPropertyValueParameter { get; set; } = true;
    /// <summary>
    /// Whether Local OnPropertyChanged Method should be generated with partial modifier
    /// If null, It is equal to LocalOnPropertyChangedMethodWithParameter
    /// </summary>
    public bool? LocalOnPropertyChangedMethodWithPartial { get; set; } = null;
    /// <summary>
    /// Name of Local OnPropertyChanged method (null for default name)
    /// </summary>
    public string? LocalOnPropertyChangedMethodName { get; set; } = null;
    /// <summary>
    /// Documentation As String to put on the property
    /// </summary>
    public string? Documentation { get; set; } = null;
}