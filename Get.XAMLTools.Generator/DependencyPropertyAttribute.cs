using System;

namespace Get.XAMLTools;

abstract class DependencyPropertyAttributeBase : Attribute
{
    /// <summary>
    /// Use custom default value
    /// If null, it is <c>default(PropertyType)</c>
    /// </summary>
    public string? DefaultValueExpression { get; set; } = null;
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

    /// <summary>
    /// Checks whether the element has changed before continuing with any logic
    /// </summary>
    public bool CheckForChanges { get; set; } = true;
    /// <summary>
    /// Whether to use <see cref="object.ReferenceEquals(object, object)"/> or <see cref="global::System.Collections.Generic.EqualityComparer{T}.Equals(T, T)"/> comparision (True = Reference)
    /// If null, Comparing Value Type uses Equals and Comparing Reference Type uses ReferenceEquals.
    /// If <seealso cref="CustomComparisonCodeForChanges"/> is not null, this property does nothing
    /// </summary>
    // use false instead of null 1. because string and 2. mostly it's the same anyway if object.Equals is not overridden
    public bool? ReferenceCompareForChanges { get; set; } = false;
    /// <summary>
    /// Set custom comparison code for changes
    /// </summary>
    public string? CustomComparisonCodeForChanges { get; set; } = null;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
class DependencyPropertyAttribute : DependencyPropertyAttributeBase
{
    public DependencyPropertyAttribute(Type PropertyType, string Name)
    {

    }
    /// <summary>
    /// Whether to use Nullable Reference Type annotation or not
    /// </summary>
    public bool UseNullableReferenceType { get; set; } = false;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
class DependencyPropertyAttribute<PropertyType> : DependencyPropertyAttributeBase
{
    public DependencyPropertyAttribute(string Name)
    {

    }
    /// <summary>
    /// Whether to use Nullable Reference Type annotation or not
    /// </summary>
    public bool UseNullableReferenceType { get; set; } = false;
}