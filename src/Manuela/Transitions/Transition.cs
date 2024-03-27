using Manuela.Styling;

namespace Manuela.Transitions;

public class Transition
{
    public ManuelaProperty Property { get; set; }
    public double Duration { get; set; } = 750;
    public Easing Easing { get; set; } = Easing.CubicOut;
}
