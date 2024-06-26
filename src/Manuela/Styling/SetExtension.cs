﻿// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using System.ComponentModel;
using Manuela.Styling;
using Manuela.Theming;
using Microsoft.Maui.Layouts;

namespace Manuela;

public class SetExtension : IMarkupExtension<ManuelaSettersDictionary>, INotifyPropertyChanged
{
    public ManuelaSettersDictionary Setters { get; } = [];

    public UIBrush Background { set => Setters[ManuelaProperty.Background] = value; }
    public UIBrush BorderColor { set => Setters[ManuelaProperty.BorderColor] = value; }
    public UISize Shadow { set => Setters[ManuelaProperty.Shadow] = value; }

    public Thickness Margin { set => Setters[ManuelaProperty.Margin] = value; }
    public Thickness Padding { set { Setters[ManuelaProperty.Padding] = value; } }
    public double BorderThickness { set => Setters[ManuelaProperty.BorderThickness] = value; }
    public int BorderRadius { set => Setters[ManuelaProperty.BorderRadius] = value; }

    public double TextSize { set => Setters[ManuelaProperty.TextSize] = value; }
    public double LineHeight { set => Setters[ManuelaProperty.LineHeight] = value; }
    public FontAttributes FontAttributes { set => Setters[ManuelaProperty.FontAttributes] = value; }
    public TextAlignment VerticalTextAlign { set => Setters[ManuelaProperty.VerticalTextAlign] = value; }
    public TextAlignment HorizontalTextAlign { set => Setters[ManuelaProperty.HorizontalTextAlign] = value; }

    public UIColor TextColor { set => Setters[ManuelaProperty.TextColor] = value; }
    public TextDecorations TextDecoration { set => Setters[ManuelaProperty.TextDecoration] = value; }

    public LayoutOptions VerticalOptions { set => Setters[ManuelaProperty.VerticalOptions] = value; }
    public LayoutOptions HorizontalOptions { set => Setters[ManuelaProperty.HorizontalOptions] = value; }

    public double Opacity { set => Setters[ManuelaProperty.Opacity] = value; }
    public double Width { set => Setters[ManuelaProperty.Width] = value; }
    public double Height { set => Setters[ManuelaProperty.Height] = value; }
    public double XAnchor { set => Setters[ManuelaProperty.XAnchor] = value; }
    public double YAnchor { set => Setters[ManuelaProperty.YAnchor] = value; }
    public double TranslateX { set => Setters[ManuelaProperty.TranslateX] = value; }
    public double TranslateY { set => Setters[ManuelaProperty.TranslateY] = value; }
    public double Rotation { set => Setters[ManuelaProperty.Rotation] = value; }
    public double RotationX { set => Setters[ManuelaProperty.RotationX] = value; }
    public double RotationY { set => Setters[ManuelaProperty.RotationY] = value; }
    public double Scale { set => Setters[ManuelaProperty.Scale] = value; }
    public double ScaleX { set => Setters[ManuelaProperty.ScaleX] = value; }
    public double ScaleY { set => Setters[ManuelaProperty.ScaleY] = value; }
    public double MaxWidth { set => Setters[ManuelaProperty.MaxWidth] = value; }
    public double MaxHeight { set => Setters[ManuelaProperty.MaxHeight] = value; }
    public double MinWidth { set => Setters[ManuelaProperty.MinWidth] = value; }
    public double MinHeight { set => Setters[ManuelaProperty.MinHeight] = value; }
    public bool Visible { set => Setters[ManuelaProperty.Visible] = value; }
    public Style Style { set => Setters[ManuelaProperty.Style] = value; }
    public Rect AbsoluteLayoutBounds { set => Setters[ManuelaProperty.AbsoluteLayoutBounds] = value; }
    public AbsoluteLayoutFlags AbsoluteLayoutFlags { set => Setters[ManuelaProperty.AbsoluteLayoutFlags] = value; }
    public string ImageSource { set => Setters[ManuelaProperty.ImageSource] = value; }
    public StackOrientation StackOrientation { set => Setters[ManuelaProperty.StackOrientation] = value; }
    public double StackSpacing { set => Setters[ManuelaProperty.StackSpacing] = value; }
    public bool IsEnabled { set => Setters[ManuelaProperty.IsEnabled] = value; }
    public int Columns { set => Setters[ManuelaProperty.Columns] = value; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ManuelaSettersDictionary ProvideValue(IServiceProvider serviceProvider)
    {
        return Setters;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}
