using System;
using System.Diagnostics;
using EasyCSharp;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace EasyXAMLTools;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
partial class Sample
{
    [DependencyProperty]
    HorizontalAlignment _HorizontalAlignment;

    [DependencyProperty(Visibility = PropertyVisibility.Protected)]
    VerticalAlignment _VerticalAlignment;

    [DependencyProperty(PropertyName = "MinimumValue", OnChanged = nameof(OnValueChange))]
    float _MinValue;

    [Event(typeof(PropertyChangedCallback))]
    static void OnValueChange()
    {
        Debug.WriteLine("MinimumValue Changed");
    }
}

