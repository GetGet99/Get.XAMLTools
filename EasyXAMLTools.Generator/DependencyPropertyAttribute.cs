using System;

namespace EasyXAMLTools;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
class DependencyPropertyAttribute : Attribute
{
    public DependencyPropertyAttribute(Type PropertyType, string Name)
    {

    }
    /// <summary>
    /// Whether to use Nullable Reference Type annotation or not
    /// </summary>
    public bool UseNullableReferenceType { get; set; } = false;
    /// <summary>
    /// Whether Changed Event should be generated
    /// </summary>
    public bool GenerateChangedEvent { get; set; } = true;
    /// <summary>
    /// Whether Local OnPropertyChanged Method should be generated
    /// </summary>
    public bool GenerateLocalOnPropertyChangedMethod { get; set; } = false;
    /// <summary>
    /// Whether Local OnPropertyChanged Method should be generated with paramter
    /// </summary>
    public bool LocalOnPropertyChangedMethodWithParameter { get; set; } = true;
    /// <summary>
    /// Whether Local OnPropertyChanged Method should be generated with partial modifier
    /// If null, It is equal to LocalOnPropertyChangedMethodWithParameter
    /// </summary>
    public bool? LocalOnPropertyChangedMethodWithPartial { get; set; } = null;
    /// <summary>
    /// Name of Changed Event (null for default name)
    /// </summary>
    public string? ChangedEventName { get; set; } = null;
    /// <summary>
    /// Name of Local OnPropertyChanged method (null for default name)
    /// </summary>
    public string? LocalOnPropertyChangedMethodName { get; set; } = null;
    /// <summary>
    /// Documentation As String to put on the property
    /// </summary>
    public string? Documentation { get; set; } = null;
}