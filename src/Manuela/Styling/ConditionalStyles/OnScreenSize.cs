namespace Manuela.Styling.ConditionalStyles;

public class OnScreenSize : ConditionalStyle
{
    public OnScreenSize(double size)
    {
        Condition = new(visualElement => visualElement.Window?.Width > size)
        {
            Triggers = v => [new(v.Window, [nameof(v.Window.Width)])]
        };
    }
}
