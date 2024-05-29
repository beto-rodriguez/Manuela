// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling;

namespace Manuela;

[ContentProperty(nameof(Properties))]
public class TransitionsExtension : IMarkupExtension<TransitionsCollection>
{
    private Transition? _allTransition;
    private readonly Dictionary<ManuelaProperty, Transition> _properties = [];

    public TransitionsExtension()
    { }

    public TransitionsExtension(string properties)
    {
        Properties = properties;
    }

    public string Properties { get; set; } = string.Empty;

    public Transition? All { set => _allTransition = value; }
    public Transition? Background { set => SetProperty(ManuelaProperty.Background, value); }
    public Transition? Margin { set => SetProperty(ManuelaProperty.Margin, value); }
    public Transition? Padding { set => SetProperty(ManuelaProperty.Padding, value); }
    public Transition? BorderColor { set => SetProperty(ManuelaProperty.BorderColor, value); }
    public Transition? BorderThickness { set => SetProperty(ManuelaProperty.BorderThickness, value); }
    public Transition? BorderRadius { set => SetProperty(ManuelaProperty.BorderRadius, value); }
    public Transition? Shadow { set => SetProperty(ManuelaProperty.Shadow, value); }
    public Transition? TextSize { set => SetProperty(ManuelaProperty.TextSize, value); }
    public Transition? LineHeight { set => SetProperty(ManuelaProperty.LineHeight, value); }
    public Transition? FontAttributes { set => SetProperty(ManuelaProperty.FontAttributes, value); }
    public Transition? VerticalTextAlign { set => SetProperty(ManuelaProperty.VerticalTextAlign, value); }
    public Transition? HorizontalTextAlign { set => SetProperty(ManuelaProperty.HorizontalTextAlign, value); }
    public Transition? TextColor { set => SetProperty(ManuelaProperty.TextColor, value); }
    public Transition? Opacity { set => SetProperty(ManuelaProperty.Opacity, value); }
    public Transition? Width { set => SetProperty(ManuelaProperty.Width, value); }
    public Transition? Height { set => SetProperty(ManuelaProperty.Height, value); }
    public Transition? XAnchor { set => SetProperty(ManuelaProperty.XAnchor, value); }
    public Transition? YAnchor { set => SetProperty(ManuelaProperty.YAnchor, value); }
    public Transition? TranslateX { set => SetProperty(ManuelaProperty.TranslateX, value); }
    public Transition? TranslateY { set => SetProperty(ManuelaProperty.TranslateY, value); }
    public Transition? Rotation { set => SetProperty(ManuelaProperty.Rotation, value); }
    public Transition? RotationX { set => SetProperty(ManuelaProperty.RotationX, value); }
    public Transition? RotationY { set => SetProperty(ManuelaProperty.RotationY, value); }
    public Transition? Scale { set => SetProperty(ManuelaProperty.Scale, value); }
    public Transition? ScaleX { set => SetProperty(ManuelaProperty.ScaleX, value); }
    public Transition? ScaleY { set => SetProperty(ManuelaProperty.ScaleY, value); }
    public Transition? MaxWidth { set => SetProperty(ManuelaProperty.MaxWidth, value); }
    public Transition? MaxHeight { set => SetProperty(ManuelaProperty.MaxHeight, value); }
    public Transition? MinWidth { set => SetProperty(ManuelaProperty.MinWidth, value); }
    public Transition? MinHeight { set => SetProperty(ManuelaProperty.MinHeight, value); }
    public Transition? AbsoluteLayoutBounds { set => SetProperty(ManuelaProperty.AbsoluteLayoutBounds, value); }

    public TransitionsCollection ProvideValue(IServiceProvider serviceProvider)
    {
        if (Properties.Length > 0)
        {
            foreach (var property in Properties.Split(','))
            {
                if (property == "All")
                {
                    All = new Transition();
                    continue;
                }

                if (!Enum.TryParse<ManuelaProperty>(property, true, out var manuelaProperty)) continue;
                SetProperty(manuelaProperty, new Transition());
            }
        }

        var collection = new TransitionsCollection();

        foreach (var item in _properties.Values)
            collection.Add(item);

        if (_allTransition is not null) collection.AllTransition = _allTransition;

        return collection;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }

    private void SetProperty(ManuelaProperty property, Transition? transition)
    {
        if (transition is null)
        {
            _ = _properties.Remove(property);
            return;
        }

        transition.Property = property;
        _properties[property] = transition;
    }
}
