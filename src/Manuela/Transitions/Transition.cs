using Manuela.Styling;

namespace Manuela.Transitions;

public class Transition
{
    public ManuelaProperty Property { get; set; }
    public uint Duration { get; set; } = 300;
    public Easing Easing { get; set; } = Easing.CubicOut;
}
