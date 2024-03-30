// The namespace not matching the folder is intentional, see #ABOUT-XAML-NS for more info.
// DO NOT MOVE THE NS.

using Manuela.Styling;

namespace Manuela;

public class Transition
{
    public ManuelaProperty Property { get; set; }
    public uint Duration { get; set; } = 300;
    public Easing Easing { get; set; } = Easing.CubicOut;
}
