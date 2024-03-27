namespace Manuela.Styling.ConditionalStyles.Screen;

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
