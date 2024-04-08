// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling;
using Manuela.Styling.ConditionalStyles.Screen;

namespace Manuela;

public class StatesExtension : IMarkupExtension<StatesCollection>
{
    public ManuelaSettersDictionary? Checked { get; set; }
    public ManuelaSettersDictionary? Default { get; set; }
    public ManuelaSettersDictionary? Disabled { get; set; }
    public ManuelaSettersDictionary? Focused { get; set; }
    public ManuelaSettersDictionary? Hovered { get; set; }
    public ManuelaSettersDictionary? Pressed { get; set; }
    public ManuelaSettersDictionary? Selected { get; set; }
    public ManuelaSettersDictionary? Unchecked { get; set; }

    public ManuelaSettersDictionary? OnDesktop { get; set; }
    public ManuelaSettersDictionary? OnPhone { get; set; }
    public ManuelaSettersDictionary? OnTablet { get; set; }
    public ManuelaSettersDictionary? OnTV { get; set; }
    public ManuelaSettersDictionary? OnWatch { get; set; }

    public ManuelaSettersDictionary? OnAndroid { get; set; }
    public ManuelaSettersDictionary? OniOS { get; set; }
    public ManuelaSettersDictionary? OnMacOS { get; set; }
    public ManuelaSettersDictionary? OnTizen { get; set; }
    public ManuelaSettersDictionary? OnWatchOS { get; set; }
    public ManuelaSettersDictionary? OnWindows { get; set; }

    public ManuelaSettersDictionary? OnXs { get; set; }
    public ManuelaSettersDictionary? OnSm { get; set; }
    public ManuelaSettersDictionary? OnMd { get; set; }
    public ManuelaSettersDictionary? OnLg { get; set; }
    public ManuelaSettersDictionary? OnXl { get; set; }
    public ManuelaSettersDictionary? OnXxl { get; set; }

    public Breakpoint XsMaxBreakpoint { get; set; } = Breakpoint.Xxl;
    public Breakpoint SmMaxBreakpoint { get; set; } = Breakpoint.Xxl;
    public Breakpoint MdMaxBreakpoint { get; set; } = Breakpoint.Xxl;
    public Breakpoint LgMaxBreakpoint { get; set; } = Breakpoint.Xxl;
    public Breakpoint XlMaxBreakpoint { get; set; } = Breakpoint.Xxl;

    public StatesCollection ProvideValue(IServiceProvider serviceProvider)
    {
        var collection = new StatesCollection();

        if (Checked is not null) collection.Add(new Checked { Setters = Checked });
        if (Default is not null) collection.Add(new Default { Setters = Default });
        if (Disabled is not null) collection.Add(new Disabled { Setters = Disabled });
        if (Focused is not null) collection.Add(new Focused { Setters = Focused });
        if (Hovered is not null) collection.Add(new Hovered { Setters = Hovered });
        if (Pressed is not null) collection.Add(new Pressed { Setters = Pressed });
        if (Selected is not null) collection.Add(new Selected { Setters = Selected });
        if (Unchecked is not null) collection.Add(new Unchecked { Setters = Unchecked });

        if (OnDesktop is not null) collection.Add(new OnDesktop { Setters = OnDesktop });
        if (OnPhone is not null) collection.Add(new OnPhone { Setters = OnPhone });
        if (OnTablet is not null) collection.Add(new OnTablet { Setters = OnTablet });
        if (OnTV is not null) collection.Add(new OnTV { Setters = OnTV });
        if (OnWatch is not null) collection.Add(new OnWatch { Setters = OnWatch });

        if (OnAndroid is not null) collection.Add(new OnAndroid { Setters = OnAndroid });
        if (OniOS is not null) collection.Add(new OnIOS { Setters = OniOS });
        if (OnMacOS is not null) collection.Add(new OnMacOS { Setters = OnMacOS });
        if (OnTizen is not null) collection.Add(new OnTizen { Setters = OnTizen });
        if (OnWatchOS is not null) collection.Add(new OnWatchOS { Setters = OnWatchOS });
        if (OnWindows is not null) collection.Add(new OnWindows { Setters = OnWindows });

        var isResponsive =
            OnXs is not null ||
            OnSm is not null ||
            OnMd is not null ||
            OnLg is not null ||
            OnXl is not null ||
            OnXxl is not null;

        if (isResponsive)
        {
            var screenCondition = new OnScreenSize();

            if (OnXs is not null) { screenCondition.Xs = OnXs; screenCondition.XsMaxBreakpoint = XsMaxBreakpoint; }
            if (OnSm is not null) { screenCondition.Sm = OnSm; screenCondition.SmMaxBreakpoint = SmMaxBreakpoint; }
            if (OnMd is not null) { screenCondition.Md = OnMd; screenCondition.MdMaxBreakpoint = MdMaxBreakpoint; }
            if (OnLg is not null) { screenCondition.Lg = OnLg; screenCondition.LgMaxBreakpoint = LgMaxBreakpoint; }
            if (OnXl is not null) { screenCondition.Xl = OnXl; screenCondition.XlMaxBreakpoint = XlMaxBreakpoint; }
            if (OnXxl is not null) screenCondition.Xxl = OnXxl;

            collection.Add(screenCondition);
        }

        return collection;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }
}
