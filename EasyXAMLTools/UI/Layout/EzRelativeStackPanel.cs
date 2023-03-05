using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;
using Windows.Foundation;

namespace EasyXAMLTools;

public class EzRelativeStackPanel : Panel
{
    public readonly static DependencyProperty OrientationProperty = DependencyProperty.Register(
        nameof(Orientation),
        typeof(Orientation),
        typeof(EzRelativeStackPanel),
        new(Orientation.Vertical, (o, e) => ((EzRelativeStackPanel)o).InvalidateArrange())
    );
    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value);
    }
    public readonly static DependencyProperty RelativeSizeProperty = DependencyProperty.RegisterAttached(
        "RelativeSize",
        typeof(double),
        typeof(EzRelativeStackPanel),
        new(1d, (o, e) => (VisualTreeHelper.GetParent(o) as EzRelativeStackPanel)?.InvalidateArrange())
    );
    public static double GetRelativeSize(DependencyObject obj)
        => (double)obj.GetValue(RelativeSizeProperty);
    public static void SetRelativeSize(DependencyObject obj, double value)
        => obj.SetValue(RelativeSizeProperty, value);
    protected override Size MeasureOverride(Size availableSize)
    {
        return availableSize;
    }
    (double Adaptive, double Full) ToOrientedSize(Size s)
        => Orientation == Orientation.Vertical ? (s.Height, s.Width) : (s.Width, s.Height);
    Size SizeFromOriented(double adaptive, double full)
        => Orientation == Orientation.Vertical ? new(full, adaptive) : new(adaptive, full);
    Point PointFromOriented(double adaptive, double full)
        => Orientation == Orientation.Vertical ? new(full, adaptive) : new(adaptive, full);
    protected override Size ArrangeOverride(Size finalSize)
    {
        try
        {
            double Used = 0;
            var childrenAndRS = (from x in Children select (UIElement: x, RelativeSize: GetRelativeSize(x))).ToArray();
            var totalRS = childrenAndRS.Sum(x => x.RelativeSize);
            var ChildrenCount = Children.Count;
            var finalOrientedSize = ToOrientedSize(finalSize);
            var Multipier = finalOrientedSize.Adaptive / totalRS;
            foreach (var (child, RelativeSize) in childrenAndRS)
            {
                var RequestedSize = Multipier * RelativeSize;
                child.Measure(SizeFromOriented(RequestedSize, finalOrientedSize.Full));
                child.Arrange(new(
                        PointFromOriented(Used, 0),
                        SizeFromOriented(RequestedSize, finalOrientedSize.Full))
                    );
                Used += RequestedSize;
            }
            return SizeFromOriented(Math.Max(Used, 0), finalOrientedSize.Full);
        } catch
        {
            return finalSize;
        }
    }
}