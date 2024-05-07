// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling;
using Manuela.States.Screen;

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

    public ForceOnIdiom ForceXsOnIdiom { get; set; } = ForceOnIdiom.None;
    public ForceOnIdiom ForceSmOnIdiom { get; set; } = ForceOnIdiom.None;
    public ForceOnIdiom ForceMdOnIdiom { get; set; } = ForceOnIdiom.None;
    public ForceOnIdiom ForceLgOnIdiom { get; set; } = ForceOnIdiom.None;
    public ForceOnIdiom ForceXlOnIdiom { get; set; } = ForceOnIdiom.None;
    public ForceOnIdiom ForceXxlOnIdiom { get; set; } = ForceOnIdiom.None;

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
            var wasForced = false;

            if (OnXs is not null)
            {
                if (IsForcedTo(ForceXsOnIdiom))
                {
                    collection.Add(new Default { Setters = OnXs });
                    wasForced = true;
                }

                screenCondition.Xs = OnXs;
                screenCondition.XsMaxBreakpoint = XsMaxBreakpoint;
            }

            if (OnSm is not null)
            {
                if (IsForcedTo(ForceSmOnIdiom))
                {
                    collection.Add(new Default { Setters = OnSm });
                    wasForced = true;
                }

                screenCondition.Sm = OnSm;
                screenCondition.SmMaxBreakpoint = SmMaxBreakpoint;
            }

            if (OnMd is not null)
            {
                if (IsForcedTo(ForceMdOnIdiom))
                {
                    collection.Add(new Default { Setters = OnMd });
                    wasForced = true;
                }

                screenCondition.Md = OnMd;
                screenCondition.MdMaxBreakpoint = MdMaxBreakpoint;
            }

            if (OnLg is not null)
            {
                if (IsForcedTo(ForceLgOnIdiom))
                {
                    collection.Add(new Default { Setters = OnLg });
                    wasForced = true;
                }

                screenCondition.Lg = OnLg;
                screenCondition.LgMaxBreakpoint = LgMaxBreakpoint;
            }

            if (OnXl is not null)
            {
                if (IsForcedTo(ForceXlOnIdiom))
                {
                    collection.Add(new Default { Setters = OnXl });
                    wasForced = true;
                }

                screenCondition.Xl = OnXl;
                screenCondition.XlMaxBreakpoint = XlMaxBreakpoint;
            }

            if (OnXxl is not null)
            {
                if (IsForcedTo(ForceXxlOnIdiom))
                {
                    collection.Add(new Default { Setters = OnXxl });
                    wasForced = true;
                }

                screenCondition.Xxl = OnXxl;
            }

            if (!wasForced)
                collection.Add(screenCondition);
        }

        return collection;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return ProvideValue(serviceProvider);
    }

    [Flags]
    public enum ForceOnIdiom
    {
        None = 0,
        Phone = 1 << 0,
        Tablet = 1 << 1,
        Desktop = 1 << 2,
        TV = 1 << 3,
        Watch = 1 << 4
    }

    public static bool IsForcedTo(ForceOnIdiom forcedTo)
    {
        var idiom = DeviceInfo.Idiom;

        if (idiom == DeviceIdiom.Phone) return forcedTo.HasFlag(ForceOnIdiom.Phone);
        if (idiom == DeviceIdiom.Tablet) return forcedTo.HasFlag(ForceOnIdiom.Tablet);
        if (idiom == DeviceIdiom.Desktop) return forcedTo.HasFlag(ForceOnIdiom.Desktop);
        if (idiom == DeviceIdiom.TV) return forcedTo.HasFlag(ForceOnIdiom.TV);
        if (idiom == DeviceIdiom.Watch) return forcedTo.HasFlag(ForceOnIdiom.Watch);

        return false;
    }
}
