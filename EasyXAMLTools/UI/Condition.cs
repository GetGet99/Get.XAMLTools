#nullable enable
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using EasyCSharp;
using System.Diagnostics.CodeAnalysis;

namespace EasyXAMLTools;

/// <summary>
/// Creates a Condition Element, to aply template on True or False
/// </summary>
public partial class Condition : ContentControl
{
    /// <summary>
    /// Set Value of the condition and update the template
    /// </summary>
    [AutoNotifyProperty(PropertyName = "ValueNullable",
        CustomType = typeof(bool?),
        CustomGetExpression = $"(bool?){nameof(ValueNullable)}",
        CustomSetExpression = $"value ?? false",
        OnChanged = nameof(UpdateTemplate))]
    [AutoNotifyProperty(OnChanged = nameof(UpdateTemplate))]
    bool _Value = false;

    /// <summary>
    /// The <see cref="DataTemplate"/> to use when <see cref="Value"/> is true.
    /// </summary>
    
    [AutoNotifyProperty(OnChanged = nameof(UpdateTemplate))]
    DataTemplate? _OnTrue;
    
    /// <summary>
    /// The <see cref="DataTemplate"/> to use when <see cref="Value"/> is false.
    /// </summary>
    [AutoNotifyProperty(OnChanged = nameof(UpdateTemplate))]
    DataTemplate? _OnFalse;
    
    void UpdateTemplate()
    {
        var NewTempalte = _Value ? _OnTrue : _OnFalse;
        if (ContentTemplate != NewTempalte)
            ContentTemplate = NewTempalte;
    }
}
