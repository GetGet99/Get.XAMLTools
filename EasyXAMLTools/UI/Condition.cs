#nullable enable
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using EasyCSharp;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;

namespace EasyXAMLTools;

/// <summary>
/// Creates a Condition Element, to aply template on True or False
/// </summary>
[DependencyProperty(
    typeof(bool),
    "Value",
    GenerateLocalOnPropertyChangedMethod = true,
    LocalOnPropertyChangedMethodName = "UpdateTemplate",
    LocalOnPropertyChangedMethodWithParameter = false,
    Documentation =
    """
    /// <summary>
    /// Gets or Set the Value of the condition and update the template
    /// </summary>
    """
)]
[DependencyProperty(
    typeof(DataTemplate),
    "OnTrue",
    GenerateLocalOnPropertyChangedMethod = true,
    LocalOnPropertyChangedMethodName = "UpdateTemplate",
    LocalOnPropertyChangedMethodWithParameter = false,
    Documentation =
    """
    /// <summary>
    /// Gets or Set the <see cref="DataTemplate"/> to use when <see cref="Value"/> is true.
    /// </summary>
    """
)]
[DependencyProperty(
    typeof(DataTemplate),
    "OnFalse",
    GenerateLocalOnPropertyChangedMethod = true,
    LocalOnPropertyChangedMethodName = "UpdateTemplate",
    LocalOnPropertyChangedMethodWithParameter = false,
    Documentation =
    """
    /// <summary>
    /// Gets or Set the <see cref="DataTemplate"/> to use when <see cref="Value"/> is false.
    /// </summary>
    """
)]
public partial class Condition : ContentControl
{
    void UpdateTemplate()
    {
        var NewTempalte = Value ? OnTrue : OnFalse;
        if (ContentTemplate != NewTempalte)
            ContentTemplate = NewTempalte;
    }
}