// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

namespace Manuela;

public class TransitionDefinitionExtension : IMarkupExtension<Transition>
{
    public Easing? Easing { get; set; }
    public uint Duration { get; set; }

    public Transition ProvideValue(IServiceProvider serviceProvider)
    {
        var t = new Transition();

        if (Easing is not null) t.Easing = Easing;
        if (Duration > 0) t.Duration = Duration;

        return t;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}
