using Manuela.Theming;

namespace Manuela.Styling;

public class Style
{
    public ManuelaSettersDictionary Setters { get; } = [];

    public Brush CustomBackground { set => Setters[ManuelaProperty.Background] = value; }
    public UIBrush Background { set => Setters[ManuelaProperty.Background] = value; }

    public Thickness Margin { set => Setters[ManuelaProperty.Margin] = value; }
    public Thickness Padding { set { Setters[ManuelaProperty.Padding] = value; } }

    public Brush CustomBorderBrush { set => Setters[ManuelaProperty.BorderColor] = value; }
    public Color CustomBorderColor { set => Setters[ManuelaProperty.BorderColor] = value; }
    public UIBrush BorderColor { set => Setters[ManuelaProperty.BorderColor] = value; }

    public double BorderThickness { set => Setters[ManuelaProperty.BorderThickness] = value; }

    public int BorderRadius { set => Setters[ManuelaProperty.BorderRadius] = value; }

    public Shadow CustomShadow { set => Setters[ManuelaProperty.Shadow] = value; }
    public UISize Shadow { set => Setters[ManuelaProperty.Shadow] = value; }

    public double TextSize { set => Setters[ManuelaProperty.TextSize] = value; }
    public double LineHeight { set => Setters[ManuelaProperty.LineHeight] = value; }
    public FontAttributes FontAttributes { set => Setters[ManuelaProperty.FontAttributes] = value; }
    public TextAlignment VerticalTextAlign { set => Setters[ManuelaProperty.VerticalTextAlign] = value; }
    public TextAlignment HorizontalTextAlign { set => Setters[ManuelaProperty.HorizontalTextAlign] = value; }
    public Color CustomTextColor { set => Setters[ManuelaProperty.TextColor] = value; }
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

    public IList<Setter> AsMauiSetters(BindableObject bindable) => Setters.AsMauiSetters(bindable);
}
